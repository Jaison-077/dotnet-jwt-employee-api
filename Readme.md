# FirstApi

A minimal .NET 10 Web API demonstrating JWT authentication for an employee management scenario.

## Features

- JWT-based authentication and authorization
- Endpoints for authentication (register / login) and employee operations
- Swagger/OpenAPI for interactive API exploration (if enabled in Startup)

## Prerequisites

- .NET 10 SDK
- (Optional) A SQL database if the project uses EF Core. Configure a connection string in appsettings.json

## Getting started

1. Clone the repository:

   git clone https://github.com/Jaison-077/dotnet-jwt-employee-api.git
   cd FirstApi

2. Restore packages and build:

   dotnet restore
   dotnet build

3. Configure appsettings.json (or user secrets / environment variables)

   Example settings to configure JWT and a database connection:

   ```json
   {
	 "Jwt": {
	   "Key": "<your-very-strong-secret>",
	   "Issuer": "FirstApi",
	   "Audience": "FirstApiUsers",
	   "ExpiryMinutes": 60
	 },
	 "ConnectionStrings": {
	   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FirstApiDb;Trusted_Connection=True;"
	 }
   }
   ```

   - Replace `<your-very-strong-secret>` with a secure random value. Prefer storing secrets in environment variables or a secrets manager for production.

4. Run the application:

   dotnet run --project FirstApi

   Or open the solution in Visual Studio and run the project.

5. Open the API in a browser (if running locally):

   - Swagger UI (if configured): https://localhost:5001/swagger

## Common endpoints

- POST /api/auth/register  — register a new user (body depends on implementation)
- POST /api/auth/login     — login and receive a JWT token
- GET /api/employees       — get list of employees (protected)
- GET /api/employees/{id}  — get employee by id (protected)

Use the Authorization header with the JWT for protected endpoints:

Authorization: Bearer <token>

## Notes

- Implementation details (models, exact route names, and request shapes) can be found in the Controllers and Services folders.
- For production, rotate and protect the JWT signing key and use HTTPS.

## Contributing

Contributions and issues are welcome. Please open an issue or a pull request on the repository.

## License

See the repository license file if present.
