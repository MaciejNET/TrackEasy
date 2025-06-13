import {create} from "zustand/react";

export interface DiscountCodeSearchParams {
  code: string;
  percentage?: number;
  pageNumber: number;
  pageSize: number;
}

interface DiscountCodeStore {
  searchParams: DiscountCodeSearchParams;
  setSearchParams: (params: Partial<DiscountCodeSearchParams>) => void;
  resetSearchParams: () => void;
}

export const useDiscountCodeStore = create<DiscountCodeStore>((set) => ({
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