import {useState} from "react";
import {ConnectionChangeRequestsList} from "@/components/connection-change-requests/connection-change-requests-list.tsx";
import {ConnectionChangeRequestDetailsModal} from "@/components/connection-change-requests/connection-change-request-details-modal.tsx";
import {ConnectionChangeRequestSearchForm} from "@/components/connection-change-requests/connection-change-request-search-form.tsx";
import {ConnectionChangeRequestDetailsDto, ConnectionChangeRequestDto} from "@/schemas/connection-change-request-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {
  approveConnectionChangeRequest,
  fetchConnectionChangeRequestDetails,
  rejectConnectionChangeRequest
} from "@/api/connection-change-requests-api.ts";
import {toast} from "sonner";

export default function ConnectionChangeRequests() {
  const queryClient = useQueryClient();

  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<ConnectionChangeRequestDetailsDto | null>(null);

  async function handleDetails(request: ConnectionChangeRequestDto) {
    try {
      // Fetch the request details when viewing details
      const requestDetails = await fetchConnectionChangeRequestDetails(request.connectionId);
      setSelectedRequest(requestDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching connection change request details:", error);
      toast.error("Failed to load connection change request details");
    }
  }

  async function handleApprove(id: string) {
    try {
      await approveConnectionChangeRequest(id);
      toast.success("Connection change request approved successfully");
      queryClient.invalidateQueries({queryKey: ['connection-change-requests']});
      setIsDetailsModalOpen(false);
    } catch (error) {
      console.error("Error approving connection change request:", error);
      toast.error("Failed to approve connection change request");
    }
  }

  async function handleReject(id: string) {
    try {
      await rejectConnectionChangeRequest(id);
      toast.success("Connection change request rejected successfully");
      queryClient.invalidateQueries({queryKey: ['connection-change-requests']});
      setIsDetailsModalOpen(false);
    } catch (error) {
      console.error("Error rejecting connection change request:", error);
      toast.error("Failed to reject connection change request");
    }
  }

  return (
    <>
      <div className="container mx-auto py-6">
        <ConnectionChangeRequestSearchForm />
        <ConnectionChangeRequestsList 
          onDetails={handleDetails}
          onApprove={(request) => handleApprove(request.connectionId)}
          onReject={(request) => handleReject(request.connectionId)}
        />
      </div>

      <ConnectionChangeRequestDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        request={selectedRequest}
        onApprove={handleApprove}
        onReject={handleReject}
      />
    </>
  );
}
