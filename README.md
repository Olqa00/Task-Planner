# Planner Application

### Project Overview
The Task Management Application is a system for managing tasks, built using Clean Architecture. The primary goal of the application is to allow users to create, update, delete, and view tasks.

### Key Features:
- Add new tasks.
- Update existing tasks.
- Delete tasks.
- View a list of all tasks.

### Chosen Technologies
1. Clean Architecture
Clean Architecture separates the application into layers, making the codebase easier to maintain, test, and extend.
2. Microsoft SQL Server (MSSQL) with Dapper
MSSQL is a mature and high-performance database management system, ideal for enterprise applications.
Dapper is a lightweight ORM (Object-Relational Mapping) library that offers high performance and simplicity compared to full-fledged ORMs like Entity Framework.
Dapper allows writing raw SQL queries, providing greater control over query optimization.
3. MediatR (CQRS)
MediatR is a library that implements the Mediator pattern, simplifying dependency management between components.
The CQRS (Command Query Responsibility Segregation) pattern separates write operations (Commands) from read operations (Queries), improving code readability and scalability.

4. Logging with Seq
Seq is a log aggregation and analysis tool that enables real-time application monitoring.
Provides an intuitive interface for searching and analyzing logs.

5. Docker
Docker allows containerization of the application and its dependencies ( database, Seq), simplifying deployment and running the application in different environments.

### Testing
#### Unit Tests
- Unit tests are automated tests that focus on verifying the correctness of individual components or units of code, such as methods, classes, or functions, in isolation from the rest of the application. 
- The goal is to ensure that each unit behaves as expected under various conditions.

#### Integration Tests
- Integration tests focus on testing the collaboration between multiple components of the application, such as the database, commands, and queries.
- These tests use a local database (MSSQL in Docker) to ensure that the application interacts correctly with the database.
- Commands (e.g., adding, updating, or deleting tasks) and queries (e.g., retrieving tasks) are tested to verify their behavior in a real-world scenario.

### Getting Started
Prerequisites
Docker (for running MSSQL and Seq).
.NET SDK (version 6.0 or later).

Running the Application
1. Clone the repository
2. Set up the database and Seq using Docker
```bash
cd infra
docker-compose up -d
```
3. Configure the application:
Update the appsettings.json file with the correct connection strings and Seq settings:
```bash
{
  "SqlServer": {
    "SqlConnection": "Server=localhost,5435;User Id=sa;Password=Pass@word;Trust Server Certificate=true"
  },
}
```
Passw0rd "P@ssw0rd" is default, you can change it in infra/.env.

4. Run the application:
```bash
cd src/WebAPI
dotnet run
```