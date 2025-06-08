import {z} from "zod";

// Schema for Coach DTO (used in list view)
export const coachDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
});

export type CoachDto = z.infer<typeof coachDtoSchema>;

// Schema for Coach Details DTO (used in detailed view)
export const coachDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
  seatsNumbers: z.array(z.number()),
});

export type CoachDetailsDto = z.infer<typeof coachDetailsDtoSchema>;

// Schema for Create Coach Command
export const createCoachCommandSchema = z.object({
  operatorId: z.string().uuid(),
  code: z.string().nonempty({message: 'Code cannot be empty'}),
  seatsNumbers: z.array(z.number()),
});

export type CreateCoachCommand = z.infer<typeof createCoachCommandSchema>;

// Schema for Update Coach Command
export const updateCoachCommandSchema = z.object({
  id: z.string().uuid(),
  operatorId: z.string().uuid(),
  name: z.string().nonempty({message: 'Name cannot be empty'}),
  seatsNumbers: z.array(z.number()),
});

export type UpdateCoachCommand = z.infer<typeof updateCoachCommandSchema>;