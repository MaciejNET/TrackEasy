import {create} from "zustand/react";

export interface OperatorSearchParams {
  name: string;
  code: string;
  pageNumber: number;
  pageSize: number;
}

interface OperatorStore {
  searchParams: OperatorSearchParams;
  setSearchParams: (params: Partial<OperatorSearchParams>) => void;
  resetSearchParams: () => void;
}

export const useOperatorStore = create<OperatorStore>((set) => ({
  searchParams: {name: '', code: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {name: '', code: '', pageNumber: 1, pageSize: 15},
    }),
}));