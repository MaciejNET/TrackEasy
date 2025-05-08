import {CitySearchForm} from "@/components/cities/city-search-form.tsx";
import {CitiesList} from "@/components/cities/cities-list.tsx";
import {useState} from "react";
import {AddEditCityForm} from "@/components/cities/add-edit-city-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteCity} from "@/components/cities/delete-city.tsx";
import {City} from "@/schemas/city-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createCity, deleteCity, updateCity} from "@/api/cities-api.ts";

export default function Cities() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedCity, setSelectedCity] = useState<City | null>(null);

  function handleAdd() {
    setSelectedCity(null);
    setModalType('Add');
  }

  function handleEdit(city: City) {
    setSelectedCity(city);
    setModalType("Edit");
  }

  function handleDeleteRequest(city: City) {
    setSelectedCity(city);
    setModalType("Delete");
  }

  function handleSave(city: City) {
    if (modalType === "Add") {
      createCity(city)
        .then(() => queryClient.invalidateQueries({queryKey: ['cities']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit" && selectedCity) {
      updateCity({...selectedCity, ...city})
        .then(() => queryClient.invalidateQueries({queryKey: ['cities']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedCity && modalType === "Delete") {
      deleteCity(selectedCity.id ?? "")
        .then(() => queryClient.invalidateQueries({queryKey: ['cities']}))
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
      <CitySearchForm onAdd={handleAdd}/>
      <CitiesList onEdit={handleEdit} onDelete={handleDeleteRequest}/>
      <AddEditCityForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        city={selectedCity}
      />
      <DeleteCity
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