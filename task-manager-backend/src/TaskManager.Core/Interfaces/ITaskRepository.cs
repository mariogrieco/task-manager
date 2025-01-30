using System.Threading.Tasks;
using TaskManager.Core.Entities;
using System.Collections.Generic;

namespace TaskManager.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId);
        Task AddAsync(TaskItem task, Guid userId);
        Task UpdateAsync(TaskItem task, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
