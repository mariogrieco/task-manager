import { useContext } from 'react';
import { httpClient } from '../api/client';
import { createTaskService } from 'task-manager-consumer';

const taskService = createTaskService(httpClient);

export const useApi = () => {
  return {
    tasks: {
      getAll: taskService.getAllTasks,
      create: taskService.createTask,
      update: taskService.updateTask,
      delete: taskService.deleteTask
    }
  };
};
