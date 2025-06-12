import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {PaginatedResult} from "@/types/paginated-result.ts";

const URL = `${BASE_URL}/notifications`;

export enum NotificationType {
  REFUND_REQUEST = "REFUND_REQUEST",
  CONNECTION_REQUEST = "CONNECTION_REQUEST",
}

export interface NotificationDto {
  id: string;
  title: string;
  message: string;
  type: NotificationType;
  objectId: string;
  createdAt: string;
}

export async function fetchNotifications(params: {
  pageNumber: number;
  pageSize: number;
}): Promise<PaginatedResult<NotificationDto>> {
  const query = new URLSearchParams();
  query.append("PageNumber", String(params.pageNumber));
  query.append("PageSize", String(params.pageSize));

  return baseAPI.get<PaginatedResult<NotificationDto>>(URL, query);
}

export async function fetchNotificationsCount(): Promise<number> {
  return baseAPI.get<number>(`${URL}/count`);
}

export async function markAsRead(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}
