import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useDiscountStore} from "@/stores/discount-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {DiscountSearchData, discountSearchSchema} from "@/schemas/discount-search-schema.ts";

type DiscountSearchFormProps = {
  onAdd: () => void,
};

export function DiscountSearchForm(props: DiscountSearchFormProps) {
  const {setSearchParams} = useDiscountStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<DiscountSearchData>({
    mode: "onChange",
    resolver: zodResolver(discountSearchSchema),
  });

  const onSubmit = (values: DiscountSearchData) => {
    setSearchParams({
      name: values.name,
      percentage: values.percentage,
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="name" className="col-span-1 col-start-1 font-medium">
          Discounts
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
            id="percentage"
            placeholder="Percentage"
            {...register("percentage")}
          />
          {errors.percentage && (
            <p className="text-red-500 text-sm">{errors.percentage.message}</p>
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
