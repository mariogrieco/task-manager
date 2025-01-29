INSERT INTO Users (Id, Username, Email, PasswordHash, PasswordSalt, CreatedAt)
VALUES (
    'a1b2c3d4e5f64a5ba8b71234567890ab',
    'admin',
    'admin@taskmanager.com',
    RANDOMBLOB(32),
    RANDOMBLOB(16),
    datetime('now')
);

INSERT INTO Tasks (Id, Title, Description, Status, CreatedAt, UpdatedAt)
VALUES (
    'b2c3d4e5f64a5ba8b71234567890ab1d',
    'Initial Task',
    'Seed database task',
    0,
    datetime('now'),
    datetime('now')
);