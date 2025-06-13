import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  ConnectionDto,
  ConnectionDetailsDto,
  CreateConnectionCommand,
  UpdateConnectionCommand,
  UpdateScheduleCommand
} from "@/schemas/connection-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

export async function fetchConnections(operatorId: string, params: {
  name?: string;
  startStation?: string;
  endStation?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<ConnectionDto>> {
  const query = new URLSearchParams();
  if (params.name) query.append("name", params.name);
  if (params.startStation) query.append("startStation", params.startStation);
  if (params.endStation) query.append("endStation", params.endStation);
  query.append("pageNumber", String(params.pageNumber));
  query.append("pageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<ConnectionDto>>(`${BASE_URL}/operators/${operatorId}/connections`, query);
}

export async function fetchConnectionDetails(operatorId: string, connectionId: string): Promise<ConnectionDetailsDto> {
  return baseAPI.get<ConnectionDetailsDto>(`${BASE_URL}/operators/${operatorId}/connections/${connectionId}`);
}

export async function createConnection(operatorId: string, connection: CreateConnectionCommand): Promise<string> {
  return baseAPI.post<string>(`${BASE_URL}/operators/${operatorId}/connections`, connection);
}

export async function updateConnection(operatorId: string, connection: UpdateConnectionCommand): Promise<void> {
  return baseAPI.patch<void>(`${BASE_URL}/operators/${operatorId}/connections/${connection.id}`, connection);
}

export async function updateConnectionSchedule(operatorId: string, schedule: UpdateScheduleCommand): Promise<void> {
  return baseAPI.post<void>(`${BASE_URL}/operators/${operatorId}/connections`, schedule);
}
