import {BASE_URL, baseAPI} from "@/lib/api.ts";
import {
  City,
  CityDto,
  CityDetailsDto,
  CountryDto,
  CreateCityCommand,
  UpdateCityCommand
} from "@/schemas/city-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/cities`;

export async function fetchCities(params: {
  name?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<CityDto>> {
  const query = new URLSearchParams();
  if (params.name) query.append("Name", params.name);
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<CityDto>>(URL, query);
}

export async function fetchCity(id: string): Promise<CityDetailsDto> {
  return baseAPI.get<CityDetailsDto>(`${URL}/${id}`);
}

export async function fetchCountries(): Promise<CountryDto[]> {
  return baseAPI.get<CountryDto[]>(`${URL}/countries`);
}

export async function createCity(city: CreateCityCommand): Promise<string> {
  return baseAPI.post<string>(URL, city);
}

export async function updateCity(city: UpdateCityCommand): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${city.id}`, city);
}

export async function deleteCity(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}
