import { httpClient } from './client';
import { AuthService } from 'task-manager-consumer';

export const setupAuthInterceptor = () => {
  const authService = new AuthService(httpClient);
  
  // Add request interceptor
  httpClient.interceptors.request.use(config => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  // Add response interceptor
  httpClient.interceptors.response.use(
    response => response,
    async error => {
      if (error.response?.status === 401) {
        // Handle token refresh logic here
      }
      return Promise.reject(error);
    }
  );

  return authService;
};
