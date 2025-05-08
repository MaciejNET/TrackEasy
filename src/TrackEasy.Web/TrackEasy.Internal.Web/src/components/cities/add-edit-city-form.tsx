import {Dialog, DialogContent, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect} from "react";
import {City, citySchema} from "@/schemas/city-schema.ts";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {useQuery} from "@tanstack/react-query";
import {fetchCountries} from "@/api/cities-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type AddEditCityFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (city: City) => void;
  modalType: ModalType | null;
  city: City | null;
};

export function AddEditCityForm(props: AddEditCityFormProps) {
  const {open, setOpen, handleSave, modalType, city} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
    setValue,
    watch,
  } = useForm<City>({
    resolver: zodResolver(citySchema),
    mode: "onChange",
    defaultValues: {
      id: city?.id || "",
      name: city?.name || "",
      countryId: city?.countryId || "",
    },
  });

  const countryId = watch("countryId");

  const {data: countries, isLoading, isError} = useQuery({
    queryKey: ['countries'],
    queryFn: fetchCountries,
  });

  useEffect(() => {
    const defaults =
      modalType === "Add"
        ? {name: "", countryId: ""}
        : {
          id: city?.id || "",
          name: city?.name || "",
          countryId: city?.countryId || "",
        };
    reset(defaults);
  }, [city, modalType, reset]);

  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} City</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          <div className="flex flex-col">
            <Input id="name" placeholder="Name" {...register("name")} />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>
          <div className="flex flex-col">
            <Select
              value={countryId}
              onValueChange={(value) => setValue("countryId", value)}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select a country" />
              </SelectTrigger>
              <SelectContent>
                {countries?.map((country) => (
                  <SelectItem key={country.id} value={country.id}>
                    {country.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.countryId && <p className="text-red-500 text-sm">{errors.countryId.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}