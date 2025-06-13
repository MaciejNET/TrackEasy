import {create} from "zustand";

type ConnectionChangeRequestSearchParams = {
  pageNumber: number;
  pageSize: number;
};

type ConnectionChangeRequestStore = {
  searchParams: ConnectionChangeRequestSearchParams;
  setSearchParams: (params: Partial<ConnectionChangeRequestSearchParams>) => void;
  resetSearchParams: () => void;
};

const DEFAULT_PAGE_SIZE = 10;

export const useConnectionChangeRequestStore = create<ConnectionChangeRequestStore>((set) => ({
  searchParams: {
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
  resetSearchParams: () =>
    set({
      searchParams: {
        pageNumber: 1,
        pageSize: DEFAULT_PAGE_SIZE,
      },
    }),
}));