 

# RaceDay RESTful API
## Project Documentation & README

**Project Title:** RaceDay RESTful API  
**Module:** Part 2 – Restful API Development  
**Institution:** IIE Rosebank International 
**Year:** 2026  
**Total Marks:** 100  
**Student Name:** [Lwandle Modjadji]  
**Student Number:** [ST10470766]  
**Date:** [10/16/2026]

---

### 1. Project Overview
The **RaceDay RESTful API** is the backend engine that powers the RaceDay system. It is responsible for handling all data operations, enforcing role-based access control, and providing a secure, scalable interface for the frontend application (Part 3).

This API was built using **ASP.NET Core Web API** in C#, connected to a **SQL Server** database using **Entity Framework Core** (Code-First approach). It provides authentication via **JWT tokens**, secure password hashing with **BCrypt**, interactive documentation through **Swagger**, comprehensive **Unit Tests**, and a **GitHub Actions CI/CD pipeline**.

---

### 2. Learning Outcomes Achieved
The following learning outcomes have been successfully demonstrated in this project:

- ✅ Built a RESTful API using ASP.NET Core Web API in C#.
- ✅ Connected the API to a SQL Server database using Entity Framework Core.
- ✅ Implemented role-based session authorization for Organiser and Participant roles.
- ✅ Wrote and ran unit tests for API endpoints.
- ✅ Configured automated CI/CD using GitHub Actions.

---

### 3. Technology Stack

| Component | Technology |
| :--- | :--- |
| **Framework** | .NET 8.0 (ASP.NET Core Web API) |
| **Language** | C# |
| **Database** | SQL Server (LocalDB) |
| **ORM** | Entity Framework Core (Code-First) |
| **Authentication** | JWT (JSON Web Tokens) |
| **Password Security** | BCrypt.Net-Next |
| **API Documentation** | Swagger / OpenAPI |
| **Testing** | xUnit + EF Core In-Memory Database |
| **CI/CD** | GitHub Actions |

---

### 4. Project Structure

```text
RaceDayAPI/
│
├── .github/
│   └── workflows/
│       └── dotnet.yml              # GitHub Actions CI/CD pipeline
│
├── Controllers/                    # API Endpoints
│   ├── AuthController.cs           # Register & Login
│   ├── EventsController.cs         # Event CRUD operations
│   ├── CategoriesController.cs     # Event categories
│   ├── EnrollmentsController.cs    # Participant enrollments
│   ├── ResultsController.cs        # Event results
│   └── UsersController.cs          # User profile management
│
├── Data/
│   └── RaceDayDbContext.cs         # EF Core Database Context
│
├── DTOs/                           # Data Transfer Objects
│   ├── UserRegisterDto.cs
│   ├── UserLoginDto.cs
│   ├── EventDto.cs
│   ├── EnrollmentDto.cs
│   └── ResultDto.cs
│
├── Models/                         # Database Entities
│   ├── User.cs
│   ├── Event.cs
│   ├── Category.cs
│   ├── EventEnrollment.cs
│   └── Result.cs
│
├── RaceDayAPI.Tests/               # Unit Test Project
│   ├── AuthControllerTests.cs
│   ├── EventsControllerTests.cs
│   └── EnrollmentsControllerTests.cs
│
├── appsettings.json                # Configuration & Connection Strings
├── Program.cs                      # Application Startup & DI
└── README.md                       # This file
```

---

### 5. Database Design

The database follows a Code-First approach in Entity Framework Core, matching the Part 1 ERD specification.

#### Core Entities

| Entity | Description |
| :--- | :--- |
| **User** | Stores Name, Surname, Email, Hashed Password, and Role (`Organiser` or `Participant`). |
| **Event** | Stores event details (Name, Description, Date, Location, Distance, EventType). Linked to an Organiser. |
| **Category** | Stores event categories (e.g., "Under 20", "10km"). Linked to an Event. |
| **EventEnrollment** | Links a Participant, an Event, and a Category together. |
| **Result** | Stores FinishTime and FinishingPosition for a specific EventEnrollment. |

#### Relationships

- **One Organiser** → manages **Many Events**
- **One Event** → has **Many Categories** and **Many Enrollments**
- **One Participant** → has **Many Enrollments**
- **One EventEnrollment** → has **One Result**

---

### 6. API Endpoints

All endpoints are documented in Swagger and accessible at `/swagger` once the application is running.

#### 6.1 Authentication (`/api/auth`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| POST | `/api/auth/register` | Public | Register a new user (Organiser or Participant). |
| POST | `/api/auth/login` | Public | Login and receive a JWT token. |

#### 6.2 User Profile (`/api/users`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| GET | `/api/users/profile` | Authenticated | View own profile information. |
| PUT | `/api/users/profile` | Authenticated | Update own profile information. |

#### 6.3 Events (`/api/events`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| GET | `/api/events` | Authenticated | View all events. |
| GET | `/api/events/{id}` | Authenticated | View a specific event. |
| POST | `/api/events` | **Organiser** | Create a new event. |
| PUT | `/api/events/{id}` | **Organiser** | Update an existing event. |
| DELETE | `/api/events/{id}` | **Organiser** | Delete an event. |

#### 6.4 Categories (`/api/events/{eventId}/categories`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| GET | `/api/events/{id}/categories` | Authenticated | View categories for a specific event. |
| POST | `/api/events/{id}/categories` | **Organiser** | Create a new category for an event. |

#### 6.5 Enrollments (`/api/enrollments`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| POST | `/api/enrollments` | **Participant** | Enroll in an event by selecting a category. |
| GET | `/api/enrollments/event/{id}` | **Organiser** | View all enrollments for a specific event. |

#### 6.6 Results (`/api/results`)
| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| POST | `/api/results` | **Organiser** | Capture finish times and finishing positions. |
| GET | `/api/results/participant/{id}` | **Participant** | View own results. |

---

### 7. Security Implementation

#### 7.1 Password Hashing
All passwords are hashed using **BCrypt** before being stored in the database. Plain text passwords are never stored or logged.

```csharp
string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
```

#### 7.2 JWT Authentication
Upon successful login, a **JSON Web Token (JWT)** is generated containing the user's ID and Role. This token must be included in the `Authorization` header of subsequent requests.

#### 7.3 Role-Based Access Control (RBAC)
Role enforcement is implemented using `[Authorize]` attributes on controllers:
- `[Authorize(Roles = "Organiser")]` – Organiser-only functionality
- `[Authorize(Roles = "Participant")]` – Participant-only functionality
- `[Authorize]` – Any authenticated user

This ensures Organiser functionality is not accessible to a Participant and vice versa.

---

### 8. Swagger Integration

Swagger is integrated into the API and provides a fully interactive UI for testing endpoints.

**Features:**
- All endpoints are visible and testable directly from the browser.
- Each endpoint has a clear description, expected data, and response type.
- JWT Bearer authentication is configured in Swagger, allowing testers to authorize and test protected endpoints.

**Access URL:** `https://localhost:[port]/swagger`

**Steps to Authenticate in Swagger:**
1. Register via `/api/auth/register`.
2. Login via `/api/auth/login` and copy the JWT token.
3. Click the **Authorize** button at the top of the Swagger page.
4. Paste the token in the format: `Bearer {your_token}`.
5. Click **Authorize** and test all protected endpoints.

---

### 9. Unit Testing

Unit tests were written using **xUnit** and an **In-Memory Database** to isolate tests from production data.

#### Test Coverage
| Test Area | Scenarios Tested |
| :--- | :--- |
| **User Registration & Login** | Successful registration, duplicate email rejection, valid login, invalid password. |
| **Event Management** | Organiser creates, updates, and deletes events successfully. |
| **Role-Based Access** | Participants are rejected (403 Forbidden) when accessing Organiser-only endpoints. |
| **Event Enrollments** | Participants enroll successfully; enrollment correctly records the participant, event, and category. |

#### Running Tests
```bash
dotnet test
```

---

### 10. CI/CD Pipeline (GitHub Actions)

A `.github/workflows/dotnet.yml` workflow is configured to automate building and testing.

**Trigger Events:**
- Every `push` to the `main` branch.
- Every `pull_request` targeting the `main` branch.

**Workflow Steps:**
1. Checkout the repository.
2. Setup .NET 8.0 SDK.
3. Restore NuGet dependencies.
4. Build the solution.
5. Run all unit tests.

**Requirement:** All tests must pass locally and through GitHub Actions before code is merged.

---

### 11. Getting Started
Trigger CI pipeline
#### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code
- SQL Server (LocalDB or Express)

#### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YourUsername/RaceDayAPI.git
   cd RaceDayAPI
   ```

2. **Configure the database connection:**
   Open `appsettings.json` and update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RaceDayDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Apply database migrations:**
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI:**
   Navigate to `https://localhost:[port]/swagger`.

---

### 12. Conclusion

The **RaceDay RESTful API** has been successfully implemented to meet all requirements outlined in Part 2 of the assignment. It provides:

- A secure authentication system using JWT and BCrypt.
- Strict role-based access control for Organisers and Participants.
- Complete CRUD functionality for events, categories, enrollments, and results.
- Comprehensive Swagger documentation for live endpoint testing.
- Full unit test coverage ensuring reliable behavior.
- An automated CI/CD pipeline via GitHub Actions.
- Trigger CI pipelines
- 

The system is robust, scalable, and ready to be integrated with the RaceDay frontend application in Part 3.

---

**© The Independent Institute of Education (Pty) Ltd 2026**
Trigger CI pipeline
