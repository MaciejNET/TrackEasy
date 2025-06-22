import {z} from "zod";


export enum Currency {
  PLN = 0,
  EUR = 1,
  USD = 2
}


export enum DayOfWeek {
  Sunday = 0,
  Monday = 1,
  Tuesday = 2,
  Wednesday = 3,
  Thursday = 4,
  Friday = 5,
  Saturday = 6
}


export const moneyDtoSchema = z.object({
  amount: z.number(),
  currency: z.nativeEnum(Currency)
});

export type MoneyDto = z.infer<typeof moneyDtoSchema>;


export const connectionDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  startStation: z.string(),
  endStation: z.string(),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek)),
  isActive: z.boolean()
});

export type ConnectionDto = z.infer<typeof connectionDtoSchema>;


export const connectionStationDtoSchema = z.object({
  stationId: z.string().uuid(),
  arrivalTime: z.string().nullable(),
  departureTime: z.string().nullable(),
  sequenceNumber: z.number()
});

export type ConnectionStationDto = z.infer<typeof connectionStationDtoSchema>;


export const connectionStationDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  stationId: z.string().uuid(),
  stationName: z.string(),
  arrivalTime: z.string().nullable(),
  departureTime: z.string().nullable(),
  sequenceNumber: z.number()
});

export type ConnectionStationDetailsDto = z.infer<typeof connectionStationDetailsDtoSchema>;


export const scheduleDtoSchema = z.object({
  validFrom: z.string(),
  validTo: z.string(),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek))
});

export type ScheduleDto = z.infer<typeof scheduleDtoSchema>;


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


export const updateConnectionCommandSchema = z.object({
  id: z.string().uuid(),
  name: z.string().min(3, {message: 'Name must be at least 3 characters'}).max(100, {message: 'Name must be at most 100 characters'}),
  money: moneyDtoSchema
});

export type UpdateConnectionCommand = z.infer<typeof updateConnectionCommandSchema>;


export const updateScheduleCommandSchema = z.object({
  connectionId: z.string().uuid(),
  schedule: scheduleDtoSchema,
  connectionStations: z.array(connectionStationDtoSchema)
});

export type UpdateScheduleCommand = z.infer<typeof updateScheduleCommandSchema>;
