import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useDiscountCodeStore} from "@/stores/discount-code-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {DiscountCodeSearchData, discountCodeSearchSchema} from "@/schemas/discount-code-search-schema.ts";

type DiscountCodeSearchFormProps = {
  onAdd: () => void,
};

export function DiscountCodeSearchForm(props: DiscountCodeSearchFormProps) {
  const {setSearchParams} = useDiscountCodeStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<DiscountCodeSearchData>({
    mode: "onChange",
    resolver: zodResolver(discountCodeSearchSchema),
  });

  const onSubmit = (values: DiscountCodeSearchData) => {
    setSearchParams({
      code: values.code,
      percentage: values.percentage,
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="code" className="col-span-1 col-start-1 font-medium">
          Discount Codes
        </label>
        <div className="col-span-4">
          <Input
            id="code"
            placeholder="Code"
            {...register("code")}
          />
          {errors.code && (
            <p className="text-red-500 text-sm">{errors.code.message}</p>
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