# User CRUD Backend API

A comprehensive .NET 9.0 Web API solution for managing user data with advanced features including PDF generation using Playwright. This API provides a complete set of CRUD operations for user management, role management, statistical analysis, and document generation.

## 📋 Tech Stack

- **Backend**: .NET 9.0, ASP.NET Core Web API
- **Database**: Entity Framework Core (Code-First approach with data annotations), Microsoft SQL Server on Azure
- **Documentation**: Swagger/OpenAPI
- **PDF Generation**: Playwright for .NET
- **Testing**: Postman collection included

## 🏗️ Code Architecture

The application follows a clean, modular architecture that promotes separation of concerns and maintainability:

### Layers

1. **Presentation Layer** (API Controllers)
   - Separate controllers for different domains (Users, Roles, Stats, PDF)
   - Handles HTTP requests and responses
   - Implements input validation
   - Routes to appropriate services

2. **Business Logic Layer** (Services)
   - Domain-specific services (UserService, RoleService, StatsService, PdfService)
   - Implements core business logic
   - Coordinates operations across domains
   - Handles complex data transformations

3. **Data Access Layer** (EF Core Context)
   - Manages database interactions through Entity Framework Core
   - Implements database configurations and relationships
   - Handles data seeding

4. **Domain Layer** (Models)
   - Contains entity definitions
   - Implements business rules through data annotations
   - Defines relationships between entities

### Key Architectural Patterns

- **Dependency Injection**: Promotes loose coupling between components
- **Single Responsibility Principle**: Each service and controller has a single responsibility
- **DTO Pattern**: Uses data transfer objects for API communication
- **Repository Pattern (via EF Core)**: Abstracts data access logic

## 🛠️ Best Practices

The codebase adheres to the following best practices:

### Code Quality

- **SOLID Principles**: The code follows SOLID principles to ensure maintainability
- **Clean Code**: Methods and classes have single responsibilities with meaningful names
- **Modular Design**: Functionality is separated into domain-specific services and controllers

### Security

- **Input Validation**: All user inputs are validated at the API level
- **Parameter Binding**: Model binding is used to prevent over-posting
- **Exception Handling**: Global exception handling with appropriate status codes

### Performance

- **Async/Await**: Asynchronous programming for I/O-bound operations
- **Eager Loading**: Related entities are loaded efficiently with Include statements
- **Indexing**: Database tables are properly indexed for optimal query performance

### API Design

- **RESTful Principles**: API follows REST conventions
- **Domain-Specific Endpoints**: Endpoints are organized by domain (users, roles, stats, pdf)
- **Consistent Responses**: Standard response formats across all endpoints
- **Swagger Documentation**: Comprehensive API documentation with examples

## 💯 Meeting Developer Evaluation Requirements

This application fulfills the technical requirements specified in the developer evaluation test:

### Backend Requirements

- ✅ **Technology**: Uses .NET 9.0 as specified
- ✅ **Database**: Uses EF Core with code-first approach, data annotations, and migrations
- ✅ **PDF Generation**: Implements Playwright for .NET for high-quality PDF generation

### Features

- ✅ **CRUD Operations**: Provides full REST API for creating, reading, updating, and deleting records
- ✅ **Data Validation**: Implements comprehensive validation before posting data

### Architecture & Integration

- ✅ **Database Design**: Uses a normalized relational database structure
- ✅ **Code Architecture**: Follows clean architecture patterns and best practices

## 🚀 How to Run Locally

### Prerequisites

- .NET 9.0 SDK installed
- Access to Microsoft SQL Server on Azure (or local SQL Server instance)
- Node.js installed (required for Playwright dependencies)
- Frontend application running on `http://localhost:3000` (API is configured to only accept requests from this origin)

### Setup and Run

1. Clone the repository
   ```
   git clone <repository-url>
   ```

2. Navigate to the project directory
   ```
   cd p3mo-user-crud-backend
   ```

3. Configure the database connection string in `appsettings.json` if needed

4. Apply the database migrations:
   ```
   dotnet ef database update
   ```

5. Restore dependencies, build and run the application:
   ```
   dotnet restore
   dotnet build
   dotnet run --urls="http://localhost:5000"
   ```

The API will be available at:
- HTTP: `http://localhost:5000` (or the port specified in your command)

Swagger UI will be available at `http://localhost:5000/swagger`.

## 💾 Database Configuration

The application is configured to use Microsoft SQL Server on Azure with the following credentials:

- **Server**: p3mo-dev-mehmet.database.windows.net
- **Database**: db-mehmet
- **User**: sqladmin
- **Password**: (Set in appsettings.json)

These credentials are already set in the `appsettings.json` file. If you need to change the database connection, update the `ConnectionStrings:DefaultConnection` value.

### Environment Variables

The following environment variables or configuration values can be set in `appsettings.json`:

- `ConnectionStrings:DefaultConnection`: The database connection string
- `FrontendUrl`: The URL of the frontend application (for PDF generation and CORS policy)

## 🔄 Working with Database Migrations

The application uses Entity Framework Core migrations for database schema management. Here's how to work with migrations:

### Prerequisites

Make sure you have EF Core tools installed:
```
dotnet tool install --global dotnet-ef
```

### Creating a Migration

To create a new migration when you make changes to your models:

```
dotnet ef migrations add <MigrationName>
```

For example:
```
dotnet ef migrations add AddRoleEntity
```

### Applying Migrations

To apply all pending migrations to the database:

```
dotnet ef database update
```

To apply migrations up to a specific one:
```
dotnet ef database update <MigrationName>
```

### Removing the Last Migration

If you need to remove the last migration (if it hasn't been applied to the database yet):

```
dotnet ef migrations remove
```

### Resetting the Database

If you need to completely reset your database, you can use the included `drop.sql` script:

1. Connect to your database using SQL Server Management Studio or another SQL client
2. Open and execute the `drop.sql` script
3. The script will:
   - Disable all constraints
   - Delete all data from tables
   - Drop all foreign key constraints
   - Drop all indexes
   - Drop all tables
4. After running the script, restart your application to recreate the database with the initial migration

### Troubleshooting Migrations

If you encounter issues with migrations:

1. **Empty migration files**: If the migration's `Up()` and `Down()` methods are empty, EF Core couldn't detect any changes to your model. Make sure you've made changes to your model classes or entity configurations.

2. **Migration not applying**: Check your connection string and database permissions. Make sure the database user has sufficient rights to create and alter tables.

3. **Conflicting migrations**: If multiple developers are working on the project, make sure to coordinate migrations. It's often best to have a single developer responsible for creating migrations.

4. **Complete reset**: If you need to start fresh, you can run the `drop.sql` script and then create a new initial migration.

5. **Manual database creation**: If EF Core migrations are consistently failing, you can create the database schema manually and add a record to the `__EFMigrationsHistory` table to indicate that migrations have been applied.

## 📊 Database Structure

The application uses Entity Framework Core with a code-first approach, implementing a normalized database design with separate tables for users, user details, and roles.

### Users Table

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | Primary Key, Auto-increment | Unique identifier |
| Email | string | Required, Email format | User's email address |
| IsActive | bool | Default: true | Whether the user is active |
| CreatedAt | DateTime | Default: UTC now | When the user was created |

### UserDetails Table

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | Primary Key, Auto-increment | Unique identifier |
| UserId | int | Foreign Key (Users.Id), Required | Reference to the Users table |
| FirstName | string | Required | User's first name |
| MiddleName | string | Optional | User's middle name |
| LastName | string | Required | User's last name |
| DateOfBirth | DateTime | Required | User's date of birth |
| RoleId | int | Foreign Key (Roles.Id), Required | Reference to the Roles table |
| Country | string | Required | User's country |

### Roles Table

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | int | Primary Key, Auto-increment | Unique identifier |
| Name | string | Required | Role name (e.g., "Admin", "User", "Guest") |
| Description | string | Optional | Description of the role |

### Relationships

- One-to-One relationship between Users and UserDetails (One user has exactly one set of details)
- One-to-Many relationship between Roles and UserDetails (One role can be assigned to many users)
- Foreign key constraints ensure referential integrity
- Cascade delete ensures UserDetails are removed when a User is deleted

### Database Normalization

The database follows normalization principles:
- **First Normal Form (1NF)**: All attributes contain atomic values
- **Second Normal Form (2NF)**: All non-key attributes are fully dependent on the primary key
- **Third Normal Form (3NF)**: No transitive dependencies on the primary key

#### Normalization Benefits

This normalized structure provides several benefits:
- **Reduced Data Redundancy**: Core user information is stored once in the Users table
- **Improved Data Integrity**: IsActive property is maintained only in one location
- **Better Role Management**: Roles are maintained in a separate table allowing for easy management
- **Better Performance**: Frequently-used queries only need to access the relevant tables
- **More Flexible Design**: Future changes can be made to any table without affecting the others

## 🔌 API Endpoints

The API is organized into domain-specific controllers:

### User Management

- `GET /api/users` - Get all users with their details
- `GET /api/users/{id}` - Get user by ID with details
- `POST /api/users` - Create new user with details
- `PUT /api/users/{id}` - Update user and details
- `DELETE /api/users/{id}` - Delete user (and associated details)

### Role Management

- `GET /api/roles` - Get all roles
- `GET /api/roles/{id}` - Get role by ID
- `POST /api/roles` - Create new role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Delete role (if not assigned to any user)

### Statistics Endpoints

- `GET /api/stats/active` - Get count of active vs inactive users
- `GET /api/stats/roles` - Get count of users by role
- `GET /api/stats/registration` - Get count of users grouped by registration month

### PDF Generation

- `GET /api/pdf/{userId}` - Generate a PDF of user details

## 📄 PDF Generation with Playwright

The application uses Playwright for .NET to generate PDFs of user details pages. Here's how it works:

1. When a request is made to `/api/pdf/{userId}`, the application retrieves the user details
2. Playwright opens a headless browser and navigates to the frontend user detail page for that user
3. The page is converted to a PDF document with custom header and footer
4. The PDF is returned to the client as a downloadable file

To configure the frontend URL for PDF generation, set the `FrontendUrl` configuration value in `appsettings.json`.

## 🧪 Testing the API

### Using Swagger

1. Run the application
2. Open your browser and navigate to `/swagger`
3. Use the Swagger UI to test the API endpoints

### Using Postman

1. Run the application
2. Import the collection from the `p3mo-user-crud-backend.postman_collection.json` file
3. Use the Postman collection to test the API endpoints

#### Postman Collection Features

The included Postman collection contains pre-configured requests for all API endpoints:

- **User Management**: Create, Read, Update, and Delete operations
- **Role Management**: Create, Read, Update, and Delete operations
- **Statistics**: Endpoints for retrieving user statistics
- **PDF Generation**: Request for generating user PDFs

The collection uses a variable `{{baseUrl}}` which is set to `http://localhost:5000` by default. You can modify this variable if your API is running on a different port or host.

## 🔧 Troubleshooting Common Issues

### API not running on expected port

If the API is not accessible on the default ports, check:
- Your `launchSettings.json` file for the configured ports
- If another application is already using the port
- Try running with explicit URLs: `dotnet run --urls="http://localhost:5000"`

### Database Connection Issues

If you're having trouble connecting to the database:
- Verify your connection string in `appsettings.json`
- Ensure the SQL Server is running and accessible
- Check firewall settings if connecting to a remote database
- Verify credentials are correct

### PDF Generation Failures

If PDF generation is failing:
- Ensure Node.js is installed (required for Playwright)
- Check that the `FrontendUrl` is correctly configured
- Ensure the frontend is running at `http://localhost:3000` before attempting to generate PDFs
- Verify that the frontend application is running and accessible
- Run `playwright install chromium` to ensure browser dependencies are installed
- Check if your skeleton UI elements use different CSS classes than those specified in the wait logic
- If specific charts aren't rendering in the PDF, add custom wait logic for those elements
- If PDF downloads are not working, check that your browser allows downloads from the application

## 📝 Notes

- The application uses a modular design with separate services and controllers for different domains
- Services follow the Single Responsibility Principle for better maintainability
- Entity Framework Core migrations are used to manage the database schema
- The API includes data seeding for initial user and role data
- CORS is configured to only allow requests from the frontend at `http://localhost:3000`
- The database schema follows normalization principles for optimal data management
- The PdfService includes enhanced waiting strategies to properly capture dynamic content
- The `drop.sql` script is provided for complete database reset when needed 