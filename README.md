# DotNet API

A RESTful Web API built with ASP.NET Core, featuring user management with dual data access approaches (Dapper and Entity Framework) and comprehensive API documentation.

## Overview

This project demonstrates building a modern .NET Web API with multiple data access patterns. It includes user management functionality with both Dapper and Entity Framework implementations, allowing developers to compare and choose the appropriate ORM for their needs.

## Technologies Used

- **Framework**: .NET 10.0
- **Runtime**: ASP.NET Core Web API
- **Data Access**:
  - Entity Framework Core 10.0.1
  - Dapper 2.1.66
- **Database**: SQL Server
- **Documentation**: OpenAPI/Swagger with Scalar UI
- **Mapping**: AutoMapper 16.0.0
- **Serialization**: System.Text.Json (implicit)

## Features

- Full CRUD operations for user management
- Dual implementation (Dapper and EF) for data access
- Sample weather forecast endpoint
- CORS configuration for development and production
- Comprehensive API documentation
- Database seeding with sample data
- Response type annotations for better API contracts

## Prerequisites

- .NET 10.0 SDK
- SQL Server (LocalDB, Express, or full SQL Server)
- Visual Studio 2022 or VS Code with C# extension

## Database Setup

1. Create a SQL Server database named `dotnetDB`
2. Update the connection string in `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=dotnetDB;User Id=sa;Password=123456;TrustServerCertificate=True;"
  }
}
```
3. Run the database schema script located in `Database/inet.sql` to create tables and populate sample data

## Installation & Running

1. Clone the repository:
```bash
git clone https://github.com/AAlkamel/dotnetApi.git
cd dotnetApi
```

2. Restore packages:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).

## API Endpoints

### User Management (Dapper)
- `GET /User/test` - Database connection test
- `GET /User/users` - Get all users
- `GET /User/user/{id}` - Get user by ID
- `POST /User/user` - Create new user
- `PUT /User/user/{id}` - Update user
- `DELETE /User/user/{id}` - Delete user

### User Management (Entity Framework)
- `GET /UserEF/test` - Database connection test
- `GET /UserEF/users` - Get all users
- `GET /UserEF/user/{id}` - Get user by ID
- `POST /UserEF/user` - Create new user
- `PUT /UserEF/user/{id}` - Update user
- `DELETE /UserEF/user/{id}` - Delete user

### Weather Forecast
- `GET /WeatherForecast` - Sample weather forecast data

## API Documentation

The API includes comprehensive OpenAPI documentation accessible through Scalar UI:

- **Development**: `http://localhost:5000/scalar/v1`
- **Production**: Configure accordingly

## Data Models

### User
```csharp
public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }
}
```

### UserAddDto
```csharp
public class UserAddDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }
}
```

## Database Schema

The application uses three main tables in the `AppSchema` schema:

1. **Users** - Core user information
2. **UserJobInfo** - Employment details
3. **UserSalary** - Compensation data

## CORS Configuration

The API is configured with CORS policies:

- **Development**: Allows localhost origins (4200, 3000, 8000)
- **Production**: Allows specific production domain

## Testing the API

You can test the API using:

1. **Browser**: Navigate to the Scalar documentation UI
2. **Postman/Insomnia**: Import the OpenAPI specification
3. **HTTP files**: Use the included `.http` files for testing

Example request to get all users:
```http
GET http://localhost:5000/User/users
```

## Project Structure

```
dotnetApi/
├── Controllers/
│   ├── UserController.cs          # Dapper-based user operations
│   ├── UserEFController.cs        # EF-based user operations
│   └── WeatherForecastController.cs
├── Data/
│   ├── DataContextDapper.cs       # Dapper data context
│   └── DataContextEF.cs           # Entity Framework context
├── DTOs/
│   └── UserAddDto.cs              # Data transfer object
├── Models/
│   ├── User.cs
│   ├── UserJobInfo.cs
│   └── UserSalary.cs
├── Database/
│   └── inet.sql                   # Database schema and sample data
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── Program.cs
└── dotnetApi.csproj
```

## Development

### Adding New Features

1. Create models in the `Models` folder
2. Add DTOs in the `DTOs` folder if needed
3. Implement controllers in the `Controllers` folder
4. Update database schema in `Database/inet.sql`
5. Update this README with new endpoints

### Data Access Patterns

The project demonstrates two approaches to data access:

- **Dapper**: Lightweight, high-performance ORM
- **Entity Framework**: Full-featured ORM with change tracking

Choose based on your performance and feature requirements.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- ASP.NET Core documentation
- Dapper documentation
- Entity Framework Core documentation
- OpenAPI/Swagger specifications
