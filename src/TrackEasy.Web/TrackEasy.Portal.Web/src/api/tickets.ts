import { TicketPriceRequest, TicketPriceResponse, TicketPriceResponseSchema } from "@/lib/schemas";
import { api } from "@/lib/base-api";

export async function getTicketPrice(request: TicketPriceRequest): Promise<TicketPriceResponse> {
  try {
    console.log('Getting ticket price for request:', request);

    const rawData = await api.post<TicketPriceResponse>('/tickets/price', request);

    console.log('Raw ticket price response:', rawData);

    // Validate the response data using Zod
    const validatedData = TicketPriceResponseSchema.parse(rawData);

    console.log('Validated ticket price data:', validatedData);

    return validatedData;
  } catch (error) {
    if (error instanceof Error) {
      console.error('Error getting ticket price:', error.message);
      throw error;
    }
    console.error('Error getting ticket price:', error);
    throw new Error('Failed to get ticket price');
  }
}
