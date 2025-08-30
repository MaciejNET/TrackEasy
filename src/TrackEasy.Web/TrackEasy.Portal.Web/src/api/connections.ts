import { ConnectionsResponse, ConnectionsResponseSchema } from "@/lib/schemas";
import { api } from "@/lib/base-api";

export interface SearchConnectionsParams {
  startStationId: string;
  endStationId: string;
  departureTime: string;
  cursor?: string;
}

export async function searchConnections(params: SearchConnectionsParams): Promise<ConnectionsResponse> {
  try {
    console.log('searchConnections called with params:', params);

    const queryParams = new URLSearchParams({
      startStationId: params.startStationId,
      endStationId: params.endStationId,
      departureTime: params.departureTime,
    });

    if (params.cursor) {
      queryParams.append('cursor', params.cursor);
    }

    const url = `/connections?${queryParams.toString()}`;
    console.log('Making API request to:', url);

    const rawData = await api.get<ConnectionsResponse>(url);

    console.log('Raw API response:', rawData);

    // Validate the response data using Zod
    const validatedData = ConnectionsResponseSchema.parse(rawData);

    console.log('Validated data:', validatedData);

    return validatedData;
  } catch (error) {
    if (error instanceof Error) {
      console.error('Error searching connections:', error.message);
      throw error;
    }
    console.error('Error searching connections:', error);
    throw new Error('Failed to search connections');
  }
}
