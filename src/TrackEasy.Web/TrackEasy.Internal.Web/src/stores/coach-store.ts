import {create} from "zustand/react";

export interface CoachSearchParams {
  code: string;
  pageNumber: number;
  pageSize: number;
}

interface CoachStore {
  searchParams: CoachSearchParams;
  setSearchParams: (params: Partial<CoachSearchParams>) => void;
  resetSearchParams: () => void;
}

export const useCoachStore = create<CoachStore>((set) => ({
  searchParams: {code: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {code: '', pageNumber: 1, pageSize: 15},
    }),
}));