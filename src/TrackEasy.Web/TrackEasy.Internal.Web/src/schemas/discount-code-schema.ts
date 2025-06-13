import {z} from "zod";

// Schema for DiscountCodeDto
export const discountCodeDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
  percentage: z.number(),
  from: z.coerce.date(),
  to: z.coerce.date()
});

export type DiscountCodeDto = z.infer<typeof discountCodeDtoSchema>;

// Schema for CreateDiscountCodeCommand
export const createDiscountCodeCommandSchema = z.object({
  code: z.string()
    .nonempty({message: 'Code cannot be empty'})
    .min(5, {message: 'Minimum length of code is 5'})
    .max(15, {message: 'Maximum length of code is 15'})
    .regex(/^[a-zA-Z0-9-_]{5,15}$/, {message: 'Code can only contain letters, numbers, hyphens, and underscores'}),
  percentage: z.number()
    .int({message: 'Percentage must be a non-floating point number'})
    .min(1, {message: 'Percentage must be greater than 0'})
    .max(99, {message: 'Percentage must be less than 100'}),
  from: z.coerce.date()
    .refine(date => date >= new Date(), {
      message: 'From date must be in the future'
    }),
  to: z.coerce.date()
});

export type CreateDiscountCodeCommand = z.infer<typeof createDiscountCodeCommandSchema>;

// Schema for UpdateDiscountCodeCommand
export const updateDiscountCodeCommandSchema = z.object({
  id: z.string().uuid(),
  percentage: z.number()
    .int({message: 'Percentage must be a non-floating point number'})
    .min(1, {message: 'Percentage must be greater than 0'})
    .max(99, {message: 'Percentage must be less than 100'}),
  from: z.coerce.date()
    .refine(date => date >= new Date(), {
      message: 'From date must be in the future'
    }),
  to: z.coerce.date()
});

export type UpdateDiscountCodeCommand = z.infer<typeof updateDiscountCodeCommandSchema>;

// Add validation to ensure 'to' is after 'from'
export const discountCodeFormSchema = createDiscountCodeCommandSchema.refine(
  data => data.to > data.from,
  {
    message: "End date must be after start date",
    path: ["to"]
  }
);

export const updateDiscountCodeFormSchema = updateDiscountCodeCommandSchema.refine(
  data => data.to > data.from,
  {
    message: "End date must be after start date",
    path: ["to"]
  }
);