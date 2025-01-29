-- Tasks Table
CREATE TABLE IF NOT EXISTS Tasks (
    Id TEXT PRIMARY KEY,
    Title TEXT NOT NULL,
    Description TEXT,
    Status INTEGER NOT NULL CHECK(Status IN (0, 1, 2)),
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL
);

-- Users Table
CREATE TABLE IF NOT EXISTS Users (
    Id TEXT PRIMARY KEY,
    Username TEXT UNIQUE NOT NULL,
    Email TEXT UNIQUE NOT NULL,
    PasswordHash BLOB NOT NULL,
    PasswordSalt BLOB NOT NULL,
    CreatedAt TEXT NOT NULL
);
