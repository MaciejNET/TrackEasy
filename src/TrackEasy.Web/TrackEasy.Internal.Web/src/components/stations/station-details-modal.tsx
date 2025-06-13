import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {StationDetailsDto} from "@/schemas/station-schema.ts";
import {MapPicker} from "@/components/map/map-picker.tsx";
import ErrorBoundary from "@/components/error-boundary.tsx";
import {Button} from "@/components/ui/button.tsx";

type StationDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  station: StationDetailsDto | null;
};

export function StationDetailsModal(props: StationDetailsModalProps) {
  const {open, setOpen, station} = props;

  if (!station) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Station Details</DialogTitle>
          <DialogDescription>
            View detailed information about this station
          </DialogDescription>
        </DialogHeader>
        <div className="space-y-4">
          <div>
            <h3 className="text-sm font-medium">Name</h3>
            <p className="text-lg">{station.name}</p>
          </div>
          <div>
            <h3 className="text-sm font-medium">City</h3>
            <p className="text-lg">{station.cityName}</p>
          </div>
          <div>
            <h3 className="text-sm font-medium">Location</h3>
            <div className="grid grid-cols-2 gap-4 mb-2">
              <div>
                <p className="text-xs text-muted-foreground">Latitude</p>
                <p className="text-sm">{station.geographicalCoordinates.latitude}</p>
              </div>
              <div>
                <p className="text-xs text-muted-foreground">Longitude</p>
                <p className="text-sm">{station.geographicalCoordinates.longitude}</p>
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
                coordinates={station.geographicalCoordinates} 
                readOnly={true}
                height="300px"
                id={`details-station-map-${station.id}`}
                key={`details-map-${open ? 'open' : 'closed'}-${station.id}`}
              />
            </ErrorBoundary>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
