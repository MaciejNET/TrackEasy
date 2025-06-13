import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {Discount} from "@/schemas/discount-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/discounts`;

export async function fetchDiscounts(params: {
  name?: string;
  percentage?: number;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<Discount>> {
  const query = new URLSearchParams();
  if (params.name) query.append("name", params.name);
  if (params.percentage) query.append("percentage", String(params.percentage));
  query.append("pageNumber", String(params.pageNumber));
  query.append("pageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<Discount>>(URL, query);
}

export async function createDiscount(discount: Discount): Promise<string> {
  return baseAPI.post<string>(URL, discount);
}

export async function updateDiscount(discount: Discount): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${discount.id}`, discount);
}

export async function deleteDiscount(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}
