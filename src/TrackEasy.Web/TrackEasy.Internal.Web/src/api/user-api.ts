import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";

const URL = `${BASE_URL}/users`;

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  operatorId?: string;
}

export interface UpdateUserRequest {
  id: string;
  firstName: string;
  lastName: string;
  birthDate: string;
}

export interface ConfirmEmailRequest {
  email: string;
  token: string;
}

export interface GenerateResetPasswordTokenCommand {
  email: string;
  password: string;
}

export interface ResetPasswordCommand {
  email: string;
  token: string;
  newPassword: string;
}

export async function getCurrentUser(): Promise<UserDto> {
  return baseAPI.get<UserDto>(`${URL}/me`);
}

export async function updateUser(request: UpdateUserRequest): Promise<void> {
  return baseAPI.patch<void>(`${URL}/${request.id}/update`, request);
}

export async function deleteUser(id: string): Promise<void> {
  return baseAPI.delete<void>(`${URL}/${id}`);
}

export async function createAdmin(admin: import("@/schemas/admin-schema").CreateAdminCommand): Promise<void> {
  return baseAPI.post<void>(`${URL}/admins`, admin);
}

export async function confirmEmail(request: ConfirmEmailRequest): Promise<void> {
  return baseAPI.post<void>(`${URL}/confirm-email`, request);
}

export async function logout(): Promise<void> {
  return baseAPI.post<void>(`${URL}/logout`, {});
}

export async function generateResetPasswordToken(command: GenerateResetPasswordTokenCommand): Promise<string> {
  return baseAPI.post<string>(`${URL}/reset-password-token`, command);
}

export async function resetPassword(command: ResetPasswordCommand): Promise<void> {
  return baseAPI.post<void>(`${URL}/reset-password`, command);
}
