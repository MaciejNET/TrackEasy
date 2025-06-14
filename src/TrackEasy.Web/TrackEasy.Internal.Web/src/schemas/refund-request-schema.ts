import {z} from "zod";


export const refundRequestDtoSchema = z.object({
  id: z.string().uuid(),
  ticketId: z.string().uuid(),
  ticketNumber: z.number(),
  emailAddress: z.string().email(),
  connectionDate: z.string(), 
  reason: z.string(),
  createdAt: z.string() 
});

export type RefundRequestDto = z.infer<typeof refundRequestDtoSchema>;


export const personDetailsDtoSchema = z.object({
  firstName: z.string(),
  lastName: z.string(),
  dateOfBirth: z.string(), 
  discount: z.string().nullable()
});

export type PersonDetailsDto = z.infer<typeof personDetailsDtoSchema>;


export const ticketConnectionStationDtoSchema = z.object({
  name: z.string(),
  arrivalTime: z.string().nullable(), 
  departureTime: z.string().nullable(), 
  sequenceNumber: z.number()
});

export type TicketConnectionStationDto = z.infer<typeof ticketConnectionStationDtoSchema>;


export const refundRequestDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  ticketNumber: z.number(),
  people: z.array(personDetailsDtoSchema),
  seatNumbers: z.array(z.number()).nullable(),
  connectionDate: z.string(), 
  stations: z.array(ticketConnectionStationDtoSchema),
  operatorCode: z.string(),
  operatorName: z.string(),
  trainName: z.string(),
  qrCodeId: z.string().uuid().nullable(),
  status: z.string(),
  reason: z.string(),
  createdAt: z.string() 
});

export type RefundRequestDetailsDto = z.infer<typeof refundRequestDetailsDtoSchema>;