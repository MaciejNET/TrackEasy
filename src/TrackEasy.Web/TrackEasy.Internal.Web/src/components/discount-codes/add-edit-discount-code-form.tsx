import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useCallback, useEffect} from "react";
import {
  CreateDiscountCodeCommand,
  DiscountCodeDto,
  discountCodeFormSchema,
  UpdateDiscountCodeCommand,
  updateDiscountCodeFormSchema
} from "@/schemas/discount-code-schema.ts";
import {format} from "date-fns";

type AddEditDiscountCodeFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (discountCode: CreateDiscountCodeCommand | UpdateDiscountCodeCommand) => void;
  modalType: ModalType | null;
  discountCode: DiscountCodeDto | null;
};

export function AddEditDiscountCodeForm(props: AddEditDiscountCodeFormProps) {
  const {open, setOpen, handleSave, modalType, discountCode} = props;

  // Use the appropriate schema based on modalType
  const formSchema = modalType === "Add" ? discountCodeFormSchema : updateDiscountCodeFormSchema;

  // Format date for input field - memoized to prevent recreation on each render
  const formatDateForInput = useCallback((date: Date) => {
    return format(date, "yyyy-MM-dd'T'HH:mm");
  }, []);

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
    setValue,
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
    defaultValues: modalType === "Add"
      ? {
        code: "",
        percentage: 0,
        from: formatDateForInput(new Date()),
        to: formatDateForInput(new Date(new Date().setMonth(new Date().getMonth() + 1))) // Default to 1 month from now
      }
      : {
        id: discountCode?.id || "",
        percentage: discountCode?.percentage ?? 0,
        from: discountCode?.from ? formatDateForInput(new Date(discountCode.from)) : formatDateForInput(new Date()),
        to: discountCode?.to ? formatDateForInput(new Date(discountCode.to)) : formatDateForInput(new Date(new Date().setMonth(new Date().getMonth() + 1)))
      },
  });

  useEffect(() => {
    const defaults = modalType === "Add"
      ? {
        code: "",
        percentage: 0,
        from: formatDateForInput(new Date()),
        to: formatDateForInput(new Date(new Date().setMonth(new Date().getMonth() + 1))) // Default to 1 month from now
      }
      : {
        id: discountCode?.id || "",
        percentage: discountCode?.percentage ?? 0,
        from: discountCode?.from ? formatDateForInput(new Date(discountCode.from)) : formatDateForInput(new Date()),
        to: discountCode?.to ? formatDateForInput(new Date(discountCode.to)) : formatDateForInput(new Date(new Date().setMonth(new Date().getMonth() + 1)))
      };
    reset(defaults);
  }, [discountCode, modalType, reset, formatDateForInput]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} Discount Code</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new discount code" : "Edit discount code details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          {modalType === "Add" && (
            <div className="flex flex-col">
              <Input id="code" placeholder="Code" {...register("code")} />
              {errors.code && <p className="text-red-500 text-sm">{errors.code.message}</p>}
            </div>
          )}
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
            <label htmlFor="from" className="text-sm font-medium mb-1">Valid From</label>
            <Input
              id="from"
              type="datetime-local"
              {...register("from")}
            />
            {errors.from && <p className="text-red-500 text-sm">{errors.from.message}</p>}
          </div>
          <div className="flex flex-col">
            <label htmlFor="to" className="text-sm font-medium mb-1">Valid To</label>
            <Input
              id="to"
              type="datetime-local"
              {...register("to")}
            />
            {errors.to && <p className="text-red-500 text-sm">{errors.to.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
