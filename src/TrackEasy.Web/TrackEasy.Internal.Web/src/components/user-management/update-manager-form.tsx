import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useQueryClient } from "@tanstack/react-query";
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { updateUser, UpdateUserRequest } from "@/api/user-api";
import { SystemListItemDto } from "@/api/system-lists-api";
import { toast } from "sonner";
import { z } from "zod";

const updateManagerSchema = z.object({
  id: z.string().uuid(),
  firstName: z.string()
    .nonempty({ message: "First name is required" })
    .min(2, { message: "First name must be at least 2 characters" })
    .max(50, { message: "First name must be less than 50 characters" }),
  lastName: z.string()
    .nonempty({ message: "Last name is required" })
    .min(2, { message: "Last name must be at least 2 characters" })
    .max(50, { message: "Last name must be less than 50 characters" }),
  birthDate: z.string()
    .nonempty({ message: "Birth date is required" })
    .regex(/^\d{4}-\d{2}-\d{2}$/, { message: "Date must be in YYYY-MM-DD format" })
});

type UpdateManagerFormValues = z.infer<typeof updateManagerSchema>;

interface UpdateManagerFormProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  manager: SystemListItemDto | null;
  onSuccess?: () => void;
}

export function UpdateManagerForm({ open, setOpen, manager, onSuccess }: UpdateManagerFormProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset
  } = useForm<UpdateManagerFormValues>({
    resolver: zodResolver(updateManagerSchema),
    defaultValues: {
      id: manager?.id || "",
      firstName: "",
      lastName: "",
      birthDate: ""
    }
  });

  const onSubmit = async (data: UpdateManagerFormValues) => {
    if (!manager) return;

    try {
      const request: UpdateUserRequest = {
        id: manager.id,
        firstName: data.firstName,
        lastName: data.lastName,
        birthDate: data.birthDate
      };

      await updateUser(request);
      toast.success("Manager updated successfully");
      
      
      if (manager.id) {
        await queryClient.invalidateQueries({ queryKey: ['managers-list'] });
      }
      
      reset();
      setOpen(false);
      if (onSuccess) onSuccess();
    } catch (error) {
      console.error("Error updating manager:", error);
      toast.error("Failed to update manager");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Update Manager</DialogTitle>
          <DialogDescription>
            Update the manager's information
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <input type="hidden" {...register("id")} value={manager?.id || ""} />

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

          <div className="space-y-2">
            <Label htmlFor="birthDate">Date of Birth</Label>
            <Input
              id="birthDate"
              type="date"
              {...register("birthDate")}
            />
            {errors.birthDate && (
              <p className="text-red-500 text-sm">{errors.birthDate.message}</p>
            )}
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting ? "Updating..." : "Update Manager"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}