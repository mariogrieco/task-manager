import { createHttpClient } from 'task-manager-consumer';
import { createTaskService } from 'task-manager-consumer/services';

const http = createHttpClient({ baseURL: 'http://api.example.com' });
const taskService = createTaskService(http);

// In component
const fetchTasks = async () => {
  const result = await taskService.getAllTasks();
  if (result.success) {
    setTasks(result.data);
  } else {
    handleError(result.error);
  }
};
