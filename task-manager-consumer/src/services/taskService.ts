import z from 'zod';
import { HttpClient } from '../httpClient';
import { Task, TaskSchema } from '../schemas';
import { ApiResult } from '../types';

export const createTaskService = (http: HttpClient) => ({
  async getAllTasks(): Promise<ApiResult<Task[]>> {
    const result = await http.get('/api/tasks');
    if (!result.success) return result;

    try {
      const parsed = z.array(TaskSchema).parse(result.data);
      return { success: true, data: parsed };
    } catch (error) {
      return {
        success: false,
        error: {
          code: 500,
          message: 'Validation failed',
          details: error
        }
      };
    }
  }
});
