import {z} from "zod";

export const discountSchema = z.object({
  id: z.string().uuid().optional(),
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of discount is 3'})
    .max(50, {message: 'Maximum length of discount is 50'})
    .regex(/^[a-zA-Z ]*$/, {message: 'Name can only contains letters'}),
  percentage: z.number()
    .int({message: 'Percentage must be a non-floating point number'})
    .min(1)
    .max(99, {message: 'Percentage must be between 1 and 99'}),
})

export type Discount = z.infer<typeof discountSchema>;