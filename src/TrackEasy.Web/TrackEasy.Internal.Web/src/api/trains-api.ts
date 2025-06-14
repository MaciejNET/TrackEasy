import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {
  TrainDto,
  TrainDetailsDto,
  AddTrainCommand,
  UpdateTrainCommand
} from "@/schemas/train-schema.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";


export async function fetchTrains(operatorId: string, params: {
  trainName?: string;
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<TrainDto>> {
  const query = new URLSearchParams();
  if (params.trainName) query.append("trainName", params.trainName);
  query.append("pageNumber", String(params.pageNumber));
  query.append("pageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<TrainDto>>(`${BASE_URL}/operators/${operatorId}/trains`, query);
}

export async function fetchTrain(operatorId: string, trainId: string): Promise<TrainDetailsDto> {
  return baseAPI.get<TrainDetailsDto>(`${BASE_URL}/operators/${operatorId}/trains/${trainId}`);
}

export async function createTrain(operatorId: string, train: AddTrainCommand): Promise<string> {
  return baseAPI.post<string>(`${BASE_URL}/operators/${operatorId}/trains`, train);
}

export async function updateTrain(operatorId: string, trainId: string, train: UpdateTrainCommand): Promise<void> {
  return baseAPI.patch<void>(`${BASE_URL}/operators/${operatorId}/trains/${trainId}`, train);
}

export async function deleteTrain(operatorId: string, trainId: string): Promise<void> {
  return baseAPI.delete<void>(`${BASE_URL}/operators/${operatorId}/trains/${trainId}`);
}