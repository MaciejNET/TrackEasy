import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";

export interface SystemListItemDto {
  id: string;
  name: string;
}

const URL = `${BASE_URL}/system-lists`;

export async function fetchCitiesList(): Promise<SystemListItemDto[]> {
  return baseAPI.get<SystemListItemDto[]>(`${URL}/cities`);
}

export async function fetchOperatorsList(): Promise<SystemListItemDto[]> {
  return baseAPI.get<SystemListItemDto[]>(`${URL}/operators`);
}

export async function fetchManagersList(operatorId: string): Promise<SystemListItemDto[]> {
  return baseAPI.get<SystemListItemDto[]>(`${URL}/managers/${operatorId}`);
}

export async function fetchAdminsList(): Promise<SystemListItemDto[]> {
  return baseAPI.get<SystemListItemDto[]>(`${URL}/admins`);
}

export async function fetchStationsList(): Promise<SystemListItemDto[]> {
  return baseAPI.get<SystemListItemDto[]>(`${URL}/stations`);
}
