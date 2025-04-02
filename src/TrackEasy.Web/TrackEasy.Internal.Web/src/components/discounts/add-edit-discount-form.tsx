import {Dialog, DialogContent, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Discount} from "@/components/types/discount.ts";
import {useForm} from "react-hook-form";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/components/types/modals.ts";

type AddEditDiscountFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  onSubmit: (discount: Discount) => void;
  modalType: ModalType;
}

export function AddEditDiscountForm(props: AddEditDiscountFormProps) {
  const {register, handleSubmit, formState: {errors}} = useForm<Discount>({mode: "onChange"})
  const {open, setOpen, onSubmit, modalType} = props;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} Discount</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div className="flex flex-col">
            <Input
              id="name"
              placeholder="Name"
              {...register("name", {
                pattern: {value: /^[A-Za-z]+$/, message: "Name must only contain letters"}
              })}
            />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>
          <div className="flex flex-col">
            <Input
              id="percentage"
              placeholder="Percentage"
              {...register("percentage", {
                pattern: {value: /^[0-9]+$/, message: "Amount must be a non-floating point number"}
              })}
            />
            {errors.percentage && <p className="text-red-500 text-sm">{errors.percentage.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Add</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  )
}