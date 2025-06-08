import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect} from "react";
import {
  CreateOperatorCommand,
  createOperatorCommandSchema,
  OperatorDto,
  UpdateOperatorCommand,
  updateOperatorCommandSchema
} from "@/schemas/operator-schema.ts";

type AddEditOperatorFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (operator: CreateOperatorCommand | UpdateOperatorCommand) => void;
  modalType: ModalType | null;
  operator: OperatorDto | null;
};

export function AddEditOperatorForm(props: AddEditOperatorFormProps) {
  const {open, setOpen, handleSave, modalType, operator} = props;

  // Use the appropriate schema based on modalType
  const formSchema = modalType === "Add" ? createOperatorCommandSchema : updateOperatorCommandSchema;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
    defaultValues: modalType === "Add"
      ? {
        name: "",
        code: "",
      }
      : {
        id: operator?.id || "",
        name: operator?.name || "",
        code: operator?.code || "",
      },
  });

  useEffect(() => {
    const defaults = modalType === "Add"
      ? {
        name: "",
        code: "",
      }
      : {
        id: operator?.id || "",
        name: operator?.name || "",
        code: operator?.code || "",
      };
    reset(defaults);
  }, [operator, modalType, reset]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} Operator</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new operator" : "Edit operator details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          <div className="flex flex-col">
            <Input id="name" placeholder="Name" {...register("name")} />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>
          <div className="flex flex-col">
            <Input
              id="code"
              placeholder="Code (2-3 characters)"
              {...register("code")}
              maxLength={3}
            />
            {errors.code && <p className="text-red-500 text-sm">{errors.code.message}</p>}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}