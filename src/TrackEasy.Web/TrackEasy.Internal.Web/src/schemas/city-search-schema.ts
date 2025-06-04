import {z} from "zod";

export const citySearchSchema = z.object({
  name: z.string()
    .regex(/^[A-Za-z ]*$/, {message: "Name can only contain letters"})
    .optional(),
});

export type CitySearchData = z.infer<typeof citySearchSchema>;