import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { deleteUser } from "@/api/user-api";
import { SystemListItemDto } from "@/api/system-lists-api";
import { useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { useState } from "react";

interface DeleteManagerDialogProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  manager: SystemListItemDto | null;
  onSuccess?: () => void;
}

export function DeleteManagerDialog({ open, setOpen, manager, onSuccess }: DeleteManagerDialogProps) {
  const queryClient = useQueryClient();
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (!manager) return;
    
    setIsDeleting(true);
    try {
      await deleteUser(manager.id);
      toast.success("Manager deleted successfully");
      
      
      await queryClient.invalidateQueries({ queryKey: ['managers-list'] });
      
      setOpen(false);
      if (onSuccess) onSuccess();
    } catch (error) {
      console.error("Error deleting manager:", error);
      toast.error("Failed to delete manager");
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Delete Manager</DialogTitle>
          <DialogDescription>
            Are you sure you want to delete this manager? This action cannot be undone.
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col space-y-4">
          <p>
            You are about to delete <span className="font-semibold">{manager?.name}</span>.
          </p>
          <div className="flex justify-end gap-2">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button 
              type="button" 
              variant="destructive" 
              onClick={handleDelete}
              disabled={isDeleting}
            >
              {isDeleting ? "Deleting..." : "Delete Manager"}
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}