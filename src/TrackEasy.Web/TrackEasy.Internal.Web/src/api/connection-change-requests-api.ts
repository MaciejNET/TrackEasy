import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  ConnectionChangeRequestDto,
  ConnectionChangeRequestDetailsDto
} from "@/schemas/connection-change-request-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/connection-change-requests`;
const CONNECTIONS_URL = `${BASE_URL}/connections`;

export async function fetchConnectionChangeRequests(params: {
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<ConnectionChangeRequestDto>> {
  const query = new URLSearchParams();
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<ConnectionChangeRequestDto>>(URL, query);
}

export async function fetchConnectionChangeRequestDetails(id: string): Promise<ConnectionChangeRequestDetailsDto> {
  return baseAPI.get<ConnectionChangeRequestDetailsDto>(`${URL}/${id}`);
}

export async function approveConnectionChangeRequest(id: string): Promise<void> {
  return baseAPI.post<void>(`${CONNECTIONS_URL}/${id}/approve`, {});
}

export async function rejectConnectionChangeRequest(id: string): Promise<void> {
  return baseAPI.post<void>(`${CONNECTIONS_URL}/${id}/reject`, {});
}