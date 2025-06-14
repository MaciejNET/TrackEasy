import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {RefundRequestDetailsDto} from "@/schemas/refund-request-schema.ts";
import {Badge} from "@/components/ui/badge.tsx";
import {Button} from "@/components/ui/button.tsx";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {CheckIcon, XIcon} from "lucide-react";

type RefundRequestDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  request: RefundRequestDetailsDto | null;
  onApprove: (id: string) => void;
  onReject: (id: string) => void;
};

export function RefundRequestDetailsModal(props: RefundRequestDetailsModalProps) {
  const {open, setOpen, request, onApprove, onReject} = props;

  if (!request) return null;

  
  console.log("RefundRequestDetailsModal - request:", request);
  console.log("RefundRequestDetailsModal - people:", request.people);
  console.log("RefundRequestDetailsModal - stations:", request.stations);

  const formatTime = (time: string | null) => {
    if (!time) return "N/A";
    return time;
  };

  const formatDate = (date: string) => {
    return date;
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-6xl max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            Refund Request for Ticket #{request.ticketNumber}
          </DialogTitle>
          <DialogDescription>
            Review the refund request details
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="text-sm font-medium">Ticket Number</h3>
              <p className="text-lg">{request.ticketNumber}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Status</h3>
              <p className="text-lg">{request.status}</p>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="text-sm font-medium">Operator</h3>
              <p className="text-lg">{request.operatorName} ({request.operatorCode})</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Train</h3>
              <p className="text-lg">{request.trainName}</p>
            </div>
          </div>

          <div>
            <h3 className="text-sm font-medium">Connection Date</h3>
            <p className="text-lg">{formatDate(request.connectionDate)}</p>
          </div>

          <div>
            <h3 className="text-sm font-medium">Reason for Refund</h3>
            <p className="text-lg">{request.reason}</p>
          </div>

          <div>
            <h3 className="text-sm font-medium">Created At</h3>
            <p className="text-lg">{request.createdAt}</p>
          </div>

          {request.people && Array.isArray(request.people) && request.people.length > 0 ? (
            <div>
              <h3 className="text-sm font-medium mb-2">Passengers</h3>
              <div className="border rounded-md overflow-hidden">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>First Name</TableHead>
                      <TableHead>Last Name</TableHead>
                      <TableHead>Date of Birth</TableHead>
                      <TableHead>Discount</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {request.people.map((person, index) => (
                      <TableRow key={index}>
                        <TableCell>{person.firstName}</TableCell>
                        <TableCell>{person.lastName}</TableCell>
                        <TableCell>{formatDate(person.dateOfBirth)}</TableCell>
                        <TableCell>{person.discount || "None"}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            </div>
          ) : (
            <div>
              <h3 className="text-sm font-medium mb-2">Passengers</h3>
              <p className="text-gray-500">No passenger information available</p>
            </div>
          )}

          {request.seatNumbers && request.seatNumbers.length > 0 && (
            <div>
              <h3 className="text-sm font-medium mb-2">Seat Numbers</h3>
              <div className="flex flex-wrap gap-2">
                {request.seatNumbers.map((seatNumber, index) => (
                  <Badge key={index} variant="outline">{seatNumber}</Badge>
                ))}
              </div>
            </div>
          )}

          {request.stations && Array.isArray(request.stations) && request.stations.length > 0 ? (
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
                    {request.stations.sort((a, b) => a.sequenceNumber - b.sequenceNumber).map((station, index) => (
                      <TableRow key={index}>
                        <TableCell>{station.sequenceNumber}</TableCell>
                        <TableCell>{station.name}</TableCell>
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

          <div className="flex justify-end gap-2 pt-4">
            <Button variant="destructive" onClick={() => onReject(request.id)}>
              <XIcon className="mr-2 h-4 w-4" /> Reject
            </Button>
            <Button variant="success" onClick={() => onApprove(request.id)}>
              <CheckIcon className="mr-2 h-4 w-4" /> Approve
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
