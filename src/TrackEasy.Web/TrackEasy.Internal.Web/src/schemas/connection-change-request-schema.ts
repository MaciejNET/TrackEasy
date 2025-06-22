import {z} from "zod";


export enum ConnectionRequestType {
  ADD = "ADD",
  UPDATE = "UPDATE",
  DELETE = "DELETE"
}


export const connectionChangeRequestDtoSchema = z.object({
  connectionId: z.string().uuid(),
  name: z.string(),
  operatorName: z.string(),
  startStation: z.string(),
  endStation: z.string(),
  requestType: z.nativeEnum(ConnectionRequestType)
});

export type ConnectionChangeRequestDto = z.infer<typeof connectionChangeRequestDtoSchema>;


export const connectionStationDtoSchema = z.object({
  stationId: z.string().uuid(),
  arrivalTime: z.string().nullable(),
  departureTime: z.string().nullable(),
  sequenceNumber: z.number()
});

export type ConnectionStationDto = z.infer<typeof connectionStationDtoSchema>;


export const scheduleDtoSchema = z.object({
  validFrom: z.string(),
  validTo: z.string(),
  daysOfWeek: z.array(z.string())
});

export type ScheduleDto = z.infer<typeof scheduleDtoSchema>;


export const connectionChangeRequestDetailsDtoSchema = z.object({
  connectionId: z.string().uuid(),
  name: z.string(),
  operatorName: z.string(),
  requestType: z.nativeEnum(ConnectionRequestType),
  schedule: scheduleDtoSchema.nullable(),
  stations: z.array(connectionStationDtoSchema).nullable()
});

export type ConnectionChangeRequestDetailsDto = z.infer<typeof connectionChangeRequestDetailsDtoSchema>;