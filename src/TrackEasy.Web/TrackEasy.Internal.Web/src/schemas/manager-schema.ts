import {z} from "zod";

// Schema for Create Manager Command
export const createManagerCommandSchema = z.object({
  operatorId: z.string().uuid({message: 'Valid operator ID is required'}),
  firstName: z.string()
    .nonempty({message: 'First name cannot be empty'})
    .min(2, {message: 'Minimum length of first name is 2'})
    .max(50, {message: 'Maximum length of first name is 50'}),
  lastName: z.string()
    .nonempty({message: 'Last name cannot be empty'})
    .min(2, {message: 'Minimum length of last name is 2'})
    .max(50, {message: 'Maximum length of last name is 50'}),
  email: z.string()
    .nonempty({message: 'Email cannot be empty'})
    .email({message: 'Invalid email format'}),
  dateOfBirth: z.string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, {message: 'Date must be in YYYY-MM-DD format'}),
  password: z.string()
    .nonempty({message: 'Password cannot be empty'})
    .min(8, {message: 'Password must be at least 8 characters'})
});

export type CreateManagerCommand = z.infer<typeof createManagerCommandSchema>;