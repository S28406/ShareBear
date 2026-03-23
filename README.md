# ShareBear 🐻

A full-stack desktop tool-sharing platform built as a Bachelor's thesis project 
at PJAIT Warsaw. Connects tool owners and borrowers in a secure, role-based 
sharing environment.

## Tech Stack

**Client:** WPF (.NET 9, MVVM)  
**Server:** ASP.NET Core (.NET 9), Entity Framework Core, PostgreSQL  
**Auth:** JWT Bearer tokens  
**Docs:** Swagger / OpenAPI  
**Deployment:** Render  
**Testing:** NUnit  

## Features

- Role-based access control (Seller, Admin, Borrower)
- JWT authentication with secure token validation
- Tool listings with server-side pagination and filtered search
- Case-insensitive full-text search using PostgreSQL ILike
- File upload handling for tool images
- Swagger UI with Bearer token support for API exploration
- Database migrations and seeder for initial data
- Multi-project solution (client, server, shared)

## Architecture
```
ShareBear/
├── Pro.Client/   # WPF desktop client (MVVM)
├── Pro.Server/   # ASP.NET Core REST API
└── Pro.Shared/   # Shared DTOs and models
```

## Screenshots
Main Page
<img width="1920" height="997" alt="image" src="https://github.com/user-attachments/assets/fbf122a8-15a8-4cb4-bc7f-618610425e24" />
Tool Description
<img width="1920" height="990" alt="image" src="https://github.com/user-attachments/assets/2d845f5f-9ffa-4d2e-9205-39e895e83540" />
Purchase History
<img width="1920" height="1006" alt="image" src="https://github.com/user-attachments/assets/07c451b3-956b-4ec8-9d88-0ccc0d593637" />
Listed Tools
<img width="1920" height="999" alt="image" src="https://github.com/user-attachments/assets/40202019-721b-4fcc-97f3-15b5a883aa28" />


## Running Locally

### Prerequisites
- .NET 9 SDK
- PostgreSQL (default config assumes `localhost:5432`)

### 1. Configure the Server
In `Pro.Server/appsettings.json`, set your values:
```json
{
  "Jwt": {
    "Issuer": "Pro.Server",
    "Audience": "Pro.Client",
    "Key": "your-secret-key-at-least-32-characters-long"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=toollendingdb;Username=postgres;Password=yourpassword"
  }
}
```

### 2. Run the Server
```bash
cd Pro.Server
dotnet ef database update
dotnet run
```

### 3. Run the Client
```bash
cd Pro.Client


dotnet run
```

The API will be available at `http://localhost:5262` and 
Swagger UI at `http://localhost:5262/swagger`.


## Academic Context
Built as a solo Bachelor's thesis project — grade **4.5/5**.
