import { useEffect, useState } from 'react';
import { useApi } from '../hooks/useApi';
import type { Task, ApiError } from 'task-manager-consumer';

const TaskList = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [error, setError] = useState<ApiError | null>(null);
  const { tasks: taskApi } = useApi();

  useEffect(() => {
    const loadTasks = async () => {
      const result = await taskApi.getAll();
      
      if (result.success) {
        setTasks(result.data);
      } else {
        setError(result.error);
      }
    };

    loadTasks();
  }, []);

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return (
    <div>
      <h2>Tasks</h2>
      <ul>
        {tasks.map(task => (
          <li key={task.id}>
            {task.title} - {task.status}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TaskList;
