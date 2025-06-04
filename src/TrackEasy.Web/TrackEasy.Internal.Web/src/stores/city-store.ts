import {create} from "zustand/react";

export interface CitySearchParams {
  name: string;
  pageNumber: number;
  pageSize: number;
}

interface CityStore {
  searchParams: CitySearchParams;
  setSearchParams: (params: Partial<CitySearchParams>) => void;
  resetSearchParams: () => void;
}

export const useCityStore = create<CityStore>((set) => ({
  searchParams: {name: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {name: '', pageNumber: 1, pageSize: 15},
    }),
}));