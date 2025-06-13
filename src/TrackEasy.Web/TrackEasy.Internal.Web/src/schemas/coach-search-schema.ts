import {z} from "zod";

export const coachSearchSchema = z.object({
  code: z.string().optional(),
});

export type CoachSearchData = z.infer<typeof coachSearchSchema>;