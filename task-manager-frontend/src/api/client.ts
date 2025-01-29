import { createHttpClient } from 'task-manager-consumer';

const baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export const httpClient = createHttpClient({
  baseURL,
  headers: {
    'Accept': 'application/json'
  }
});
