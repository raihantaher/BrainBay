# BrainBay - Rick and Morty Character Manager

This solution consists of two main components:
1. A console application that fetches character data from the Rick and Morty API and stores it in a SQL database
2. A web API that serves the character data with caching

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full SQL Server instance)
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio or VS Code
3. Update the connection string in `BrainBay.Api/appsettings.json` if needed
4. Run the following commands to create the database:

```bash
cd BrainBay.Core
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. Run the console application to fetch and store characters:

```bash
cd BrainBay.Console
dotnet run
```

6. Run the web API:

```bash
cd BrainBay.Api
dotnet run
```

The API will be available at `https://localhost:7001` (or a similar port).

## API Endpoints

- GET `/api/characters` - Get all characters
- GET `/api/characters/{id}` - Get a specific character by ID
- GET `/api/characters/origin/{origin}` - Get characters by origin
- POST `/api/characters` - Create a new character

## Features

- Fetches character data from the Rick and Morty API
- Stores only alive characters in the database
- Caches character data for 5 minutes
- Provides response headers to indicate data source
- Supports filtering characters by origin
- Auto-increments character IDs

## Limitations

- No UI or view layer has been implemented due to time constraints
- API responses are in JSON format only
- No authentication or authorization mechanisms

## Testing

Run the tests using:

```bash
dotnet test
``` 