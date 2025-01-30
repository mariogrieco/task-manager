export interface Task {
    id: string;
    title: string;
    description?: string;
    status: 'Pending' | 'In-progress' | 'Completed';
    createdAt: Date;
  }
  
  export interface User {
    id: string;
    username: string;
    email: string;
  }
  
  export type ApiError = {
    code: number;
    message: string;
    details?: unknown;
  };
  
  export type ApiResult<T> = 
    | { success: true; data: T }
    | { success: false; error: ApiError };