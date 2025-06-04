import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useCityStore} from "@/stores/city-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {CitySearchData, citySearchSchema} from "@/schemas/city-search-schema.ts";

type CitySearchFormProps = {
  onAdd: () => void,
};

export function CitySearchForm(props: CitySearchFormProps) {
  const {setSearchParams} = useCityStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<CitySearchData>({
    mode: "onChange",
    resolver: zodResolver(citySearchSchema),
  });

  const onSubmit = (values: CitySearchData) => {
    setSearchParams({
      name: values.name,
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="name" className="col-span-1 col-start-1 font-medium">
          Cities
        </label>
        <div className="col-span-8">
          <Input
            id="name"
            placeholder="Name"
            {...register("name")}
          />
          {errors.name && (
            <p className="text-red-500 text-sm">{errors.name.message}</p>
          )}
        </div>
        <div className="col-span-2 col-start-11 space-y-2 flex flex-col">
          <Button type="submit">Search</Button>
          <Button type="button" variant="outline" onClick={onAdd}>
            Add
          </Button>
        </div>
      </div>
    </form>
  );
}