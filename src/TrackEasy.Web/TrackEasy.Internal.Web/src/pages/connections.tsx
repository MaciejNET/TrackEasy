import {useState} from "react";
import {ConnectionsList} from "@/components/connections/connections-list.tsx";
import {ConnectionDetailsModal} from "@/components/connections/connection-details-modal.tsx";
import {ConnectionSearchForm} from "@/components/connections/connection-search-form.tsx";
import {AddEditConnectionForm} from "@/components/connections/add-edit-connection-form.tsx";
import {EditScheduleForm} from "@/components/connections/edit-schedule-form.tsx";
import {
  ConnectionDetailsDto,
  ConnectionDto,
  CreateConnectionCommand,
  UpdateConnectionCommand,
  UpdateScheduleCommand
} from "@/schemas/connection-schema.ts";
import {useConnectionStore} from "@/stores/connection-store.ts";
import {useQueryClient} from "@tanstack/react-query";
import {
  createConnection,
  fetchConnectionDetails,
  updateConnection,
  updateConnectionSchedule
} from "@/api/connections-api.ts";
import {toast} from "sonner";

export default function Connections() {
  const queryClient = useQueryClient();
  const {searchParams} = useConnectionStore();

  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);
  const [isAddEditModalOpen, setIsAddEditModalOpen] = useState(false);
  const [isEditScheduleModalOpen, setIsEditScheduleModalOpen] = useState(false);
  const [selectedConnection, setSelectedConnection] = useState<ConnectionDetailsDto | null>(null);
  const [modalType, setModalType] = useState<"Add" | "Edit" | "EditSchedule" | null>(null);

  async function handleDetails(connection: ConnectionDto) {
    try {
      // Fetch the connection details when viewing details
      const connectionDetails = await fetchConnectionDetails(searchParams.operatorId, connection.id);
      setSelectedConnection(connectionDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching connection details:", error);
      toast.error("Failed to load connection details");
    }
  }

  async function handleEdit(connection: ConnectionDto) {
    try {
      // Fetch the connection details when editing
      const connectionDetails = await fetchConnectionDetails(searchParams.operatorId, connection.id);
      setSelectedConnection(connectionDetails);
      setModalType("Edit");
      setIsAddEditModalOpen(true);
    } catch (error) {
      console.error("Error fetching connection details:", error);
      toast.error("Failed to load connection details");
    }
  }

  async function handleEditSchedule(connection: ConnectionDto) {
    try {
      // Fetch the connection details when editing schedule
      const connectionDetails = await fetchConnectionDetails(searchParams.operatorId, connection.id);
      setSelectedConnection(connectionDetails);
      setModalType("EditSchedule");
      setIsEditScheduleModalOpen(true);
    } catch (error) {
      console.error("Error fetching connection details:", error);
      toast.error("Failed to load connection details");
    }
  }

  function handleAdd() {
    setSelectedConnection(null);
    setModalType("Add");
    setIsAddEditModalOpen(true);
  }

  async function handleSave(command: CreateConnectionCommand | UpdateConnectionCommand | UpdateScheduleCommand) {
    try {
      if (modalType === "EditSchedule") {
        // It's an UpdateScheduleCommand
        await updateConnectionSchedule(searchParams.operatorId, command as UpdateScheduleCommand);
        toast.success("Connection schedule updated successfully");
      } else if (modalType === "Edit") {
        // It's an UpdateConnectionCommand
        await updateConnection(searchParams.operatorId, command as UpdateConnectionCommand);
        toast.success("Connection updated successfully");
      } else {
        // It's a CreateConnectionCommand
        await createConnection(searchParams.operatorId, command as CreateConnectionCommand);
        toast.success("Connection created successfully");
      }

      // Invalidate queries to refresh the data
      queryClient.invalidateQueries({queryKey: ['connections']});

      // Close the modal
      setIsAddEditModalOpen(false);
      setIsEditScheduleModalOpen(false);
    } catch (error) {
      console.error("Error saving connection:", error);
      toast.error("Failed to save connection");
    }
  }

  return (
    <>
      <div className="container mx-auto py-6">
        <ConnectionSearchForm onAdd={handleAdd}/>
        <ConnectionsList
          onDetails={handleDetails}
          onEdit={handleEdit}
          onEditSchedule={handleEditSchedule}
        />
      </div>

      <ConnectionDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        connection={selectedConnection}
      />

      <AddEditConnectionForm
        open={isAddEditModalOpen}
        setOpen={setIsAddEditModalOpen}
        operatorId={searchParams.operatorId}
        connection={selectedConnection}
        onSave={handleSave}
      />

      <EditScheduleForm
        open={isEditScheduleModalOpen}
        setOpen={setIsEditScheduleModalOpen}
        operatorId={searchParams.operatorId}
        connection={selectedConnection}
        onSave={handleSave}
      />
    </>
  );
}
