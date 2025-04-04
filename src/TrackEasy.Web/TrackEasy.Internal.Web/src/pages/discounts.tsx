import {DiscountSearchForm} from "@/components/discounts/discount-search-form.tsx";
import {DiscountsList} from "@/components/discounts/discounts-list.tsx";
import {useEffect, useState} from "react";
import {Loader2} from "lucide-react";
import {AddEditDiscountForm} from "@/components/discounts/add-edit-discount-form.tsx";
import {Discount} from "@/components/types/discount.ts";
import {ModalType} from "@/components/types/modals.ts";
import {DeleteDiscount} from "@/components/discounts/delete-discount.tsx";

export default function Discounts() {
  const discountsInit: Discount[] = [
    {
      id: "test",
      name: "First",
      percentage: 10
    },
    {
      id: "test5",
      name: "Third",
      percentage: 20
    },
    {
      id: "test1",
      name: "First",
      percentage: 70
    },
    {
      id: "test2",
      name: "Test",
      percentage: 12
    },
    {
      id: "test3",
      name: "Second",
      percentage: 61
    }
  ]

  const [isSearched, setIsSearched] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isAddEditModalOpen, setIsAddEditModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [modalType, setModalType] = useState<ModalType>("Add");
  const [discounts, setDiscounts] = useState<Discount[]>(discountsInit);
  const [selectedDiscount, setSelectedDiscount] = useState<Discount | null>(null);

  async function onSearch(data?: { name: string; amount: string; }) {
    setIsSearched(false);
    setIsLoading(true);

    await new Promise(resolve => setTimeout(resolve, 2000));

    let filteredDiscounts = discountsInit;
    if (data && (data.name !== "" || data.amount !== "")) {
      filteredDiscounts = discountsInit.filter(discount => {
        const matchName = data.name !== "" ? discount.name.toLowerCase().includes(data.name.toLowerCase()) : true;
        const matchAmount = data.amount !== "" ? discount.percentage === parseInt(data.amount, 10) : true;
        return matchName && matchAmount;
      });
    }

    setDiscounts(filteredDiscounts);
    setIsLoading(false);
    setIsSearched(true);
  }

  useEffect(() => {
    onSearch().then();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  function onAdd() {
    setModalType("Add");
    setSelectedDiscount(null);
    setIsAddEditModalOpen(true);
  }

  function onEdit(discount: Discount) {
    setModalType("Edit");
    setSelectedDiscount(discount);
    setIsAddEditModalOpen(true);
  }

  function onDelete(discount: Discount) {
    setSelectedDiscount(discount);
    setIsDeleteModalOpen(true);
  }

  function onCancel() {
    setIsDeleteModalOpen(false);
  }

  async function addDiscount(discount: Discount) {
    discount.id = Math.random().toString(36);
    const newDiscounts = [...discounts, discount];
    setDiscounts(newDiscounts);
    setIsAddEditModalOpen(false);
    await new Promise(resolve => setTimeout(resolve, 2000));
  }

  async function editDiscount(discount: Discount) {
    const newDiscounts = discounts.map(d => d.id === discount.id ? discount : d);
    setDiscounts(newDiscounts);
    setIsAddEditModalOpen(false);
    await new Promise(resolve => setTimeout(resolve, 2000));
  }

  async function onSubmit(discount: Discount) {
    if (modalType === "Add") {
      await addDiscount(discount);
    } else if (modalType === "Edit") {
      await editDiscount(discount);
    }
  }

  async function deleteDiscount(id: string) {
    const newDiscounts = discounts.filter(discount => discount.id !== id);
    setDiscounts(newDiscounts);
    setIsDeleteModalOpen(false);
    await new Promise(resolve => setTimeout(resolve, 2000));
  }

  return (
    <>
      <DiscountSearchForm onSearch={onSearch} onAdd={onAdd}/>
      {isLoading &&
        <div className="flex justify-center py-2">
          <Loader2 className="animate-spin" size={35}/>
        </div>
      }
      {isSearched &&
        (discounts.length > 0 ?
          <DiscountsList discounts={discounts} onEdit={onEdit} onDelete={onDelete}/>
          : <h2 className="text-center">No results</h2>)
      }
      <AddEditDiscountForm
        open={isAddEditModalOpen}
        setOpen={setIsAddEditModalOpen}
        onSubmit={onSubmit}
        modalType={modalType}
        discount={selectedDiscount}
      />
      <DeleteDiscount
        open={isDeleteModalOpen}
        setOpen={setIsDeleteModalOpen}
        discountId={selectedDiscount ? selectedDiscount.id : ""}
        onDelete={deleteDiscount}
        onCancel={onCancel}
      />
    </>
  );
}