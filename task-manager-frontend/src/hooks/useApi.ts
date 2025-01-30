
import { createHttpClient, createTaskService } from 'task-manager-consumer';

const http = createHttpClient({
  baseURL: 'http://127.0.0.1:5001',  
});

const taskService = createTaskService(http);

export const useApi = () => {
  return {
    tasks: {
      getAllTasks: taskService.getAllTasks,
      // create: taskService.createTask,
      // update: taskService.updateTask,
      // delete: taskService.deleteTask
    }
  };
};
