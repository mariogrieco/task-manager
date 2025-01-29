import type { ApiResult, ApiError } from './types';

export type HttpClient = {
  get: <T>(url: string) => Promise<ApiResult<T>>;
  post: <T>(url: string, body: unknown) => Promise<ApiResult<T>>;
};

export const createHttpClient = (config: {
  baseURL: string;
  headers?: Record<string, string>;
}) => {
  const baseHeaders = {
    'Content-Type': 'application/json',
    ...config.headers,
  };

  const handleResponse = async <T>(response: Response): Promise<ApiResult<T>> => {
    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      return {
        success: false,
        error: {
          code: response.status,
          message: errorData.message || response.statusText,
          details: errorData,
        },
      };
    }
    
    const data = await response.json();
    return { success: true, data };
  };

  return {
    get: async <T>(url: string): Promise<ApiResult<T>> => {
      try {
        const response = await fetch(`${config.baseURL}${url}`, {
          method: 'GET',
          headers: baseHeaders,
        });
        return handleResponse<T>(response);
      } catch (error) {
        return {
          success: false,
          error: {
            code: 0,
            message: 'Network error',
            details: error,
          },
        };
      }
    },

    post: async <T>(url: string, body: unknown): Promise<ApiResult<T>> => {
      try {
        const response = await fetch(`${config.baseURL}${url}`, {
          method: 'POST',
          headers: baseHeaders,
          body: JSON.stringify(body),
        });
        return handleResponse<T>(response);
      } catch (error) {
        return {
          success: false,
          error: {
            code: 0,
            message: 'Network error',
            details: error,
          },
        };
      }
    },
  };
};
