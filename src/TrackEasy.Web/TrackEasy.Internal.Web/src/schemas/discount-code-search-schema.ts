import {z} from "zod";

export const discountCodeSearchSchema = z.object({
  code: z.string()
    .regex(/^[a-zA-Z0-9-_]*$/, {message: "Code can only contain letters, numbers, hyphens, and underscores"})
    .optional(),
  percentage: z.preprocess((val) => {
      if (typeof val === "string" && val.trim() === "") {
        return undefined;
      }
      return Number(val);
    },
    z.number()
      .int({message: "Percentage must be a non-floating point number"})
      .optional()
  ),
});

export type DiscountCodeSearchData = z.infer<typeof discountCodeSearchSchema>;