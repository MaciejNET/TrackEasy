import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {ConnectionDetailsDto, Currency, DayOfWeek} from "@/schemas/connection-schema.ts";
import {Badge} from "@/components/ui/badge.tsx";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {CheckCircle, XCircle} from "lucide-react";

type ConnectionDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  connection: ConnectionDetailsDto | null;
};

export function ConnectionDetailsModal(props: ConnectionDetailsModalProps) {
  const {open, setOpen, connection} = props;

  if (!connection) return null;

  // Debug the data being received
  console.log("ConnectionDetailsModal - connection:", connection);
  console.log("ConnectionDetailsModal - stations:", connection.stations);

  const formatTime = (time: string | null) => {
    if (!time) return "N/A";
    return time;
  };

  const formatDate = (date: string) => {
    return date;
  };

  const formatCurrency = (currency: Currency) => {
    switch (currency) {
      case Currency.PLN:
        return "PLN";
      case Currency.EUR:
        return "EUR";
      case Currency.USD:
        return "USD";
      default:
        // Map numeric values to currency names
        const currencyNames: Record<number, string> = {
          0: "PLN",
          1: "EUR",
          2: "USD"
        };
        return currencyNames[currency as number] || String(currency);
    }
  };

  const formatDaysOfWeek = (days: DayOfWeek[]) => {
    const dayMap: Record<number, string> = {
      [DayOfWeek.Sunday]: "sun",
      [DayOfWeek.Monday]: "mon",
      [DayOfWeek.Tuesday]: "tue",
      [DayOfWeek.Wednesday]: "wed",
      [DayOfWeek.Thursday]: "thu",
      [DayOfWeek.Friday]: "fri",
      [DayOfWeek.Saturday]: "sat"
    };

    return days.map(day => dayMap[day] || String(day)).join(', ');
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-6xl max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            Connection Details: {connection.name}
            {connection.isActive ? 
              <Badge className="bg-green-500"><CheckCircle className="h-3 w-3 mr-1" /> Active</Badge> : 
              <Badge className="bg-red-500"><XCircle className="h-3 w-3 mr-1" /> Inactive</Badge>}
          </DialogTitle>
          <DialogDescription>
            View detailed information about this connection
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="text-sm font-medium">Name</h3>
              <p className="text-lg">{connection.name}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Train</h3>
              <p className="text-lg">{connection.trainName}</p>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="text-sm font-medium">Price per Kilometer</h3>
              <p className="text-lg">{connection.pricePerKilometer.amount.toFixed(2)} {formatCurrency(connection.pricePerKilometer.currency)}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Seat Reservation</h3>
              <p className="text-lg">{connection.needsSeatReservation ? "Required" : "Not Required"}</p>
            </div>
          </div>

          <div>
            <h3 className="text-sm font-medium mb-2">Schedule</h3>
            <div className="border rounded-md p-4">
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <h4 className="text-xs font-medium">Valid From</h4>
                  <p>{formatDate(connection.validFrom)}</p>
                </div>
                <div>
                  <h4 className="text-xs font-medium">Valid To</h4>
                  <p>{formatDate(connection.validTo)}</p>
                </div>
              </div>
              <div className="mt-2">
                <h4 className="text-xs font-medium">Days of Week</h4>
                <p>{formatDaysOfWeek(connection.daysOfWeek)}</p>
              </div>
            </div>
          </div>

          {connection.stations && Array.isArray(connection.stations) && connection.stations.length > 0 ? (
            <div>
              <h3 className="text-sm font-medium mb-2">Stations</h3>
              <div className="border rounded-md overflow-hidden">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Sequence</TableHead>
                      <TableHead>Station</TableHead>
                      <TableHead>Arrival</TableHead>
                      <TableHead>Departure</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {connection.stations.sort((a, b) => a.sequenceNumber - b.sequenceNumber).map((station) => (
                      <TableRow key={station.id}>
                        <TableCell>{station.sequenceNumber}</TableCell>
                        <TableCell>{station.stationName}</TableCell>
                        <TableCell>{formatTime(station.arrivalTime)}</TableCell>
                        <TableCell>{formatTime(station.departureTime)}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            </div>
          ) : (
            <div>
              <h3 className="text-sm font-medium mb-2">Stations</h3>
              <p className="text-gray-500">No station information available</p>
            </div>
          )}

          {connection.hasPendingRequest && (
            <div className="mt-4 p-4 bg-yellow-50 border border-yellow-200 rounded-md">
              <p className="text-yellow-700">
                This connection has a pending change request. Some changes may not be applied until the request is approved.
              </p>
            </div>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}
