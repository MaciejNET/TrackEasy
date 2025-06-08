import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  DiscountCodeDto,
  CreateDiscountCodeCommand,
  UpdateDiscountCodeCommand
} from "@/schemas/discount-code-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/discount-codes`;

export async function fetchDiscountCodes(params: {
  code?: string;
  percentage?: number;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<DiscountCodeDto>> {
  const query = new URLSearchParams();
  if (params.code) query.append("Code", params.code);
  if (params.percentage) query.append("Percentage", String(params.percentage));
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<DiscountCodeDto>>(URL, query);
}

export async function fetchDiscountCode(code: string): Promise<DiscountCodeDto> {
  return baseAPI.get<DiscountCodeDto>(`${URL}/${code}`);
}

export async function createDiscountCode(discountCode: CreateDiscountCodeCommand): Promise<void> {
  return baseAPI.post<void>(URL, discountCode);
}

export async function updateDiscountCode(discountCode: UpdateDiscountCodeCommand): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${discountCode.id}`, discountCode);
}

export async function deleteDiscountCode(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}