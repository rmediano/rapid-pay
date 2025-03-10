# Setup

## SQL Server
Start the docker container:
```bash
docker compose -f docker-compose.sqlserver.yaml up -d
```
Run the migrations command
```bash
dotnet ef migrations add InitialCreate
```
Set `DATABASE_TYPE` to `sqlserver` in `launchsettings.json`

```json
{
  "DATABASE_TYPE": "sqlserver"
}
```
Run the application

## DynamoDB
Start the docker container:
```bash
docker compose -f docker-compose.dynamodb.yaml up -d
```
Set `DATABASE_TYPE` to `dynamodb` in `launchsettings.json`
```json
{
  "DATABASE_TYPE": "sqlserver"
}
```
Run the application

# Auth
### Login endpoint
Basic auth with the following credentials
```bash
username: "user",
password: "testpassword"
-H 'Authorization: Basic dXNlcjp0ZXN0cGFzc3dvcmQ='
```
### Create, GetBalance, Pay endpoints
Use the jwt token from the login response in the authorization header
```bash
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiI...'
```