import {z} from "zod";

// Enum for Currency
export enum Currency {
  PLN = 0,
  EUR = 1,
  USD = 2
}

// Enum for DayOfWeek
export enum DayOfWeek {
  Sunday = 0,
  Monday = 1,
  Tuesday = 2,
  Wednesday = 3,
  Thursday = 4,
  Friday = 5,
  Saturday = 6
}

// Schema for MoneyDto
export const moneyDtoSchema = z.object({
  amount: z.number(),
  currency: z.nativeEnum(Currency)
});

export type MoneyDto = z.infer<typeof moneyDtoSchema>;

// Schema for ConnectionDto (used in list view)
export const connectionDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  startStation: z.string(),
  endStation: z.string(),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek)),
  isActive: z.boolean()
});

export type ConnectionDto = z.infer<typeof connectionDtoSchema>;

// Schema for ConnectionStationDto
export const connectionStationDtoSchema = z.object({
  stationId: z.string().uuid(),
  arrivalTime: z.string().nullable(),
  departureTime: z.string().nullable(),
  sequenceNumber: z.number()
});

export type ConnectionStationDto = z.infer<typeof connectionStationDtoSchema>;

// Schema for ConnectionStationDetailsDto
export const connectionStationDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  stationId: z.string().uuid(),
  stationName: z.string(),
  arrivalTime: z.string().nullable(),
  departureTime: z.string().nullable(),
  sequenceNumber: z.number()
});

export type ConnectionStationDetailsDto = z.infer<typeof connectionStationDetailsDtoSchema>;

// Schema for ScheduleDto
export const scheduleDtoSchema = z.object({
  validFrom: z.string(),
  validTo: z.string(),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek))
});

export type ScheduleDto = z.infer<typeof scheduleDtoSchema>;

// Schema for ConnectionDetailsDto (used in detail view)
export const connectionDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  pricePerKilometer: moneyDtoSchema,
  trainId: z.string().uuid(),
  trainName: z.string(),
  validFrom: z.string(),
  validTo: z.string(),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek)),
  stations: z.array(connectionStationDetailsDtoSchema),
  hasPendingRequest: z.boolean(),
  needsSeatReservation: z.boolean(),
  isActive: z.boolean()
});

export type ConnectionDetailsDto = z.infer<typeof connectionDetailsDtoSchema>;

// Schema for CreateConnectionCommand
export const createConnectionCommandSchema = z.object({
  name: z.string().min(3, {message: 'Name must be at least 3 characters'}).max(100, {message: 'Name must be at most 100 characters'}),
  operatorId: z.string().uuid(),
  pricePerKilometer: moneyDtoSchema,
  trainId: z.string().uuid(),
  schedule: scheduleDtoSchema,
  connectionStations: z.array(connectionStationDtoSchema),
  needsSeatReservation: z.boolean()
});

export type CreateConnectionCommand = z.infer<typeof createConnectionCommandSchema>;

// Schema for UpdateConnectionCommand
export const updateConnectionCommandSchema = z.object({
  id: z.string().uuid(),
  name: z.string().min(3, {message: 'Name must be at least 3 characters'}).max(100, {message: 'Name must be at most 100 characters'}),
  money: moneyDtoSchema
});

export type UpdateConnectionCommand = z.infer<typeof updateConnectionCommandSchema>;

// Schema for UpdateScheduleCommand
export const updateScheduleCommandSchema = z.object({
  id: z.string().uuid(),
  schedule: scheduleDtoSchema,
  connectionStations: z.array(connectionStationDtoSchema)
});

export type UpdateScheduleCommand = z.infer<typeof updateScheduleCommandSchema>;
