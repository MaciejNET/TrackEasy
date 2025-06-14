
const TOKEN_KEY = 'auth_token';
const USER_KEY = 'auth_user';


export interface UserClaims {
  userId?: string;
  email?: string;
  role?: string;
  isTwoFactorVerified?: boolean;
  operatorId?: string;
  isAuthenticated: boolean;
}

export function saveToken(token: string): void {
  
  let sanitizedToken = token;
  if (sanitizedToken.startsWith('"')) {
    sanitizedToken = sanitizedToken.substring(1);
  }
  if (sanitizedToken.endsWith('"')) {
    sanitizedToken = sanitizedToken.substring(0, sanitizedToken.length - 1);
  }
  localStorage.setItem(TOKEN_KEY, sanitizedToken);
}

export function getToken(): string | null {
  const token = localStorage.getItem(TOKEN_KEY);
  if (!token) return null;

  
  let sanitizedToken = token;
  if (sanitizedToken.startsWith('"')) {
    sanitizedToken = sanitizedToken.substring(1);
  }
  if (sanitizedToken.endsWith('"')) {
    sanitizedToken = sanitizedToken.substring(0, sanitizedToken.length - 1);
  }
  return sanitizedToken;
}

export function removeToken(): void {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
}

export function saveUserClaims(user: UserClaims): void {
  localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function getUserClaims(): UserClaims | null {
  const userJson = localStorage.getItem(USER_KEY);
  if (!userJson) return null;

  try {
    return JSON.parse(userJson) as UserClaims;
  } catch (error) {
    console.error('Error parsing user claims:', error);
    return null;
  }
}

export function parseJwt(token: string): UserClaims | null {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );

    const payload = JSON.parse(jsonPayload);

    const nameIdentifier = payload.nameid || payload.sub || payload.NameIdentifier;
    const email = payload.email || payload.Email;
    const role = payload.role || payload.Role;
    const twoFactorVerified = payload['two-factor-verified'] || payload.TwoFactorVerified || payload.IsTwoFactorVerified;
    const operatorId = payload.operatorId || payload.OperatorId;

    return {
      userId: nameIdentifier,
      email: email,
      role: role,
      isTwoFactorVerified: twoFactorVerified !== undefined ? twoFactorVerified : false,
      operatorId: operatorId,
      isAuthenticated: true
    };
  } catch (error) {
    console.error('Error parsing JWT token:', error);
    return null;
  }
}
