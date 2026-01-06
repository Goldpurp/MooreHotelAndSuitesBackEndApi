
MooreHotelAndSuites - Full .NET 9 Clean Architecture scaffold (HTTP only, minimal implementation)

Stack
- .NET 9
- EF Core (PostgreSQL)
- ASP.NET Identity (Identity + JWT)
- Swagger (OpenAPI)
- AutoMapper

What I generated
- Projects:
  - src/MooreHotelAndSuites.API (Web API, Program.cs, controllers, appsettings, appsettings.Development.json, launchSettings for HTTP only)
  - src/MooreHotelAndSuites.Application (services)
  - src/MooreHotelAndSuites.Domain (entities + interfaces)
  - src/MooreHotelAndSuites.Infrastructure (EF Core, Identity, repositories, seeder)
- Seeded admin configured in appsettings: MooreHotel_Admin / Moore123!

Pre-installation (one-time)
1. Install .NET 9 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
2. Install dotnet-ef tool (global):
   dotnet tool install --global dotnet-ef

Run locally
1. From repository root:
   dotnet restore

2. Create EF migrations (Infrastructure project is where DbContext lives):
   dotnet ef migrations add InitialCreate -p src/MooreHotelAndSuites.Infrastructure -s src/MooreHotelAndSuites.API

3. Apply migrations to your PostgreSQL (ensure connection string is correct in src/MooreHotelAndSuites.API/appsettings.json):
   dotnet ef database update -p src/MooreHotelAndSuites.Infrastructure -s src/MooreHotelAndSuites.API

4. Run the API (HTTP only):
   dotnet run --project src/MooreHotelAndSuites.API

5. Open Swagger UI (development mode):
   http://localhost:5000/swagger

Notes & Security
- Replace Jwt:Key in appsettings.json with a secure secret (>=32 chars) before production.
- Use HTTPS in production and configure CORS appropriately.
- Identity will hash passwords securely when seeding via Identity APIs.

If you want, I can:
- Generate EF migration files and include them in the repo.
- Add unit test project skeletons.
- Add a simple React dashboard scaffold.

