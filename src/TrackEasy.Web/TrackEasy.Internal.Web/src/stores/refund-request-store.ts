import {create} from "zustand";

type RefundRequestSearchParams = {
  operatorId: string;
  pageNumber: number;
  pageSize: number;
};

type RefundRequestStore = {
  searchParams: RefundRequestSearchParams;
  setSearchParams: (params: Partial<RefundRequestSearchParams>) => void;
  resetSearchParams: (operatorId: string) => void;
};

const DEFAULT_PAGE_SIZE = 10;

export const useRefundRequestStore = create<RefundRequestStore>((set) => ({
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