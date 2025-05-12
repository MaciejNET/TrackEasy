import {BASE_URL, baseAPI} from "@/lib/api.ts";

const URL = `${BASE_URL}/users`;

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  operatorId?: string;
}

export async function getCurrentUser(): Promise<UserDto> {
  return baseAPI.get<UserDto>(`${URL}/me`);
}