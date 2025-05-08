import {z} from "zod";

export const citySchema = z.object({
  id: z.string().uuid().optional(),
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of city name is 3'})
    .max(50, {message: 'Maximum length of city name is 50'})
    .regex(/^[a-zA-Z ]*$/, {message: 'Name can only contains letters'}),
  countryId: z.string().uuid({message: 'Country ID must be a valid UUID'}),
});

export type City = z.infer<typeof citySchema>;

export const countrySchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
});

export type Country = z.infer<typeof countrySchema>;