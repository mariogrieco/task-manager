import React, { useEffect } from 'react'
import { useApi } from '../hooks/useApi'

const HomePage: React.FC = () => {
  const api = useApi()

  useEffect(() => {
    (async () => {
        console.log( api.tasks.getAllTasks );
        // const r = await api.tasks.getAllTasks()
        console.log(r);
    })()
  }, [api])

  return (
    <div>
      <h1>Task Manager</h1>
      <p>Welcome to the Task Manager application</p>
    </div>
  )
}

export default HomePage
