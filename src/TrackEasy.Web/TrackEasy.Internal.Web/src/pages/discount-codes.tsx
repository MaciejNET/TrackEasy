import {DiscountCodeSearchForm} from "@/components/discount-codes/discount-code-search-form.tsx";
import {DiscountCodesList} from "@/components/discount-codes/discount-codes-list.tsx";
import {useState} from "react";
import {AddEditDiscountCodeForm} from "@/components/discount-codes/add-edit-discount-code-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteDiscountCode} from "@/components/discount-codes/delete-discount-code.tsx";
import {
  CreateDiscountCodeCommand,
  DiscountCodeDto,
  UpdateDiscountCodeCommand
} from "@/schemas/discount-code-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {
  createDiscountCode,
  deleteDiscountCode,
  updateDiscountCode
} from "@/api/discount-codes-api.ts";

export default function DiscountCodes() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedDiscountCode, setSelectedDiscountCode] = useState<DiscountCodeDto | null>(null);

  function handleAdd() {
    setSelectedDiscountCode(null);
    setModalType('Add');
  }

  function handleEdit(discountCode: DiscountCodeDto) {
    setSelectedDiscountCode(discountCode);
    setModalType("Edit");
  }

  function handleDeleteRequest(discountCode: DiscountCodeDto) {
    setSelectedDiscountCode(discountCode);
    setModalType("Delete");
  }

  function handleSave(discountCode: CreateDiscountCodeCommand | UpdateDiscountCodeCommand) {
    if (modalType === "Add") {
      createDiscountCode(discountCode as CreateDiscountCodeCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['discount-codes']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit" && selectedDiscountCode) {
      updateDiscountCode(discountCode as UpdateDiscountCodeCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['discount-codes']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedDiscountCode && modalType === "Delete") {
      deleteDiscountCode(selectedDiscountCode.id)
        .then(() => queryClient.invalidateQueries({queryKey: ['discount-codes']}))
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
      <DiscountCodeSearchForm onAdd={handleAdd}/>
      <DiscountCodesList onEdit={handleEdit} onDelete={handleDeleteRequest}/>
      <AddEditDiscountCodeForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        discountCode={selectedDiscountCode}
      />
      <DeleteDiscountCode
        open={isDeleteModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        onDelete={handleDelete}
        onCancel={onCancel}
      />
    </>
  );
}