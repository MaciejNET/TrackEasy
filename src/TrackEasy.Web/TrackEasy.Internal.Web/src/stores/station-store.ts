import {create} from "zustand/react";

export interface StationSearchParams {
  stationName: string;
  cityName: string;
  pageNumber: number;
  pageSize: number;
}

interface StationStore {
  searchParams: StationSearchParams;
  setSearchParams: (params: Partial<StationSearchParams>) => void;
  resetSearchParams: () => void;
}

export const useStationStore = create<StationStore>((set) => ({
  searchParams: {stationName: '', cityName: '', pageNumber: 1, pageSize: 15},
  setSearchParams: (params) =>
    set((state) => ({
      searchParams: {...state.searchParams, ...params},
    })),
  resetSearchParams: () =>
    set({
      searchParams: {stationName: '', cityName: '', pageNumber: 1, pageSize: 15},
    }),
}));