import {StationSearchForm} from "@/components/stations/station-search-form.tsx";
import {StationsList} from "@/components/stations/stations-list.tsx";
import {useState} from "react";
import {AddEditStationForm} from "@/components/stations/add-edit-station-form.tsx";
import {ModalType} from "@/types/modals.ts";
import {DeleteStation} from "@/components/stations/delete-station.tsx";
import {StationDetailsModal} from "@/components/stations/station-details-modal.tsx";
import {StationDetailsDto, StationDto, CreateStationCommand, UpdateStationCommand} from "@/schemas/station-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {createStation, deleteStation, fetchStation, updateStation} from "@/api/stations-api.ts";

export default function Stations() {
  const queryClient = useQueryClient();

  const [modalType, setModalType] = useState<ModalType | null>(null);
  const [selectedStation, setSelectedStation] = useState<StationDetailsDto | null>(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  function handleAdd() {
    setSelectedStation(null);
    setModalType('Add');
  }

  async function handleEdit(station: StationDto) {
    try {
      // Fetch the station details when editing
      const stationDetails = await fetchStation(station.id);
      setSelectedStation(stationDetails);
      setModalType("Edit");
    } catch (error) {
      console.error("Error fetching station details:", error);
    }
  }

  async function handleDetails(station: StationDto) {
    try {
      // Fetch the station details when viewing details
      const stationDetails = await fetchStation(station.id);
      setSelectedStation(stationDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching station details:", error);
    }
  }

  function handleDeleteRequest(station: StationDto) {
    // For delete, we only need the ID, so we can create a minimal StationDetailsDto
    setSelectedStation({
      id: station.id,
      name: station.name,
      cityId: "00000000-0000-0000-0000-000000000000", // Default UUID since we don't have it in StationDto
      cityName: station.city,
      // Add default Warsaw coordinates since we don't have them in the StationDto
      geographicalCoordinates: {
        latitude: 52.2297,
        longitude: 21.0122
      }
    });
    setModalType("Delete");
  }

  function handleSave(station: CreateStationCommand | UpdateStationCommand) {
    if (modalType === "Add") {
      createStation(station as CreateStationCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['stations']}))
        .catch((error) => console.error(error));
    } else if (modalType === "Edit") {
      updateStation(station as UpdateStationCommand)
        .then(() => queryClient.invalidateQueries({queryKey: ['stations']}))
        .catch((error) => console.error(error));
    }

    setModalType(null);
  }

  function handleDelete() {
    if (selectedStation && modalType === "Delete") {
      deleteStation(selectedStation.id)
        .then(() => queryClient.invalidateQueries({queryKey: ['stations']}))
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
      <StationSearchForm onAdd={handleAdd}/>
      <StationsList onEdit={handleEdit} onDelete={handleDeleteRequest} onDetails={handleDetails}/>
      <AddEditStationForm
        open={isAddEditModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        handleSave={handleSave}
        modalType={modalType}
        station={selectedStation}
      />
      <DeleteStation
        open={isDeleteModalOpen}
        setOpen={(open) => {
          if (!open) {
            setModalType(null)
          }
        }}
        onDelete={handleDelete}
        onCancel={onCancel}
      />
      <StationDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        station={selectedStation}
      />
    </>
  );
}
