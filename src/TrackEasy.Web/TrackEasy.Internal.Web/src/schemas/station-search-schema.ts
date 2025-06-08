import {z} from "zod";

export const stationSearchSchema = z.object({
  stationName: z.string()
    .regex(/^[A-Za-z0-9 ]*$/, {message: "Station name can only contain letters, numbers, and spaces"})
    .optional(),
  cityName: z.string()
    .regex(/^[A-Za-z ]*$/, {message: "City name can only contain letters"})
    .optional(),
});

export type StationSearchData = z.infer<typeof stationSearchSchema>;