import {z} from "zod";

export const trainSearchSchema = z.object({
  trainName: z.string().optional(),
});

export type TrainSearchData = z.infer<typeof trainSearchSchema>;