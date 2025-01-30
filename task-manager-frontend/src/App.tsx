import React, { useEffect, useState } from 'react'
import { useApi } from './hooks/useApi'
import { type Task } from 'task-manager-consumer/src/types'

const HomePage: React.FC = () => {
  const [ tasks, setTasks ] = useState<Task[]>([])

  const api = useApi()

  useEffect(() => {
    (async () => {
       try {
         const task = await api.tasks.getAllTasks()
         if (task.success) setTasks(task.data)
       } catch (e) {
        console.log(e)
       }
    })();
  }, [api.tasks])

  return (
    <div>
      <h1>Task Manager</h1>
      <p>Welcome to the Task Manager application</p>
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
    </div>
  )
}

export default HomePage
