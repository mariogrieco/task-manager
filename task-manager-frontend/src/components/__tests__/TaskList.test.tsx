import { render, screen } from '@testing-library/react';
import TaskList from '../TaskList';
import { useApi } from '../../hooks/useApi';
import { vi, describe, it, expect } from 'vitest';

vi.mock('../../hooks/useApi');

describe('TaskList', () => {
  it('displays tasks', async () => {
    const mockTasks = [
      { id: '1', title: 'Test Task', status: 'pending', createdAt: new Date() }
    ];
    
    (useApi as vi.mock).mockReturnValue({
      tasks: {
        getAll: vi.fn().mockResolvedValue({
          success: true,
          data: mockTasks
        })
      }
    });

    render(<TaskList />);
    
    await screen.findByText('Test Task');
    expect(screen.getByText('pending')).toBeInTheDocument();
  });
});
