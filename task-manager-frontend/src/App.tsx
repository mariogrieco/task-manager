import { useEffect } from 'react';
import { setupAuthInterceptor } from './api/authInterceptor';
import TaskList from './components/TaskList';

function App() {
  useEffect(() => {
    setupAuthInterceptor();
  }, []);

  return (
    <div className="App">
      <h1>Task Manager</h1>
      <TaskList />
    </div>
  );
}

export default App;
