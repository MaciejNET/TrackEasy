import {z} from "zod";

// Schema for Train DTO (used in list view)
export const trainDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
});

export type TrainDto = z.infer<typeof trainDtoSchema>;

// Schema for Coach DTO (used in train details)
export const coachDtoSchema = z.object({
  id: z.string().uuid(),
  code: z.string(),
});

export type CoachDto = z.infer<typeof coachDtoSchema>;

// Schema for Train Details DTO (used in detailed view)
export const trainDetailsDtoSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  coaches: z.array(
    z.tuple([
      coachDtoSchema,
      z.number()
    ])
  ),
});

export type TrainDetailsDto = z.infer<typeof trainDetailsDtoSchema>;

// Schema for Coach Selection in Train form
export const coachSelectionSchema = z.object({
  coachId: z.string().uuid(),
  number: z.number().int().positive(),
});

export type CoachSelection = z.infer<typeof coachSelectionSchema>;

// Schema for Add Train Command
export const addTrainCommandSchema = z.object({
  operatorId: z.string().uuid(),
  name: z.string().nonempty({message: 'Name cannot be empty'}),
  coaches: z.array(
    z.tuple([
      z.string().uuid(),
      z.number().int().positive()
    ])
  ),
});

export type AddTrainCommand = z.infer<typeof addTrainCommandSchema>;

// Schema for Update Train Command
export const updateTrainCommandSchema = z.object({
  operatorId: z.string().uuid(),
  trainId: z.string().uuid(),
  name: z.string().nonempty({message: 'Name cannot be empty'}),
  coaches: z.array(
    z.tuple([
      z.string().uuid(),
      z.number().int().positive()
    ])
  ),
});

export type UpdateTrainCommand = z.infer<typeof updateTrainCommandSchema>;