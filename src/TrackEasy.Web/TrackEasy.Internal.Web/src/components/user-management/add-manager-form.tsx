import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {useQueryClient} from "@tanstack/react-query";
import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog";
import {Input} from "@/components/ui/input";
import {Button} from "@/components/ui/button";
import {Label} from "@/components/ui/label";
import {createManager} from "@/api/operators-api";
import {CreateManagerCommand, createManagerCommandSchema} from "@/schemas/manager-schema";
import {toast} from "sonner";

interface AddManagerFormProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  operatorId: string;
}

export function AddManagerForm({open, setOpen, operatorId}: AddManagerFormProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    formState: {errors, isSubmitting},
    reset
  } = useForm<CreateManagerCommand>({
    resolver: zodResolver(createManagerCommandSchema),
    defaultValues: {
      operatorId: operatorId,
      firstName: "",
      lastName: "",
      email: "",
      dateOfBirth: "",
      password: ""
    }
  });

  const onSubmit = async (data: CreateManagerCommand) => {
    try {
      await createManager(operatorId, data);
      toast.success("Manager created successfully");
      await queryClient.invalidateQueries({queryKey: ['managers-list', operatorId]});
      reset();
      setOpen(false);
    } catch (error) {
      console.error("Error creating manager:", error);
      toast.error("Failed to create manager");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add New Manager</DialogTitle>
          <DialogDescription>
            Create a new manager for the selected operator
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <input type="hidden" {...register("operatorId")} value={operatorId}/>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="firstName">First Name</Label>
              <Input id="firstName" {...register("firstName")} />
              {errors.firstName && (
                <p className="text-red-500 text-sm">{errors.firstName.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="lastName">Last Name</Label>
              <Input id="lastName" {...register("lastName")} />
              {errors.lastName && (
                <p className="text-red-500 text-sm">{errors.lastName.message}</p>
              )}
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input id="email" type="email" {...register("email")} />
            {errors.email && (
              <p className="text-red-500 text-sm">{errors.email.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="dateOfBirth">Date of Birth</Label>
            <Input
              id="dateOfBirth"
              type="date"
              {...register("dateOfBirth")}
            />
            {errors.dateOfBirth && (
              <p className="text-red-500 text-sm">{errors.dateOfBirth.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              type="password"
              {...register("password")}
            />
            {errors.password && (
              <p className="text-red-500 text-sm">{errors.password.message}</p>
            )}
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting ? "Creating..." : "Create Manager"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}