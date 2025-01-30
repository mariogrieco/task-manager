# Register user

curl -X POST http://localhost:5001/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"SecurePassword123!"}'

## Login

curl -X POST http://localhost:5001/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"SecurePassword123!"}'

## Use token in subsequent requests

curl -X DELETE http://localhost:5001/api/tasks/{task-id} \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
