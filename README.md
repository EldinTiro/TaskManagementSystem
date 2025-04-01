# TaskManagementSystem

## Start steps:

Navigate to the root directory and using the docker-compose command, start the local db instance of the Sql Server and rabbitMq instance

```bash
docker-compose up -d
```

To build the application using .NET CLI, run the command:
```bash
dotnet build
```

Update the database with already generated migrations using the .NET CLI command:

```bash
dotnet ef database update
```

By updating db some initial data will be seeded as well

To run the application using .NET CLI, run the command:
```bash
dotnet run
```

## Some request examples:
### To Create a Task:
[Method: POST](http://localhost:5131/api/Tasks)
```json
{
  "name": "File explorer",
  "description": "File explorer description",
  "status": 1,
  "assignedTo": "John Doe"
}
```

### To Update a Task:
[Method: PUT](http://localhost:5131/api/Tasks{id})
```json
{
  "name": "Seeded record updated",
  "description": "Seeded record updated description",
  "status": 2,
  "assignedTo": "John Doe",
  "id" : 1
}
```

### To Get all Tasks:
[Method: GET](http://localhost:5131/api/Tasks)
```
No parameters
```

### To Get Task:
[Method: GET](http://localhost:5131/api/Tasks{id})
```
/api/Tasks/1
```

#Features and best practices that are implemented inside the project:
1. Code-First approach
2. Seeding initial data
3. Scaffolding controllers from Entities
4. AutoMapping DTOs to Entity models and reverse with Automapper
5. DataTransferObjects with DataAnotation validation
6. Creating and building Sql Server DB and RabbitMq instance by using docker (docker-compose)
7. Global exception handlers
8. Repository pattern as an abstraction between the intelligence and controller layers
9. Unit test using xUnit library with InMemoryDb
10. Consumers Auto-retries defined in Program.cs
...

#What could be improved:
1. More validations and handled exceptions to avoid 500 errors
2. Using Architecture design patter such is (Clean architecture, Onion, N-tier...)
3. Adding logs (for example Log4Net) and many more...
