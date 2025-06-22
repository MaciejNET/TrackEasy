import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {CreateOperatorCommand, OperatorDto, UpdateOperatorCommand} from "@/schemas/operator-schema.ts";
import {CreateManagerCommand} from "@/schemas/manager-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/operators`;

export async function fetchOperators(params: {
  name?: string;
  code?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<OperatorDto>> {
  const query = new URLSearchParams();
  if (params.name) query.append("Name", params.name);
  if (params.code) query.append("Code", params.code);
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<OperatorDto>>(URL, query);
}

export async function fetchOperator(id: string): Promise<OperatorDto> {
  return baseAPI.get<OperatorDto>(`${URL}/${id}`);
}

export async function createOperator(operator: CreateOperatorCommand): Promise<string> {
  return baseAPI.post<string>(URL, operator);
}

export async function updateOperator(operator: UpdateOperatorCommand): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${operator.id}`, operator);
}

export async function deleteOperator(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}

export async function createManager(operatorId: string, manager: CreateManagerCommand): Promise<void> {
  
  const managerData = {...manager, operatorId};
  return baseAPI.post<void>(`${URL}/${operatorId}/managers`, managerData);
}
