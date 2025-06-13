import {z} from "zod";

// Schema for RefundRequestDto (used in list view)
export const refundRequestDtoSchema = z.object({
  id: z.string().uuid(),
  ticketId: z.string().uuid(),
  ticketNumber: z.number(),
  emailAddress: z.string().email(),
  connectionDate: z.string(), // DateOnly represented as string
  reason: z.string(),
  createdAt: z.string() // DateTime represented as string
});

export type RefundRequestDto = z.infer<typeof refundRequestDtoSchema>;

// Schema for PersonDetailsDto
export const personDetailsDtoSchema = z.object({
  firstName: z.string(),
  lastName: z.string(),
  dateOfBirth: z.string(), // DateOnly represented as string
  discount: z.string().nullable()
});

export type PersonDetailsDto = z.infer<typeof personDetailsDtoSchema>;

// Schema for TicketConnectionStationDto
export const ticketConnectionStationDtoSchema = z.object({
  name: z.string(),
  arrivalTime: z.string().nullable(), // TimeOnly represented as string
  departureTime: z.string().nullable(), // TimeOnly represented as string
  sequenceNumber: z.number()
});

export type TicketConnectionStationDto = z.infer<typeof ticketConnectionStationDtoSchema>;

// Schema for RefundRequestDetailsDto (used in detail view)
export const refundRequestDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  ticketNumber: z.number(),
  people: z.array(personDetailsDtoSchema),
  seatNumbers: z.array(z.number()).nullable(),
  connectionDate: z.string(), // DateOnly represented as string
  stations: z.array(ticketConnectionStationDtoSchema),
  operatorCode: z.string(),
  operatorName: z.string(),
  trainName: z.string(),
  qrCodeId: z.string().uuid().nullable(),
  status: z.string(),
  reason: z.string(),
  createdAt: z.string() // DateTime represented as string
});

export type RefundRequestDetailsDto = z.infer<typeof refundRequestDetailsDtoSchema>;