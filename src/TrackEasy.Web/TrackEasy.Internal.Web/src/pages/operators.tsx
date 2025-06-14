import {OperatorSearchForm} from "@/components/operators/operator-search-form.tsx";
import {OperatorsList} from "@/components/operators/operators-list.tsx";
import {useState} from "react";
import {AddEditOperatorForm} from "@/components/operators/add-edit-operator-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteOperator} from "@/components/operators/delete-operator.tsx";
import {OperatorDetailsModal} from "@/components/operators/operator-details-modal.tsx";
import {OperatorDto, CreateOperatorCommand, UpdateOperatorCommand} from "@/schemas/operator-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createOperator, deleteOperator, fetchOperator, updateOperator} from "@/api/operators-api.ts";

export default function Operators() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedOperator, setSelectedOperator] = useState<OperatorDto | null>(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  function handleAdd() {
    setSelectedOperator(null);
    setModalType('Add');
  }

  async function handleEdit(operator: OperatorDto) {
    try {
      
      const operatorDetails = await fetchOperator(operator.id);
      setSelectedOperator(operatorDetails);
      setModalType("Edit");
    } catch (error) {
      console.error("Error fetching operator details:", error);
    }
  }

  async function handleDetails(operator: OperatorDto) {
    try {
      
      const operatorDetails = await fetchOperator(operator.id);
      setSelectedOperator(operatorDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching operator details:", error);
    }
  }

  function handleDeleteRequest(operator: OperatorDto) {
    setSelectedOperator(operator);
    setModalType("Delete");
  }

  function handleSave(operator: CreateOperatorCommand | UpdateOperatorCommand) {
    if (modalType === "Add") {
      createOperator(operator as CreateOperatorCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['operators']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit") {
      updateOperator(operator as UpdateOperatorCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['operators']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedOperator && modalType === "Delete") {
      deleteOperator(selectedOperator.id)
        .then(() => queryClient.invalidateQueries({queryKey: ['operators']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function onCancel() {
    setModalType(null);
  }

  const isAddEditModalOpen = modalType === "Add" || modalType === "Edit";
  const isDeleteModalOpen = modalType === "Delete";

  return (
    <>
      <OperatorSearchForm onAdd={handleAdd}/>
      <OperatorsList onEdit={handleEdit} onDelete={handleDeleteRequest} onDetails={handleDetails}/>
      <AddEditOperatorForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        operator={selectedOperator}
      />
      <DeleteOperator
        open={isDeleteModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        onDelete={handleDelete}
        onCancel={onCancel}
      />
      <OperatorDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        operator={selectedOperator}
      />
    </>
  );
}