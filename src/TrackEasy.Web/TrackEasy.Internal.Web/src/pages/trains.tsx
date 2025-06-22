import {TrainSearchForm} from "@/components/trains/train-search-form.tsx";
import {TrainsList} from "@/components/trains/trains-list.tsx";
import {useState, useEffect} from "react";
import {AddEditTrainForm} from "@/components/trains/add-edit-train-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteTrain} from "@/components/trains/delete-train.tsx";
import {TrainDetailsModal} from "@/components/trains/train-details-modal.tsx";
import {TrainDto, AddTrainCommand, UpdateTrainCommand} from "@/schemas/train-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createTrain, deleteTrain, fetchTrain, updateTrain} from "@/api/trains-api.ts";
import {useUserStore} from "@/stores/user-store.ts";
import {toast} from "sonner";

export default function Trains() {
  const queryClient = useQueryClient();
  const {user} = useUserStore();
  const [operatorId, setOperatorId] = useState<string>("");

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedTrain, setSelectedTrain] = useState<TrainDto | null>(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  
  useEffect(() => {
    if (user && user.operatorId) {
      setOperatorId(user.operatorId);
    } else {
      toast.error("You don't have access to this page");
    }
  }, [user]);

  function handleAdd() {
    setSelectedTrain(null);
    setModalType('Add');
  }

  function handleEdit(train: TrainDto) {
    setSelectedTrain(train);
    setModalType("Edit");
  }

  function handleDetails(train: TrainDto) {
    setSelectedTrain(train);
    setIsDetailsModalOpen(true);
  }

  function handleDeleteRequest(train: TrainDto) {
    setSelectedTrain(train);
    setModalType("Delete");
  }

  async function handleSave(train: AddTrainCommand | UpdateTrainCommand) {
    try {
      if (modalType === "Add") {
        await createTrain(operatorId, train as AddTrainCommand);
        toast.success("Train created successfully");
      } else if (modalType === "Edit") {
        await updateTrain(operatorId, (train as UpdateTrainCommand).trainId, train as UpdateTrainCommand);
        toast.success("Train updated successfully");
      }
      
      await queryClient.invalidateQueries({queryKey: ['trains', operatorId]});
      setModalType(null);
    } catch (error) {
      console.error(error);
      toast.error("An error occurred");
    }
  }

  async function handleDelete() {
    if (selectedTrain && modalType === "Delete") {
      try {
        await deleteTrain(operatorId, selectedTrain.id);
        toast.success("Train deleted successfully");
        await queryClient.invalidateQueries({queryKey: ['trains', operatorId]});
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

  
  if (!operatorId) {
    return null;
  }

  return (
    <>
      <TrainSearchForm onAdd={handleAdd}/>
      <TrainsList 
        operatorId={operatorId}
        onEdit={handleEdit} 
        onDelete={handleDeleteRequest} 
        onDetails={handleDetails}
      />
      <AddEditTrainForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        train={selectedTrain}
        operatorId={operatorId}
      />
      <DeleteTrain
        open={isDeleteModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        onDelete={handleDelete}
        onCancel={onCancel}
      />
      <TrainDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        train={selectedTrain}
        operatorId={operatorId}
      />
    </>
  );
}