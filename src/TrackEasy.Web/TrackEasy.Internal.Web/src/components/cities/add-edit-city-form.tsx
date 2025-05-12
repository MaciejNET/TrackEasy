import {Dialog, DialogContent, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import * as React from "react";
import {useEffect, useState} from "react";
import {
  CityDetailsDto,
  CreateCityCommand,
  createCityCommandSchema,
  UpdateCityCommand,
  updateCityCommandSchema
} from "@/schemas/city-schema.ts";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {useQuery} from "@tanstack/react-query";
import {fetchCountries} from "@/api/cities-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {X, Trash2} from "lucide-react";
import {Textarea} from "@/components/ui/textarea.tsx";

type AddEditCityFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (city: CreateCityCommand | UpdateCityCommand) => void;
  modalType: ModalType | null;
  city: CityDetailsDto | null;
};

export function AddEditCityForm(props: AddEditCityFormProps) {
  const {open, setOpen, handleSave, modalType, city} = props;
  const [newFunFact, setNewFunFact] = useState("");
  const [charCount, setCharCount] = useState(0);

  // Use the appropriate schema based on modalType
  const formSchema = modalType === "Add" ? createCityCommandSchema : updateCityCommandSchema;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
    setValue,
    watch,
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
    defaultValues: modalType === "Add"
      ? {
        name: city?.name || "",
        country: city?.country?.id || 0,
        funFacts: city?.funFacts || [],
      }
      : {
        id: city?.id || "",
        name: city?.name || "",
        country: city?.country?.id || 0,
        funFacts: city?.funFacts || [],
      },
  });

  const country = watch("country");
  const funFacts = watch("funFacts");

  const {data: countries, isLoading, isError} = useQuery({
    queryKey: ['countries'],
    queryFn: fetchCountries,
  });

  useEffect(() => {
    const defaults = modalType === "Add"
      ? {
        name: "",
        country: 0,
        funFacts: []
      }
      : {
        id: city?.id || "",
        name: city?.name || "",
        country: city?.country?.id || 0,
        funFacts: city?.funFacts || [],
      };
    reset(defaults);
  }, [city, modalType, reset]);

  const addFunFact = () => {
    if (newFunFact.trim() !== "") {
      setValue("funFacts", [...(funFacts || []), newFunFact]);
      setNewFunFact("");
      setCharCount(0);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter' && e.ctrlKey) {
      e.preventDefault();
      addFunFact();
    }
  };

  const clearAllFunFacts = () => {
    setValue("funFacts", []);
  };

  const removeFunFact = (index: number) => {
    const updatedFacts = [...(funFacts || [])];
    updatedFacts.splice(index, 1);
    setValue("funFacts", updatedFacts);
  };

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
              value={country?.toString()}
              onValueChange={(value) => setValue("country", parseInt(value, 10))}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select a country"/>
              </SelectTrigger>
              <SelectContent>
                {countries?.map((country) => (
                  <SelectItem key={country.id} value={country.id.toString()}>
                    {country.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.country && <p className="text-red-500 text-sm">{errors.country.message}</p>}
          </div>
          <div className="flex flex-col">
            <div className="flex justify-between items-center mb-1">
              <label className="text-sm font-medium">Fun Facts</label>
              {funFacts && funFacts.length > 0 && (
                <div className="flex items-center gap-2">
                  <span
                    className="text-xs text-gray-500">{funFacts.length} fact{funFacts.length !== 1 ? 's' : ''}</span>
                  <Button
                    type="button"
                    onClick={clearAllFunFacts}
                    variant="ghost"
                    size="sm"
                    className="h-6 text-xs text-destructive hover:text-destructive"
                  >
                    Clear All
                  </Button>
                </div>
              )}
            </div>
            <div className="flex flex-col gap-2">
              <div className="relative">
                <Textarea
                  id="newFunFact"
                  placeholder="Add a fun fact (max 255 characters)"
                  value={newFunFact}
                  onChange={(e) => {
                    const value = e.target.value;
                    setNewFunFact(value);
                    setCharCount(value.length);
                  }}
                  onKeyDown={handleKeyDown}
                  className="min-h-[80px] resize-none pr-16"
                  maxLength={255}
                />
                <div className="absolute bottom-2 right-2 text-xs text-muted-foreground">
                  <span className={charCount > 240 ? (charCount > 254 ? "text-destructive" : "text-amber-500") : ""}>
                    {charCount}
                  </span>/255
                </div>
              </div>
              <div className="flex justify-between items-center">
                <span className="text-xs text-muted-foreground">Press Ctrl+Enter to add</span>
                <Button type="button" onClick={addFunFact} variant="outline">
                  Add
                </Button>
              </div>
            </div>
            <div className="mt-3">
              {funFacts && funFacts.length > 0 ? (
                <div className="border rounded-md max-h-60 overflow-y-auto">
                  <ul className="divide-y">
                    {funFacts.map((fact, index) => (
                      <li 
                        key={index} 
                        className="flex items-center justify-between p-3 hover:bg-muted/50 transition-colors"
                      >
                        <span className="text-sm">{fact}</span>
                        <button
                          type="button"
                          onClick={() => removeFunFact(index)}
                          className="text-muted-foreground hover:text-destructive flex items-center justify-center h-6 w-6 rounded-full hover:bg-muted transition-colors"
                        >
                          <Trash2 size={16}/>
                        </button>
                      </li>
                    ))}
                  </ul>
                </div>
              ) : (
                <p className="text-xs text-gray-500 py-2">No fun facts added yet</p>
              )}
            </div>
            {errors.funFacts && <p className="text-red-500 text-sm">{errors.funFacts.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
