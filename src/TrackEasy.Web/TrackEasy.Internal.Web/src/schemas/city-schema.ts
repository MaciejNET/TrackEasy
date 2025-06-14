import {z} from "zod";


export const countryDtoSchema = z.object({
  id: z.number(),
  name: z.string(),
});

export type CountryDto = z.infer<typeof countryDtoSchema>;


export const cityDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  country: z.string(), 
});

export type CityDto = z.infer<typeof cityDtoSchema>;


export const cityDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  country: countryDtoSchema,
  funFacts: z.array(z.string().max(255, { message: 'Fun fact cannot exceed 255 characters' })),
});

export type CityDetailsDto = z.infer<typeof cityDetailsDtoSchema>;


export const createCityCommandSchema = z.object({
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of city name is 3'})
    .max(50, {message: 'Maximum length of city name is 50'})
    .regex(/^[a-zA-Z ]*$/, {message: 'Name can only contains letters'}),
  country: z.number(), 
  funFacts: z.array(z.string().max(255, { message: 'Fun fact cannot exceed 255 characters' })),
});

export type CreateCityCommand = z.infer<typeof createCityCommandSchema>;


export const updateCityCommandSchema = z.object({
  id: z.string().uuid(),
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of city name is 3'})
    .max(50, {message: 'Maximum length of city name is 50'})
    .regex(/^[a-zA-Z ]*$/, {message: 'Name can only contains letters'}),
  country: z.number(), 
  funFacts: z.array(z.string().max(255, { message: 'Fun fact cannot exceed 255 characters' })),
});

export type UpdateCityCommand = z.infer<typeof updateCityCommandSchema>;


export const citySchema = createCityCommandSchema.extend({
  id: z.string().uuid().optional(),
});

export type City = z.infer<typeof citySchema>;
