import {DiscountSearchForm} from "@/components/discounts/discount-search-form.tsx";
import {DiscountsList} from "@/components/discounts/discounts-list.tsx";
import {useState} from "react";
import {AddEditDiscountForm} from "@/components/discounts/add-edit-discount-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteDiscount} from "@/components/discounts/delete-discount.tsx";
import {Discount} from "@/schemas/discount-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createDiscount, deleteDiscount, updateDiscount} from "@/api/discounts-api.ts";

export default function Discounts() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedDiscount, setSelectedDiscount] = useState<Discount | null>(null);

  function handleAdd() {
    setSelectedDiscount(null);
    setModalType('Add');
  }

  function handleEdit(discount: Discount) {
    setSelectedDiscount(discount);
    setModalType("Edit");
  }

  function handleDeleteRequest(discount: Discount) {
    setSelectedDiscount(discount);
    setModalType("Delete");
  }

  function handleSave(discount: Discount) {
    if (modalType === "Add") {
      createDiscount(discount)
        .then(() => queryClient.invalidateQueries({queryKey: ['discounts']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit" && selectedDiscount) {
      updateDiscount({...selectedDiscount, ...discount})
        .then(() => queryClient.invalidateQueries({queryKey: ['discounts']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedDiscount && modalType === "Delete") {
      deleteDiscount(selectedDiscount.id ?? "")
        .then(() => queryClient.invalidateQueries({queryKey: ['discounts']}))
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
      <DiscountSearchForm onAdd={handleAdd}/>
      <DiscountsList onEdit={handleEdit} onDelete={handleDeleteRequest}/>
      <AddEditDiscountForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        discount={selectedDiscount}
      />
      <DeleteDiscount
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