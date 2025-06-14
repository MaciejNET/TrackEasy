import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  CoachDto,
  CoachDetailsDto,
  CreateCoachCommand,
  UpdateCoachCommand
} from "@/schemas/coach-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";


export async function fetchCoaches(operatorId: string, params: {
  code?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<CoachDto>> {
  const query = new URLSearchParams();
  if (params.code) query.append("code", params.code);
  query.append("pageNumber", String(params.pageNumber));
  query.append("pageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<CoachDto>>(`${BASE_URL}/operators/${operatorId}/coaches`, query);
}

export async function fetchCoach(operatorId: string, coachId: string): Promise<CoachDetailsDto> {
  return baseAPI.get<CoachDetailsDto>(`${BASE_URL}/operators/${operatorId}/coaches/${coachId}`);
}

export async function createCoach(operatorId: string, coach: CreateCoachCommand): Promise<void> {
  return baseAPI.post<void>(`${BASE_URL}/operators/${operatorId}/coaches`, coach);
}

export async function updateCoach(operatorId: string, coachId: string, coach: UpdateCoachCommand): Promise<void> {
  return baseAPI.patch<void>(`${BASE_URL}/operators/${operatorId}/coaches/${coachId}`, coach);
}

export async function deleteCoach(operatorId: string, coachId: string): Promise<void> {
  return baseAPI.delete<void>(`${BASE_URL}/operators/${operatorId}/coaches/${coachId}`);
}