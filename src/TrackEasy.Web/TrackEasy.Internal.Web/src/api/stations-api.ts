import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  StationDto,
  StationDetailsDto,
  CreateStationCommand,
  UpdateStationCommand
} from "@/schemas/station-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/stations`;

export async function fetchStations(params: {
  stationName?: string;
  cityName?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<StationDto>> {
  const query = new URLSearchParams();
  if (params.stationName) query.append("StationName", params.stationName);
  if (params.cityName) query.append("CityName", params.cityName);
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<StationDto>>(URL, query);
}

export async function fetchStation(id: string): Promise<StationDetailsDto> {
  return baseAPI.get<StationDetailsDto>(`${URL}/${id}`);
}

export async function createStation(station: CreateStationCommand): Promise<void> {
  return baseAPI.post<void>(URL, station);
}

export async function updateStation(station: UpdateStationCommand): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${station.id}`, station);
}

export async function deleteStation(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}
