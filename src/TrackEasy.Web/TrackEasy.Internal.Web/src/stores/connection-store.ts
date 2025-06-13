import {create} from "zustand";

type ConnectionSearchParams = {
  operatorId: string;
  name?: string;
  startStation?: string;
  endStation?: string;
  pageNumber: number;
  pageSize: number;
};

type ConnectionStore = {
  searchParams: ConnectionSearchParams;
  setSearchParams: (params: Partial<ConnectionSearchParams>) => void;
  resetSearchParams: (operatorId: string) => void;
};

const DEFAULT_PAGE_SIZE = 10;

export const useConnectionStore = create<ConnectionStore>((set) => ({
  searchParams: {
    operatorId: '',
    pageNumber: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  },
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {
        ...state.searchParams,
        ...params,
      },
    })),
  resetSearchParams: (operatorId) =>
    set({
      searchParams: {
        operatorId,
        pageNumber: 1,
        pageSize: DEFAULT_PAGE_SIZE,
      },
    }),
}));