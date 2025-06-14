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
import { UpdateAdminCommand, updateAdminSchema } from "@/schemas/admin-schema";

interface UpdateAdminFormProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  admin: SystemListItemDto | null;
  onSuccess?: () => void;
}

export function UpdateAdminForm({ open, setOpen, admin, onSuccess }: UpdateAdminFormProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset
  } = useForm<UpdateAdminCommand>({
    resolver: zodResolver(updateAdminSchema),
    defaultValues: {
      id: admin?.id || "",
      firstName: "",
      lastName: "",
      birthDate: ""
    }
  });

  const onSubmit = async (data: UpdateAdminCommand) => {
    if (!admin) return;

    try {
      const request: UpdateUserRequest = {
        id: admin.id,
        firstName: data.firstName,
        lastName: data.lastName,
        birthDate: data.birthDate
      };

      await updateUser(request);
      toast.success("Admin updated successfully");
      
      
      await queryClient.invalidateQueries({ queryKey: ['admins-list'] });
      
      reset();
      setOpen(false);
      if (onSuccess) onSuccess();
    } catch (error) {
      console.error("Error updating admin:", error);
      toast.error("Failed to update admin");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Update Admin</DialogTitle>
          <DialogDescription>
            Update the admin's information
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <input type="hidden" {...register("id")} value={admin?.id || ""} />

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
              {isSubmitting ? "Updating..." : "Update Admin"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}