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
            (Id, Username, Email, PasswordHash, PasswordSalt, CreatedAt)
            VALUES (@Id, @Username, @Email, @PasswordHash, @PasswordSalt, @CreatedAt)"; // 6 columns

        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Email", user.Email); // Critical line
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt.ToString("o"));
        
        await command.ExecuteNonQueryAsync();
    }


    public async Task<User?> GetByEmailAsync(string email) {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT 
                Id, 
                Username, 
                PasswordHash, 
                PasswordSalt, 
                CreatedAt,
                Email
            FROM Users 
            WHERE Email = @Email";
        
        command.Parameters.AddWithValue("@Email", email);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = (byte[])reader["PasswordHash"],
                PasswordSalt = (byte[])reader["PasswordSalt"],
                CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")))
            };
        }
        return null;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT 
                Id, 
                Username, 
                PasswordHash, 
                PasswordSalt, 
                CreatedAt,
                Email
            FROM Users 
            WHERE Username = @Username";
        
        command.Parameters.AddWithValue("@Username", username);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = (byte[])reader["PasswordHash"],
                PasswordSalt = (byte[])reader["PasswordSalt"],
                CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")))
            };
        }
        return null;
    }

    public Task DeleteAsync(Guid id) => throw new NotImplementedException();
    public Task<User?> GetByIdAsync(Guid id) => throw new NotImplementedException();
    public Task UpdateAsync(User user) => throw new NotImplementedException();
}