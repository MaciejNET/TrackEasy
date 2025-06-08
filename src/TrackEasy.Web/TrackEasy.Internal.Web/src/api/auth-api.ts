import {baseAPI} from "@/lib/api.ts";
import {BASE_URL} from "@/lib/api-constants.ts";

const URL = `${BASE_URL}/users`;

export interface LoginRequest {
  email: string;
  password: string;
}

export interface TwoFactorRequest {
  code: string;
}

export async function generateToken(request: LoginRequest): Promise<string> {
  return baseAPI.post<string>(`${URL}/token`, {
    email: request.email,
    password: request.password
  });
}

export async function generateTwoFactorToken(request: TwoFactorRequest): Promise<string> {
  return baseAPI.post<string>(`${URL}/token-2fa`, {
    code: request.code
  });
}
