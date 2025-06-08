import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {CityDetailsDto} from "@/schemas/city-schema.ts";
import {Info} from "lucide-react";

type CityDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  city: CityDetailsDto | null;
};

export function CityDetailsModal(props: CityDetailsModalProps) {
  const {open, setOpen, city} = props;

  if (!city) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>City Details</DialogTitle>
          <DialogDescription>
            View detailed information about this city
          </DialogDescription>
        </DialogHeader>
        <div className="space-y-4">
          <div>
            <h3 className="text-sm font-medium">Name</h3>
            <p className="text-lg">{city.name}</p>
          </div>
          <div>
            <h3 className="text-sm font-medium">Country</h3>
            <p className="text-lg">{city.country.name}</p>
          </div>
          <div>
            <h3 className="text-sm font-medium">Fun Facts</h3>
            {city.funFacts && city.funFacts.length > 0 ? (
              <div className="border rounded-md mt-2">
                <ul className="divide-y">
                  {city.funFacts.map((fact, index) => (
                    <li 
                      key={index} 
                      className="flex items-center p-3"
                    >
                      <Info size={16} className="text-muted-foreground mr-2 flex-shrink-0" />
                      <span className="text-sm">{fact}</span>
                    </li>
                  ))}
                </ul>
              </div>
            ) : (
              <p className="text-sm text-gray-500">No fun facts available</p>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
