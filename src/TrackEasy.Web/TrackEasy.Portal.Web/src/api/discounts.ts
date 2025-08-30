import { Discount, DiscountSchema, DiscountCode, DiscountCodeSchema } from "@/lib/schemas";
import { api } from "@/lib/base-api";
import { z } from "zod";

export async function fetchDiscounts(): Promise<Discount[]> {
  try {
    console.log('Fetching discounts from /system-lists/discounts');

    const rawData = await api.get<Discount[]>('/system-lists/discounts');

    console.log('Raw discounts response:', rawData);

    // Validate the response data using Zod
    const validatedData = z.array(DiscountSchema).parse(rawData);

    console.log('Validated discounts data:', validatedData);

    return validatedData;
  } catch (error) {
    if (error instanceof Error) {
      console.error('Error fetching discounts:', error.message);
      throw error;
    }
    console.error('Error fetching discounts:', error);
    throw new Error('Failed to fetch discounts');
  }
}

export async function validateDiscountCode(code: string): Promise<DiscountCode> {
  try {
    console.log('Validating discount code:', code);

    const rawData = await api.get<DiscountCode>(`/discount-codes/${code}`);

    console.log('Raw discount code response:', rawData);

    // Validate the response data using Zod
    const validatedData = DiscountCodeSchema.parse(rawData);

    console.log('Validated discount code data:', validatedData);

    return validatedData;
  } catch (error) {
    if (error instanceof Error) {
      console.error('Error validating discount code:', error.message);
      throw error;
    }
    console.error('Error validating discount code:', error);
    throw new Error('Failed to validate discount code');
  }
}
