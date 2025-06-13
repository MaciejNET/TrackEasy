import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {RefundRequestDetailsDto, RefundRequestDto} from "@/schemas/refund-request-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

export async function fetchRefundRequests(operatorId: string, params: {
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<RefundRequestDto>> {
  const query = new URLSearchParams();
  query.append("pageNumber", String(params.pageNumber));
  query.append("pageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<RefundRequestDto>>(`${BASE_URL}/operators/${operatorId}/refund-requests`, query);
}

export async function fetchRefundRequestDetails(operatorId: string, refundRequestId: string): Promise<RefundRequestDetailsDto> {
  return baseAPI.get<RefundRequestDetailsDto>(`${BASE_URL}/operators/${operatorId}/refund-requests/${refundRequestId}`);
}

export async function approveRefundRequest(operatorId: string, refundRequestId: string): Promise<void> {
  return baseAPI.post<void>(`${BASE_URL}/operators/${operatorId}/refund-requests/${refundRequestId}/accept`, {});
}

export async function rejectRefundRequest(operatorId: string, refundRequestId: string): Promise<void> {
  return baseAPI.post<void>(`${BASE_URL}/operators/${operatorId}/refund-requests/${refundRequestId}/reject`, {});
}