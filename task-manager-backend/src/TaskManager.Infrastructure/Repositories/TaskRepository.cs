using Microsoft.Data.Sqlite;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly SqliteConnectionFactory _connectionFactory;

        public TaskRepository(SqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddAsync(TaskItem task, Guid userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Tasks 
                (Id, Title, Description, Status, CreatedAt, UpdatedAt, UserId)
                VALUES (@Id, @Title, @Description, @Status, @CreatedAt, @UpdatedAt, @UserId)";
            
            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description ?? "");
            command.Parameters.AddWithValue("@Status", (int)task.StatusItem);
            command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", task.UpdatedAt);
            command.Parameters.AddWithValue("@UserId", userId);
            
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Tasks WHERE Id = @Id AND UserId = @UserId";
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(Guid userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks WHERE UserId = @UserId";
            command.Parameters.AddWithValue("@UserId", userId);
            
            var tasks = new List<TaskItem>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tasks.Add(new TaskItem
                {
                    Id = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    StatusItem = (Core.Entities.TaskStatusItem)reader.GetInt32(3),
                    CreatedAt = reader.GetDateTime(4),
                    UpdatedAt = reader.GetDateTime(5),
                    UserId = reader.GetGuid(6) // Assuming the last column is UserId
                });
            }
            return tasks;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, Guid userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks WHERE Id = @Id AND UserId = @UserId";
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);
            
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TaskItem
                {
                    Id = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    StatusItem = (Core.Entities.TaskStatusItem)reader.GetInt32(3),
                    CreatedAt = reader.GetDateTime(4),
                    UpdatedAt = reader.GetDateTime(5),
                    UserId = reader.GetGuid(6) // Assuming the last column is UserId
                };
            }
            return null;
        }

        public async Task UpdateAsync(TaskItem task, Guid userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            
            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Tasks 
                SET Title = @Title, 
                    Description = @Description, 
                    Status = @Status, 
                    UpdatedAt = @UpdatedAt 
                WHERE Id = @Id AND UserId = @UserId";
            
            command.Parameters.AddWithValue("@Id", task.Id);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description ?? "");
            command.Parameters.AddWithValue("@Status", (int)task.StatusItem);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
            command.Parameters.AddWithValue("@UserId", userId);
            
            await command.ExecuteNonQueryAsync();
        }
    }
}
