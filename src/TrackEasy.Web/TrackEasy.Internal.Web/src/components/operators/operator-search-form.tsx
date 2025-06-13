import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useOperatorStore} from "@/stores/operator-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {OperatorSearchData, operatorSearchSchema} from "@/schemas/operator-search-schema.ts";

type OperatorSearchFormProps = {
  onAdd: () => void,
};

export function OperatorSearchForm(props: OperatorSearchFormProps) {
  const {setSearchParams} = useOperatorStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<OperatorSearchData>({
    mode: "onChange",
    resolver: zodResolver(operatorSearchSchema),
  });

  const onSubmit = (values: OperatorSearchData) => {
    setSearchParams({
      name: values.name,
      code: values.code,
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="name" className="col-span-1 col-start-1 font-medium">
          Operators
        </label>
        <div className="col-span-4">
          <Input
            id="name"
            placeholder="Name"
            {...register("name")}
          />
          {errors.name && (
            <p className="text-red-500 text-sm">{errors.name.message}</p>
          )}
        </div>
        <div className="col-span-4 col-start-6">
          <Input
            id="code"
            placeholder="Code"
            {...register("code")}
          />
          {errors.code && (
            <p className="text-red-500 text-sm">{errors.code.message}</p>
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