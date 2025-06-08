import {useState} from "react";
import {useQuery, useQueryClient} from "@tanstack/react-query";
import {fetchAdminsList, SystemListItemDto} from "@/api/system-lists-api";
import {Button} from "@/components/ui/button";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {Loader} from "@/components/loader";
import {ErrorDisplay} from "@/components/error-display";
import {NoData} from "@/components/no-data";
import {ManagersList} from "@/components/user-management/managers-list";
import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog";
import {deleteUser} from "@/api/user-api";
import {toast} from "sonner";
import {Plus} from "lucide-react";
import {AddAdminForm} from "@/components/user-management/add-admin-form";
import {UpdateAdminForm} from "@/components/user-management/update-admin-form";

export function AdminsTab() {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [isAddFormOpen, setIsAddFormOpen] = useState(false);
  const [isUpdateFormOpen, setIsUpdateFormOpen] = useState(false);
  const [selectedAdmin, setSelectedAdmin] = useState<SystemListItemDto | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);
  const queryClient = useQueryClient();

  // Fetch admins list
  const {
    data: admins,
    isLoading,
    isError,
    refetch
  } = useQuery({
    queryKey: ['admins-list'],
    queryFn: fetchAdminsList,
  });

  const handleAddClick = () => {
    setIsAddFormOpen(true);
  };

  const handleEditClick = (admin: SystemListItemDto) => {
    setSelectedAdmin(admin);
    setIsUpdateFormOpen(true);
  };

  const handleDeleteClick = (admin: SystemListItemDto) => {
    setSelectedAdmin(admin);
    setIsDeleteDialogOpen(true);
  };

  const handleOperationSuccess = () => {
    // Refresh the admins list
    refetch();
  };

  const handleDelete = async () => {
    if (!selectedAdmin) return;

    setIsDeleting(true);
    try {
      await deleteUser(selectedAdmin.id);
      toast.success("Admin deleted successfully");

      // Invalidate queries to refresh the data
      await queryClient.invalidateQueries({queryKey: ['admins-list']});
      await refetch();

      setIsDeleteDialogOpen(false);
    } catch (error) {
      console.error("Error deleting admin:", error);
      toast.error("Failed to delete admin");
    } finally {
      setIsDeleting(false);
    }
  };

  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>;

  return (
    <div className="space-y-4">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between">
          <div>
            <CardTitle>Admins</CardTitle>
            <CardDescription>
              View and manage admin users
            </CardDescription>
          </div>
          <Button onClick={handleAddClick} className="ml-auto">
            <Plus className="mr-2 h-4 w-4"/> Add Admin
          </Button>
        </CardHeader>
        <CardContent>
          {admins && admins.length > 0 ? (
            <ManagersList
              managers={admins}
              onEdit={handleEditClick}
              onDelete={handleDeleteClick}
            />
          ) : (
            <NoData/>
          )}
        </CardContent>
      </Card>

      {/* Delete Admin Dialog */}
      <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Delete Admin</DialogTitle>
            <DialogDescription>
              Are you sure you want to delete this admin? This action cannot be undone.
            </DialogDescription>
          </DialogHeader>
          <div className="flex flex-col space-y-4">
            <p>
              You are about to delete <span className="font-semibold">{selectedAdmin?.name}</span>.
            </p>
            <div className="flex justify-end gap-2">
              <Button type="button" variant="outline" onClick={() => setIsDeleteDialogOpen(false)}>
                Cancel
              </Button>
              <Button
                type="button"
                variant="destructive"
                onClick={handleDelete}
                disabled={isDeleting}
              >
                {isDeleting ? "Deleting..." : "Delete Admin"}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      {/* Add Admin Form */}
      <AddAdminForm 
        open={isAddFormOpen} 
        setOpen={setIsAddFormOpen} 
        onSuccess={handleOperationSuccess}
      />

      {/* Update Admin Form */}
      <UpdateAdminForm
        open={isUpdateFormOpen}
        setOpen={setIsUpdateFormOpen}
        admin={selectedAdmin}
        onSuccess={handleOperationSuccess}
      />
    </div>
  );
}
