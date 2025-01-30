using Microsoft.Data.Sqlite;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public UserRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users 
            (Id, Username, PasswordHash, PasswordSalt, CreatedAt)
            VALUES (@Id, @Username, @PasswordHash, @PasswordSalt, @CreatedAt)";
        
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                PasswordHash = (byte[])reader["PasswordHash"],
                PasswordSalt = (byte[])reader["PasswordSalt"],
                CreatedAt = reader.GetDateTime(4)
            };
        }
        return null;
    }

    // Implement other interface methods as needed
    public Task DeleteAsync(Guid id) => throw new NotImplementedException();
    public Task<User?> GetByIdAsync(Guid id) => throw new NotImplementedException();
    public Task UpdateAsync(User user) => throw new NotImplementedException();
}