import {z} from "zod";

export const discountSearchSchema = z.object({
  name: z.string()
    .regex(/^[A-Za-z ]*$/, {message: "Name can only contain letters"})
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

export type DiscountSearchData = z.infer<typeof discountSearchSchema>;
