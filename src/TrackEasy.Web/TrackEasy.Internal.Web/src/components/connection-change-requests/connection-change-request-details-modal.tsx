import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {ConnectionChangeRequestDetailsDto, ConnectionRequestType} from "@/schemas/connection-change-request-schema.ts";
import {Badge} from "@/components/ui/badge.tsx";
import {Button} from "@/components/ui/button.tsx";
import {CheckIcon, XIcon} from "lucide-react";

type ConnectionChangeRequestDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  request: ConnectionChangeRequestDetailsDto | null;
  onApprove: (id: string) => void;
  onReject: (id: string) => void;
};

export function ConnectionChangeRequestDetailsModal(props: ConnectionChangeRequestDetailsModalProps) {
  const {open, setOpen, request, onApprove, onReject} = props;

  if (!request) return null;

  const getRequestTypeBadge = (requestType: ConnectionRequestType) => {
    switch (requestType) {
      case ConnectionRequestType.ADD:
        return <Badge className="bg-green-500">Add</Badge>;
      case ConnectionRequestType.UPDATE:
        return <Badge className="bg-blue-500">Update</Badge>;
      case ConnectionRequestType.DELETE:
        return <Badge className="bg-red-500">Delete</Badge>;
      default:
        return null;
    }
  };

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
            Connection Change Request {getRequestTypeBadge(request.requestType)}
          </DialogTitle>
          <DialogDescription>
            Review the connection change request details
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="text-sm font-medium">Name</h3>
              <p className="text-lg">{request.name}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Operator</h3>
              <p className="text-lg">{request.operatorName}</p>
            </div>
          </div>

          {request.schedule && (
            <div>
              <h3 className="text-sm font-medium mb-2">Schedule</h3>
              <div className="border rounded-md p-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <h4 className="text-xs font-medium">Valid From</h4>
                    <p>{formatDate(request.schedule.validFrom)}</p>
                  </div>
                  <div>
                    <h4 className="text-xs font-medium">Valid To</h4>
                    <p>{formatDate(request.schedule.validTo)}</p>
                  </div>
                </div>
                <div className="mt-2">
                  <h4 className="text-xs font-medium">Days of Week</h4>
                  <div className="flex flex-wrap gap-1 mt-1">
                    {request.schedule.daysOfWeek.map((day, index) => (
                      <Badge key={index} variant="outline">{day}</Badge>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          )}

          {request.stations && request.stations.length > 0 && (
            <div>
              <h3 className="text-sm font-medium mb-2">Stations</h3>
              <div className="border rounded-md overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                  <thead className="bg-gray-50">
                    <tr>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Sequence</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Station ID</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Arrival</th>
                      <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Departure</th>
                    </tr>
                  </thead>
                  <tbody className="bg-white divide-y divide-gray-200">
                    {request.stations.sort((a, b) => a.sequenceNumber - b.sequenceNumber).map((station) => (
                      <tr key={station.stationId}>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{station.sequenceNumber}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{station.stationId}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{formatTime(station.arrivalTime)}</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm">{formatTime(station.departureTime)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          <div className="flex justify-end gap-2 pt-4">
            <Button variant="destructive" onClick={() => onReject(request.connectionId)}>
              <XIcon className="mr-2 h-4 w-4" /> Reject
            </Button>
            <Button variant="success" onClick={() => onApprove(request.connectionId)}>
              <CheckIcon className="mr-2 h-4 w-4" /> Approve
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
