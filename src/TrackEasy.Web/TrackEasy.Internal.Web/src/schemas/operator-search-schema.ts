import {z} from "zod";

export const operatorSearchSchema = z.object({
  name: z.string()
    .regex(/^[A-Za-z0-9 ]*$/, {message: "Name can only contain letters, numbers, and spaces"})
    .optional(),
  code: z.string()
    .regex(/^[A-Za-z0-9]*$/, {message: "Code can only contain letters and numbers"})
    .optional(),
});

export type OperatorSearchData = z.infer<typeof operatorSearchSchema>;