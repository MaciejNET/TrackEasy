import {z} from "zod";

// Schema for Create Admin Command
export const createAdminCommandSchema = z.object({
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

export type CreateAdminCommand = z.infer<typeof createAdminCommandSchema>;

// Schema for Update Admin Command
export const updateAdminSchema = z.object({
  id: z.string().uuid(),
  firstName: z.string()
    .nonempty({ message: "First name is required" })
    .min(2, { message: "First name must be at least 2 characters" })
    .max(50, { message: "First name must be less than 50 characters" }),
  lastName: z.string()
    .nonempty({ message: "Last name is required" })
    .min(2, { message: "Last name must be at least 2 characters" })
    .max(50, { message: "Last name must be less than 50 characters" }),
  birthDate: z.string()
    .nonempty({ message: "Birth date is required" })
    .regex(/^\d{4}-\d{2}-\d{2}$/, { message: "Date must be in YYYY-MM-DD format" })
});

export type UpdateAdminCommand = z.infer<typeof updateAdminSchema>;