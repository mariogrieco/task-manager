import { z } from 'zod';

export const TaskSchema = z.object({
  id: z.string().uuid(),
  title: z.string().min(1),
  description: z.string().optional(),
  status: z.enum(['pending', 'in-progress', 'completed']),
  createdAt: z.string().datetime().transform((val) => new Date(val)),
});

export type Task = z.infer<typeof TaskSchema>;
