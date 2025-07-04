import {z} from "zod";


export const operatorDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  code: z.string(),
});

export type OperatorDto = z.infer<typeof operatorDtoSchema>;


export const createOperatorCommandSchema = z.object({
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of operator name is 3'})
    .max(50, {message: 'Maximum length of operator name is 50'}),
  code: z.string()
    .nonempty({message: 'Code cannot be empty'})
    .min(2, {message: 'Minimum length of code is 2'})
    .max(3, {message: 'Maximum length of code is 3'})
});

export type CreateOperatorCommand = z.infer<typeof createOperatorCommandSchema>;


export const updateOperatorCommandSchema = z.object({
  id: z.string().uuid(),
  name: z.string()
    .nonempty({message: 'Name cannot be empty'})
    .min(3, {message: 'Minimum length of operator name is 3'})
    .max(50, {message: 'Maximum length of operator name is 50'}),
  code: z.string()
    .nonempty({message: 'Code cannot be empty'})
    .min(2, {message: 'Minimum length of code is 2'})
    .max(3, {message: 'Maximum length of code is 3'})
});

export type UpdateOperatorCommand = z.infer<typeof updateOperatorCommandSchema>;