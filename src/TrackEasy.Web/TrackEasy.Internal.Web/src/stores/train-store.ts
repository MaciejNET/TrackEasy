import {create} from "zustand/react";

export interface TrainSearchParams {
  trainName: string;
  pageNumber: number;
  pageSize: number;
}

interface TrainStore {
  searchParams: TrainSearchParams;
  setSearchParams: (params: Partial<TrainSearchParams>) => void;
  resetSearchParams: () => void;
}

export const useTrainStore = create<TrainStore>((set) => ({
  searchParams: {trainName: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {trainName: '', pageNumber: 1, pageSize: 15},
    }),
}));