import {CitySearchForm} from "@/components/cities/city-search-form.tsx";
import {CitiesList} from "@/components/cities/cities-list.tsx";
import {useState} from "react";
import {AddEditCityForm} from "@/components/cities/add-edit-city-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteCity} from "@/components/cities/delete-city.tsx";
import {CityDetailsModal} from "@/components/cities/city-details-modal.tsx";
import {CityDetailsDto, CityDto, CreateCityCommand, UpdateCityCommand} from "@/schemas/city-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createCity, deleteCity, fetchCity, updateCity} from "@/api/cities-api.ts";

export default function Cities() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedCity, setSelectedCity] = useState<CityDetailsDto | null>(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  function handleAdd() {
    setSelectedCity(null);
    setModalType('Add');
  }

  async function handleEdit(city: CityDto) {
    try {
      // Fetch the city details when editing
      const cityDetails = await fetchCity(city.id);
      setSelectedCity(cityDetails);
      setModalType("Edit");
    } catch (error) {
      console.error("Error fetching city details:", error);
    }
  }

  async function handleDetails(city: CityDto) {
    try {
      // Fetch the city details when viewing details
      const cityDetails = await fetchCity(city.id);
      setSelectedCity(cityDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching city details:", error);
    }
  }

  function handleDeleteRequest(city: CityDto) {
    // For delete, we only need the ID, so we can create a minimal CityDetailsDto
    setSelectedCity({
      id: city.id,
      name: city.name,
      country: { id: 0, name: city.country }, // We don't need the actual country for delete
      funFacts: []
    });
    setModalType("Delete");
  }

  function handleSave(city: CreateCityCommand | UpdateCityCommand) {
    if (modalType === "Add") {
      createCity(city as CreateCityCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['cities']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit") {
      updateCity(city as UpdateCityCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['cities']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedCity && modalType === "Delete") {
      deleteCity(selectedCity.id)
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
      <CitiesList onEdit={handleEdit} onDelete={handleDeleteRequest} onDetails={handleDetails}/>
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
      <CityDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        city={selectedCity}
      />
    </>
  );
}
