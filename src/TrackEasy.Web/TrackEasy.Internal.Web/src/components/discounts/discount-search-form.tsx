import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";

type FormData = {
  name: string;
  amount: string;
};

type DiscountSearchFormProps = {
  onSearch: (data: FormData) => void,
  onAdd: () => void,
}

export function DiscountSearchForm(props: DiscountSearchFormProps) {
  const {register, handleSubmit, formState: {errors}} = useForm<FormData>({mode: "onChange"});
  const {onSearch, onAdd} = props;

  const onSubmit = (data: FormData) => {
    onSearch(data);
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
            {...register("name", {
              pattern: {value: /^[A-Za-z]+$/, message: "Name must only contain letters"}
            })}
          />
          {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
        </div>
        <div className="col-span-4 col-start-6">
          <Input
            id="amount"
            placeholder="Amount"
            {...register("amount", {
              pattern: {value: /^[0-9]+$/, message: "Amount must be a non-floating point number"}
            })}
          />
          {errors.amount && <p className="text-red-500 text-sm">{errors.amount.message}</p>}
        </div>
        <div className="col-span-2 col-start-11 space-y-2 flex flex-col">
          <Button type="submit">Search</Button>
          <Button type="button" variant="outline" onClick={onAdd}>Add</Button>
        </div>
      </div>
    </form>
  );
}
