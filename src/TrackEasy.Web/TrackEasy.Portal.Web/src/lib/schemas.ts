import { z } from "zod";

// Generic system list item schema
export const SystemListItemSchema = z.object({
  id: z.string().uuid(),
  name: z.string().min(1),
});

// Type for system list items
export type SystemListItem = z.infer<typeof SystemListItemSchema>;

// Schema for array of system list items
export const SystemListSchema = z.array(SystemListItemSchema);

// Type for system lists
export type SystemList = z.infer<typeof SystemListSchema>;

// Currency enum - API returns integers, we map them to strings
export const CurrencySchema = z.number().transform((val) => {
  switch (val) {
    case 0: return 'PLN';
    case 1: return 'EUR';
    case 2: return 'USD';
    default: return 'PLN'; // fallback
  }
});
export type Currency = z.infer<typeof CurrencySchema>;

// Price schema
export const PriceSchema = z.object({
  amount: z.number(),
  currency: CurrencySchema,
});
export type Price = z.infer<typeof PriceSchema>;

// Connection schema
export const ConnectionSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  operatorName: z.string(),
  operatorCode: z.string(),
  departureTime: z.string(),
  arrivalTime: z.string(),
  departureStationId: z.string().uuid(),
  departureStation: z.string(),
  arrivalStationId: z.string().uuid(),
  arrivalStation: z.string(),
  price: PriceSchema,
  duration: z.string(),
});

// Journey option schema
export const JourneyOptionSchema = z.object({
  connections: z.array(ConnectionSchema),
  transfersCount: z.number(),
  startStation: z.string(),
  endStation: z.string(),
  departureTime: z.string(),
  arrivalTime: z.string(),
  totalDuration: z.string(),
});

// Connections response schema
export const ConnectionsResponseSchema = z.object({
  items: z.array(JourneyOptionSchema),
  nextCursor: z.string().nullable(),
  hasNextPage: z.boolean(),
});

// Types
export type Connection = z.infer<typeof ConnectionSchema>;
export type JourneyOption = z.infer<typeof JourneyOptionSchema>;
export type ConnectionsResponse = z.infer<typeof ConnectionsResponseSchema>;

// Ticket pricing schemas
export const PassengerSchema = z.object({
  firstName: z.string().min(1),
  lastName: z.string().min(1),
  dateOfBirth: z.string(), // YYYY-MM-DD format
  discountId: z.string().uuid().nullable(),
});
export type Passenger = z.infer<typeof PassengerSchema>;

export const TicketConnectionSchema = z.object({
  id: z.string().uuid(),
  startStationId: z.string().uuid(),
  endStationId: z.string().uuid(),
  connectionDate: z.string(), // YYYY-MM-DD format
});
export type TicketConnection = z.infer<typeof TicketConnectionSchema>;

export const TicketPriceRequestSchema = z.object({
  passengers: z.array(PassengerSchema),
  discountCodeId: z.string().uuid().nullable(),
  connections: z.array(TicketConnectionSchema),
});
export type TicketPriceRequest = z.infer<typeof TicketPriceRequestSchema>;

export const TicketPriceResponseSchema = z.object({
  amount: z.number(),
  currency: z.number().transform((val) => {
    switch (val) {
      case 0: return 'PLN';
      case 1: return 'EUR';
      case 2: return 'USD';
      default: return 'PLN'; // fallback
    }
  }),
});
export type TicketPriceResponse = z.infer<typeof TicketPriceResponseSchema>;

// Discount schemas
export const DiscountSchema = z.object({
  id: z.string().uuid(),
  name: z.string().min(1),
});
export type Discount = z.infer<typeof DiscountSchema>;

export const DiscountCodeSchema = z.object({
  id: z.string().uuid(),
  code: z.string().min(1),
  percentage: z.number(),
  from: z.string(), // ISO date string
  to: z.string(), // ISO date string
});
export type DiscountCode = z.infer<typeof DiscountCodeSchema>;
