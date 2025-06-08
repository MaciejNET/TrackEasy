import {CoachSearchForm} from "@/components/coaches/coach-search-form.tsx";
import {CoachesList} from "@/components/coaches/coaches-list.tsx";
import {useState, useEffect} from "react";
import {AddEditCoachForm} from "@/components/coaches/add-edit-coach-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteCoach} from "@/components/coaches/delete-coach.tsx";
import {CoachDetailsModal} from "@/components/coaches/coach-details-modal.tsx";
import {CoachDto, CreateCoachCommand, UpdateCoachCommand} from "@/schemas/coach-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createCoach, deleteCoach, updateCoach} from "@/api/coaches-api.ts";
import {useUserStore} from "@/stores/user-store.ts";
import {toast} from "sonner";

export default function Coaches() {
  const queryClient = useQueryClient();
  const {user} = useUserStore();
  const [operatorId, setOperatorId] = useState<string>("");

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedCoach, setSelectedCoach] = useState<CoachDto | null>(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  // Get operatorId from user context
  useEffect(() => {
    if (user && user.operatorId) {
      setOperatorId(user.operatorId);
    } else {
      toast.error("You don't have access to this page");
    }
  }, [user]);

  function handleAdd() {
    setSelectedCoach(null);
    setModalType('Add');
  }

  function handleEdit(coach: CoachDto) {
    setSelectedCoach(coach);
    setModalType("Edit");
  }

  function handleDetails(coach: CoachDto) {
    setSelectedCoach(coach);
    setIsDetailsModalOpen(true);
  }

  function handleDeleteRequest(coach: CoachDto) {
    setSelectedCoach(coach);
    setModalType("Delete");
  }

  async function handleSave(coach: CreateCoachCommand | UpdateCoachCommand) {
    try {
      if (modalType === "Add") {
        await createCoach(operatorId, coach as CreateCoachCommand);
        toast.success("Coach created successfully");
      } else if (modalType === "Edit") {
        await updateCoach(operatorId, (coach as UpdateCoachCommand).id, coach as UpdateCoachCommand);
        toast.success("Coach updated successfully");
      }
      
      await queryClient.invalidateQueries({queryKey: ['coaches', operatorId]});
      setModalType(null);
    } catch (error) {
      console.error(error);
      toast.error("An error occurred");
    }
  }

  async function handleDelete() {
    if (selectedCoach && modalType === "Delete") {
      try {
        await deleteCoach(operatorId, selectedCoach.id);
        toast.success("Coach deleted successfully");
        await queryClient.invalidateQueries({queryKey: ['coaches', operatorId]});
        setModalType(null);
      } catch (error) {
        console.error(error);
        toast.error("An error occurred");
      }
    }
  }

  function onCancel() {
    setModalType(null);
  }

  const isAddEditModalOpen = modalType === "Add" || modalType === "Edit";
  const isDeleteModalOpen = modalType === "Delete";

  // Don't render anything if operatorId is not available
  if (!operatorId) {
    return null;
  }

  return (
    <>
      <CoachSearchForm onAdd={handleAdd}/>
      <CoachesList 
        operatorId={operatorId}
        onEdit={handleEdit} 
        onDelete={handleDeleteRequest} 
        onDetails={handleDetails}
      />
      <AddEditCoachForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        coach={selectedCoach}
        operatorId={operatorId}
      />
      <DeleteCoach
        open={isDeleteModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        onDelete={handleDelete}
        onCancel={onCancel}
      />
      <CoachDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        coach={selectedCoach}
        operatorId={operatorId}
      />
    </>
  );
}