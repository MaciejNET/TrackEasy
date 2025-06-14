import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect, useState} from "react";
import {
  CreateStationCommand,
  createStationCommandSchema,
  GeographicalCoordinatesDto,
  StationDetailsDto,
  UpdateStationCommand,
  updateStationCommandSchema
} from "@/schemas/station-schema.ts";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {useQuery} from "@tanstack/react-query";
import {fetchCitiesList, SystemListItemDto} from "@/api/system-lists-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {MapPicker} from "@/components/map/map-picker.tsx";
import ErrorBoundary from "@/components/error-boundary.tsx";

type AddEditStationFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (station: CreateStationCommand | UpdateStationCommand) => void;
  modalType: ModalType | null;
  station: StationDetailsDto | null;
};

export function AddEditStationForm(props: AddEditStationFormProps) {
  const {open, setOpen, handleSave, modalType, station} = props;

  
  const [coordinates, setCoordinates] = useState<GeographicalCoordinatesDto>({
    latitude: 52.2297,
    longitude: 21.0122
  });

  
  const formSchema = modalType === "Add" ? createStationCommandSchema : updateStationCommandSchema;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
    setValue,
    watch,
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
    defaultValues: modalType === "Add"
      ? {
        name: station?.name || "",
        cityId: "",
        geographicalCoordinates: station?.geographicalCoordinates || coordinates
      }
      : {
        id: station?.id || "",
        name: station?.name || "",
        cityId: "",
        geographicalCoordinates: station?.geographicalCoordinates || coordinates
      },
  });

  const cityId = watch("cityId");

  const {data: cities, isLoading, isError} = useQuery({
    queryKey: ['cities-list'],
    queryFn: fetchCitiesList,
  });

  useEffect(() => {
    
    if (station?.geographicalCoordinates) {
      
      const roundedCoordinates = {
        latitude: Math.round(station.geographicalCoordinates.latitude * 100) / 100,
        longitude: Math.round(station.geographicalCoordinates.longitude * 100) / 100
      };
      setCoordinates(roundedCoordinates);
    }

    const defaults = modalType === "Add"
      ? {
        name: "",
        cityId: "",
        geographicalCoordinates: station?.geographicalCoordinates 
          ? {
              latitude: Math.round(station.geographicalCoordinates.latitude * 100) / 100,
              longitude: Math.round(station.geographicalCoordinates.longitude * 100) / 100
            }
          : coordinates
      }
      : {
        id: station?.id || "",
        name: station?.name || "",
        cityId: station?.cityId || "",
        geographicalCoordinates: station?.geographicalCoordinates 
          ? {
              latitude: Math.round(station.geographicalCoordinates.latitude * 100) / 100,
              longitude: Math.round(station.geographicalCoordinates.longitude * 100) / 100
            }
          : coordinates
      };
    reset(defaults);
  }, [station, modalType, reset]);

  const handleMapChange = (newCoordinates: GeographicalCoordinatesDto) => {
    
    const roundedCoordinates = {
      latitude: Math.round(newCoordinates.latitude * 100) / 100,
      longitude: Math.round(newCoordinates.longitude * 100) / 100
    };
    setCoordinates(roundedCoordinates);
    setValue("geographicalCoordinates", roundedCoordinates);
  };

  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>{modalType} Station</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new station" : "Edit station details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          <div className="flex flex-col">
            <Input id="name" placeholder="Name" {...register("name")} />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>
          <div className="flex flex-col">
            <Select
              value={cityId}
              onValueChange={(value) => setValue("cityId", value)}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select a city"/>
              </SelectTrigger>
              <SelectContent>
                {cities?.map((city: SystemListItemDto) => (
                  <SelectItem key={city.id} value={city.id}>
                    {city.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.cityId && <p className="text-red-500 text-sm">{errors.cityId.message}</p>}
          </div>
          <div className="flex flex-col">
            <h3 className="text-sm font-medium mb-2">Location</h3>
            <div className="mb-2">
              <p className="text-xs text-muted-foreground mb-1">Click on the map to select location</p>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="text-xs">Latitude</label>
                  <Input
                    value={coordinates.latitude}
                    onChange={(e) => handleMapChange({...coordinates, latitude: parseFloat(e.target.value)})}
                    type="number"
                    step="0.01"
                    min="-90"
                    max="90"
                  />
                </div>
                <div>
                  <label className="text-xs">Longitude</label>
                  <Input
                    value={coordinates.longitude}
                    onChange={(e) => handleMapChange({...coordinates, longitude: parseFloat(e.target.value)})}
                    type="number"
                    step="0.01"
                    min="-180"
                    max="180"
                  />
                </div>
              </div>
            </div>
            <ErrorBoundary
              fallback={
                <div className="border border-dashed border-gray-300 rounded-md p-4 text-center bg-gray-50 h-[300px] flex items-center justify-center">
                  <div>
                    <p className="text-sm text-gray-500 mb-2">Unable to load map</p>
                    <Button 
                      variant="outline" 
                      size="sm" 
                      onClick={() => window.location.reload()}
                    >
                      Reload page
                    </Button>
                  </div>
                </div>
              }
            >
              <MapPicker 
                coordinates={coordinates} 
                onChange={handleMapChange} 
                height="300px"
                id={`edit-station-map-${modalType}`}
                key={`edit-map-${open ? 'open' : 'closed'}-${modalType}`}
              />
            </ErrorBoundary>
            {errors.geographicalCoordinates && (
              <p className="text-red-500 text-sm mt-1">
                {errors.geographicalCoordinates.message || "Invalid coordinates"}
              </p>
            )}
          </div>
          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
