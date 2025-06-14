import {useState} from "react";
import {RefundRequestsList} from "@/components/refund-requests/refund-requests-list.tsx";
import {RefundRequestDetailsModal} from "@/components/refund-requests/refund-request-details-modal.tsx";
import {RefundRequestSearchForm} from "@/components/refund-requests/refund-request-search-form.tsx";
import {RefundRequestDetailsDto, RefundRequestDto} from "@/schemas/refund-request-schema.ts";
import {useQueryClient} from "@tanstack/react-query";
import {
  approveRefundRequest,
  fetchRefundRequestDetails,
  rejectRefundRequest
} from "@/api/refund-requests-api.ts";
import {toast} from "sonner";
import {useRefundRequestStore} from "@/stores/refund-request-store.ts";

export default function RefundRequests() {
  const queryClient = useQueryClient();
  const {searchParams} = useRefundRequestStore();

  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);
  const [selectedRequest, setSelectedRequest] = useState<RefundRequestDetailsDto | null>(null);

  async function handleDetails(request: RefundRequestDto) {
    try {
      
      const requestDetails = await fetchRefundRequestDetails(searchParams.operatorId, request.id);
      setSelectedRequest(requestDetails);
      setIsDetailsModalOpen(true);
    } catch (error) {
      console.error("Error fetching refund request details:", error);
      toast.error("Failed to load refund request details");
    }
  }

  async function handleApprove(id: string) {
    try {
      await approveRefundRequest(searchParams.operatorId, id);
      toast.success("Refund request approved successfully");
      queryClient.invalidateQueries({queryKey: ['refund-requests']});
      setIsDetailsModalOpen(false);
    } catch (error) {
      console.error("Error approving refund request:", error);
      toast.error("Failed to approve refund request");
    }
  }

  async function handleReject(id: string) {
    try {
      await rejectRefundRequest(searchParams.operatorId, id);
      toast.success("Refund request rejected successfully");
      queryClient.invalidateQueries({queryKey: ['refund-requests']});
      setIsDetailsModalOpen(false);
    } catch (error) {
      console.error("Error rejecting refund request:", error);
      toast.error("Failed to reject refund request");
    }
  }

  return (
    <>
      <div className="container mx-auto py-6">
        <RefundRequestSearchForm />
        <RefundRequestsList 
          onDetails={handleDetails}
          onApprove={(request) => handleApprove(request.id)}
          onReject={(request) => handleReject(request.id)}
        />
      </div>

      <RefundRequestDetailsModal
        open={isDetailsModalOpen}
        setOpen={setIsDetailsModalOpen}
        request={selectedRequest}
        onApprove={handleApprove}
        onReject={handleReject}
      />
    </>
  );
}