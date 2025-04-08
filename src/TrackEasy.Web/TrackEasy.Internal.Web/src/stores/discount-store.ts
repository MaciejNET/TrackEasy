import {create} from "zustand/react";

export interface SearchParams {
  name: string;
  percentage?: number;
  pageNumber: number;
  pageSize: number;
}

interface DiscountStore {
  searchParams: SearchParams;
  setSearchParams: (params: Partial<SearchParams>) => void;
  resetSearchParams: () => void;
}

export const useDiscountStore = create<DiscountStore>((set) => ({
  searchParams: {name: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {name: '', pageNumber: 1, pageSize: 20},
    }),
}));