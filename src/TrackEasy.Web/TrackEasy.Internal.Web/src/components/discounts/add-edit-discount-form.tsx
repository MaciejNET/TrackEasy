import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect} from "react";
import {Discount, discountSchema} from "@/schemas/discount-schema.ts";

type AddEditDiscountFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (discount: Discount) => void;
  modalType: ModalType | null;
  discount: Discount | null;
};

export function AddEditDiscountForm(props: AddEditDiscountFormProps) {
  const {open, setOpen, handleSave, modalType, discount} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
  } = useForm<Discount>({
    resolver: zodResolver(discountSchema),
    mode: "onChange",
    defaultValues: {
      id: discount?.id || "",
      name: discount?.name || "",
      percentage: discount?.percentage ?? 0,
    },
  });

  useEffect(() => {
    const defaults =
      modalType === "Add"
        ? {name: "", percentage: 0}
        : {
          id: discount?.id || "",
          name: discount?.name || "",
          percentage: discount?.percentage ?? 0,
        };
    reset(defaults);
  }, [discount, modalType, reset]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} Discount</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new discount" : "Edit discount details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          <div className="flex flex-col">
            <Input id="name" placeholder="Name" {...register("name")} />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>
          <div className="flex flex-col">
            <Input
              id="percentage"
              type="number"
              placeholder="Percentage"
              {...register("percentage", {valueAsNumber: true})}
            />
            {errors.percentage && <p className="text-red-500 text-sm">{errors.percentage.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
