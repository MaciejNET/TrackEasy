import {BASE_URL, baseAPI} from "@/lib/api.ts";
import {City, Country} from "@/schemas/city-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/cities`;

export async function fetchCities(params: {
  name?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<City>> {
  const query = new URLSearchParams();
  if (params.name) query.append("Name", params.name);
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<City>>(URL, query);
}

export async function fetchCity(id: string): Promise<City> {
  return baseAPI.get<City>(`${URL}/${id}`);
}

export async function fetchCountries(): Promise<Country[]> {
  return baseAPI.get<Country[]>(`${URL}/countries`);
}

export async function createCity(city: City): Promise<string> {
  return baseAPI.post<string>(URL, city);
}

export async function updateCity(city: City): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${city.id}`, city);
}

export async function deleteCity(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}