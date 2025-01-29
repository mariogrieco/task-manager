import { describe, it, expect, vi, beforeEach } from 'vitest';
import { createHttpClient } from '../src/httpClient';

const mockFetch = vi.fn();
global.fetch = mockFetch;

const mockResponse = <T>(data: T): Response => ({
  ok: true,
  json: () => Promise.resolve(data),
  headers: new Headers(),
  status: 200,
  statusText: 'OK',
} as Response);

describe('HTTP Client', () => {
  const baseURL = 'http://api.example.com';
  
  beforeEach(() => {
    mockFetch.mockReset();
  });

  it('should make successful GET request', async () => {
    const testData = { id: '1', title: 'Test Task' };
    mockFetch.mockResolvedValue(mockResponse(testData));
    
    const client = createHttpClient({ baseURL });
    const result = await client.get('/tasks/1');

    expect(result).toEqual({
      success: true,
      data: testData
    });
  });
});
