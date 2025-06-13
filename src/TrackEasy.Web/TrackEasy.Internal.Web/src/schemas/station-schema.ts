import {z} from "zod";

// Schema for Geographical Coordinates DTO
export const geographicalCoordinatesDtoSchema = z.object({
  latitude: z.number()
    .min(-90, {message: 'Latitude must be between -90 and 90'})
    .max(90, {message: 'Latitude must be between -90 and 90'}),
  longitude: z.number()
    .min(-180, {message: 'Longitude must be between -180 and 180'})
    .max(180, {message: 'Longitude must be between -180 and 180'})
});

export type GeographicalCoordinatesDto = z.infer<typeof geographicalCoordinatesDtoSchema>;

// Schema for Station DTO (used in list view)
export const stationDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  city: z.string(), // City name as string
});

export type StationDto = z.infer<typeof stationDtoSchema>;

// Schema for Station Details DTO (used in detail view)
export const stationDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  cityId: z.string().uuid(),
  cityName: z.string(),
  geographicalCoordinates: geographicalCoordinatesDtoSchema
});

export type StationDetailsDto = z.infer<typeof stationDetailsDtoSchema>;

// Schema for Create Station Command
export const createStationCommandSchema = z.object({
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of station name is 3'})
    .max(50, {message: 'Maximum length of station name is 50'}),
  cityId: z.string().uuid({message: 'Valid city ID is required'}),
  geographicalCoordinates: geographicalCoordinatesDtoSchema
});

export type CreateStationCommand = z.infer<typeof createStationCommandSchema>;

// Schema for Update Station Command
export const updateStationCommandSchema = z.object({
  id: z.string().uuid(),
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of station name is 3'})
    .max(50, {message: 'Maximum length of station name is 50'}),
  cityId: z.string().uuid({message: 'Valid city ID is required'}),
  geographicalCoordinates: geographicalCoordinatesDtoSchema
});

export type UpdateStationCommand = z.infer<typeof updateStationCommandSchema>;
