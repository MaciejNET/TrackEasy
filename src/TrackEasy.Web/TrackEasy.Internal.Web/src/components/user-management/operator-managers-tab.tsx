import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { fetchOperatorsList, fetchManagersList, SystemListItemDto } from "@/api/system-lists-api";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Loader } from "@/components/loader";
import { ErrorDisplay } from "@/components/error-display";
import { NoData } from "@/components/no-data";
import { AddManagerForm } from "@/components/user-management/add-manager-form";
import { ManagersList } from "@/components/user-management/managers-list";
import { UpdateManagerForm } from "@/components/user-management/update-manager-form";
import { DeleteManagerDialog } from "@/components/user-management/delete-manager-dialog";

export function OperatorManagersTab() {
  const [selectedOperatorId, setSelectedOperatorId] = useState<string | null>(null);
  const [isAddFormOpen, setIsAddFormOpen] = useState(false);
  const [isUpdateFormOpen, setIsUpdateFormOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedManager, setSelectedManager] = useState<SystemListItemDto | null>(null);

  // Fetch operators list
  const { 
    data: operators, 
    isLoading: isLoadingOperators, 
    isError: isErrorOperators 
  } = useQuery({
    queryKey: ['operators-list'],
    queryFn: fetchOperatorsList,
  });

  // Fetch managers list for selected operator
  const { 
    data: managers, 
    isLoading: isLoadingManagers, 
    isError: isErrorManagers 
  } = useQuery({
    queryKey: ['managers-list', selectedOperatorId],
    queryFn: () => selectedOperatorId ? fetchManagersList(selectedOperatorId) : Promise.resolve([]),
    enabled: !!selectedOperatorId,
  });

  const handleOperatorChange = (value: string) => {
    setSelectedOperatorId(value);
  };

  const handleAddClick = () => {
    setIsAddFormOpen(true);
  };

  const handleEditClick = (manager: SystemListItemDto) => {
    setSelectedManager(manager);
    setIsUpdateFormOpen(true);
  };

  const handleDeleteClick = (manager: SystemListItemDto) => {
    setSelectedManager(manager);
    setIsDeleteDialogOpen(true);
  };

  const handleOperationSuccess = () => {
    // Refresh the managers list
    if (selectedOperatorId) {
      // The query will be invalidated by the form components
    }
  };

  if (isLoadingOperators) return <Loader />;
  if (isErrorOperators) return <ErrorDisplay />;

  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <CardTitle>Operator Managers</CardTitle>
          <CardDescription>
            Select an operator to view and manage its managers
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex items-center gap-4 mb-6">
            <div className="w-1/3">
              <Select onValueChange={handleOperatorChange} value={selectedOperatorId || undefined}>
                <SelectTrigger>
                  <SelectValue placeholder="Select an operator" />
                </SelectTrigger>
                <SelectContent>
                  {operators?.map((operator: SystemListItemDto) => (
                    <SelectItem key={operator.id} value={operator.id}>
                      {operator.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
            <Button 
              onClick={handleAddClick} 
              disabled={!selectedOperatorId}
            >
              Add Manager
            </Button>
          </div>

          {selectedOperatorId ? (
            isLoadingManagers ? (
              <Loader />
            ) : isErrorManagers ? (
              <ErrorDisplay />
            ) : managers && managers.length > 0 ? (
              <ManagersList 
                managers={managers} 
                onEdit={handleEditClick}
                onDelete={handleDeleteClick}
              />
            ) : (
              <NoData message="No managers found for this operator" />
            )
          ) : (
            <div className="text-center p-8 text-muted-foreground">
              Please select an operator to view its managers
            </div>
          )}
        </CardContent>
      </Card>

      <AddManagerForm 
        open={isAddFormOpen} 
        setOpen={setIsAddFormOpen} 
        operatorId={selectedOperatorId || ""} 
        onSuccess={handleOperationSuccess}
      />

      <UpdateManagerForm
        open={isUpdateFormOpen}
        setOpen={setIsUpdateFormOpen}
        manager={selectedManager}
        onSuccess={handleOperationSuccess}
      />

      <DeleteManagerDialog
        open={isDeleteDialogOpen}
        setOpen={setIsDeleteDialogOpen}
        manager={selectedManager}
        onSuccess={handleOperationSuccess}
      />
    </div>
  );
}
