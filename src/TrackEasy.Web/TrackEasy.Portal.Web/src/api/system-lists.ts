import { SystemListItem, SystemListSchema } from "@/lib/schemas";
import { api } from "@/lib/base-api";

/**
 * Generic function to fetch system lists
 * @param listType - The type of system list to fetch (e.g., 'stations', 'trains', 'routes')
 * @returns Promise<SystemListItem[]> - Array of system list items
 */
export async function fetchSystemList(listType: string): Promise<SystemListItem[]> {
  try {
    const rawData = await api.get<SystemListItem[]>(`/system-lists/${listType}`);

    // Validate the response data using Zod
    const validatedData = SystemListSchema.parse(rawData);

    return validatedData;
  } catch (error) {
    if (error instanceof Error) {
      console.error(`Error fetching ${listType}:`, error.message);
      throw error;
    }
    console.error(`Error fetching ${listType}:`, error);
    throw new Error(`Failed to fetch ${listType}`);
  }
}

// Specific functions for common system lists
export const fetchStations = () => fetchSystemList('stations');
export const fetchTrains = () => fetchSystemList('trains');
export const fetchRoutes = () => fetchSystemList('routes');
export const fetchPlatforms = () => fetchSystemList('platforms');
