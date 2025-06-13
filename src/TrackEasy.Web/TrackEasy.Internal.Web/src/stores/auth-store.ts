import {create} from "zustand/react";
import {generateToken, generateTwoFactorToken, LoginRequest, TwoFactorRequest} from "@/api/auth-api.ts";
import {getToken, getUserClaims, parseJwt, removeToken, saveToken, saveUserClaims, UserClaims} from "@/lib/auth-storage.ts";
import {logout as logoutApi} from "@/api/user-api.ts";

interface AuthState {
  token: string | null;
  user: UserClaims | null;
  isLoading: boolean;
  error: string | null;
  needsTwoFactor: boolean;

  // Actions
  login: (credentials: LoginRequest) => Promise<boolean>;
  submitTwoFactorCode: (request: TwoFactorRequest) => Promise<boolean>;
  logout: () => Promise<void>;
  checkAuth: () => boolean;
}

export const useAuthStore = create<AuthState>((set, get) => ({
  token: getToken(),
  user: getUserClaims(),
  isLoading: false,
  error: null,
  needsTwoFactor: false,

  login: async (credentials) => {
    set({isLoading: true, error: null});

    try {
      const token = await generateToken(credentials);
      saveToken(token);

      const claims = parseJwt(token);

      if (claims) {
        saveUserClaims(claims);

        // Check if 2FA is required
        if (claims.isAuthenticated && claims.isTwoFactorVerified === false) {
          set({
            token,
            user: claims,
            isLoading: false,
            needsTwoFactor: true
          });
          return true;
        }

        // User is fully authenticated
        if (claims.isAuthenticated && claims.isTwoFactorVerified) {
          set({
            token,
            user: claims,
            isLoading: false,
            needsTwoFactor: false
          });
          return true;
        }
      }

      // Authentication failed
      set({
        isLoading: false,
        error: "Authentication failed",
        token: null,
        user: null
      });
      return false;

    } catch (error) {
      set({
        isLoading: false,
        error: error instanceof Error ? error.message : "Authentication failed",
        token: null,
        user: null
      });
      return false;
    }
  },

  submitTwoFactorCode: async (request) => {
    set({isLoading: true, error: null});

    try {
      const token = await generateTwoFactorToken(request);
      saveToken(token);

      const claims = parseJwt(token);

      if (claims && claims.isAuthenticated && claims.isTwoFactorVerified) {
        saveUserClaims(claims);
        set({
          token,
          user: claims,
          isLoading: false,
          needsTwoFactor: false
        });
        return true;
      }

      // 2FA failed
      set({
        isLoading: false,
        error: "Two-factor authentication failed",
      });
      return false;

    } catch (error) {
      set({
        isLoading: false,
        error: error instanceof Error ? error.message : "Two-factor authentication failed",
      });
      return false;
    }
  },

  logout: async () => {
    try {
      // Send logout request to the server
      await logoutApi();
    } catch (error) {
      console.error('Error during logout:', error);
    } finally {
      // Always clear local state even if the API call fails
      removeToken();
      set({
        token: null,
        user: null,
        needsTwoFactor: false,
        error: null
      });
    }
  },

  checkAuth: () => {
    const {token, user} = get();
    return !!(token && user && user.isAuthenticated && user.isTwoFactorVerified);
  }
}));
