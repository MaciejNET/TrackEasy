import {z} from "zod";


export const coachDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
});

export type CoachDto = z.infer<typeof coachDtoSchema>;


export const coachDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
  seatsNumbers: z.array(z.number()),
});

export type CoachDetailsDto = z.infer<typeof coachDetailsDtoSchema>;


export const createCoachCommandSchema = z.object({
  operatorId: z.string().uuid(),
  code: z.string().nonempty({message: 'Code cannot be empty'}),
  seatsNumbers: z.array(z.number()),
});

export type CreateCoachCommand = z.infer<typeof createCoachCommandSchema>;


export const updateCoachCommandSchema = z.object({
  id: z.string().uuid(),
  operatorId: z.string().uuid(),
  name: z.string().nonempty({message: 'Name cannot be empty'}),
  seatsNumbers: z.array(z.number()),
});

export type UpdateCoachCommand = z.infer<typeof updateCoachCommandSchema>;