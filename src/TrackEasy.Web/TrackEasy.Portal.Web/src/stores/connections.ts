import { create } from 'zustand';
import { Station, ConnectionsResponse, JourneyOption } from '@/lib/schemas';

interface SearchParams {
  startStationId: string;
  endStationId: string;
  date: string;
  time: string;
}

interface ConnectionsStore {
  // Search parameters
  searchParams: SearchParams | null;

  // Search results
  searchResults: ConnectionsResponse | null;

  // Loading states
  isSearching: boolean;

  // Selected journey for ticket purchase
  selectedJourney: JourneyOption | null;

  // Actions
  setSearchParams: (params: SearchParams) => void;
  setSearchResults: (results: ConnectionsResponse) => void;
  setIsSearching: (loading: boolean) => void;
  setSelectedJourney: (journey: JourneyOption) => void;
  clearSearch: () => void;
}

export const useConnectionsStore = create<ConnectionsStore>((set, get) => {
  console.log('Creating connections store');

  return {
    // Initial state
    searchParams: null,
    searchResults: null,
    isSearching: false,
    selectedJourney: null,

    // Actions
    setSearchParams: (params) => {
      console.log('Setting search params in store:', params);
      console.log('Previous state:', get());
      set({ searchParams: params });
      console.log('New state:', get());
    },
    setSearchResults: (results) => {
      console.log('Setting search results in store:', results);
      set({ searchResults: results });
    },
    setIsSearching: (loading) => {
      console.log('Setting isSearching in store:', loading);
      set({ isSearching: loading });
    },
    setSelectedJourney: (journey) => {
      console.log('Setting selected journey in store:', journey);
      set({ selectedJourney: journey });
    },
    clearSearch: () => {
      console.log('Clearing search in store');
      set({
        searchParams: null,
        searchResults: null,
        isSearching: false,
        selectedJourney: null,
      });
    },
  };
});
