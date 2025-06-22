import {create} from "zustand/react";
import {getCurrentUser, UserDto} from "@/api/user-api.ts";

interface UserState {
  user: UserDto | null;
  isLoading: boolean;
  error: string | null;
  
  
  fetchUser: () => Promise<void>;
  clearUser: () => void;
}

export const useUserStore = create<UserState>((set) => ({
  user: null,
  isLoading: false,
  error: null,
  
  fetchUser: async () => {
    set({isLoading: true, error: null});
    
    try {
      const userData = await getCurrentUser();
      set({
        user: userData,
        isLoading: false
      });
    } catch (error) {
      set({
        isLoading: false,
        error: error instanceof Error ? error.message : "Failed to fetch user data"
      });
    }
  },
  
  clearUser: () => {
    set({
      user: null,
      error: null
    });
  }
}));