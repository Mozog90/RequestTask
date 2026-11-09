# RequestTask

RequestTask is an AI-assisted ticketing and workflow management system designed to help product and development teams organize, assign, and track software development requests.

The goal is to simplify how tickets move between teams by using AI to analyze incoming requests, identify relevant information, and recommend where work should be assigned.

The system is designed to support different organizations and departments, including government agencies.

## Project Goals

- Simplify ticket submission and management.
- Reduce manual ticket triage.
- Use AI to classify and route development requests.
- Match tickets to appropriate teams and developers.
- Track ticket status and ownership.
- Support QA testing and feedback.
- Maintain a history of ticket activity and decisions.
- Use a modular architecture that can expand over time.

## Planned Features

### Ticket Management

- Create tickets for bugs, feature requests, and application changes.
- Store descriptions, priorities, and ticket statuses.
- Update ticket status throughout development.
- Track assignments, ownership, and ticket history.

### AI-Assisted Ticket Analysis

- Extract keywords from ticket descriptions.
- Suggest categories and priorities.
- Estimate ticket complexity.
- Recommend appropriate development teams.
- Flag uncertain requests for human review.

### Ticket Routing

- Route routine tickets automatically when appropriate.
- Send complex requests to a team lead.
- Allow team leads to redirect or reject requests.
- Track assignment and routing decisions.

### Developer and QA Workflow

- Developers receive and work on assigned tickets.
- Completed development work moves to QA.
- QA testers verify changes and record results.
- Failed tickets return to development.
- Verified tickets can be marked as completed.

### User Management

- Authentication and role-based permissions.
- Roles for Product Team, Developer, QA Tester, Team Lead, and Administrator.

## Ticket Workflow

The planned workflow is:

1. Product Team submits a ticket.
2. AI analyzes the request and suggests routing.
3. The ticket is assigned automatically or sent to a team lead.
4. A developer works on the request.
5. Completed work is submitted to QA.
6. QA approves the work or returns it for corrections.
7. Verified tickets are marked as completed.

AI will assist with routing, but human review and manual assignment will remain available.

The full workflow is still under development.

## Technology Stack

| Component                    | Technology                   |
| ---------------------------- | ---------------------------- |
| Frontend (Planned)           | React, TypeScript            |
| Backend                      | C#, ASP.NET Core 10          |
| Database                     | PostgreSQL 16                |
| Database Access              | Entity Framework Core        |
| Authentication (Planned)     | ASP.NET Core Identity        |
| AI (Planned)                 | Microsoft.Extensions.AI      |
| Testing                      | xUnit, SQLite in-memory      |
| End-to-End Testing (Planned) | Playwright                   |
| Development                  | Docker, Git, GitHub, VS Code |

## System Architecture

RequestTask uses a modular monolith architecture.

The planned application modules include:

- Ticket Management
- Workflow and Routing
- AI Processing
- User Management
- QA Management

The backend communicates with PostgreSQL using Entity Framework Core. The planned React frontend will communicate with the backend through REST APIs.

AI processing will remain separate from core ticket management so the application can continue functioning if AI services are unavailable.

More details are available in [Architecture Documentation](docs/architecture.md).

### System Design Diagram

![RequestTask System Design](docs/images/RequestTask%20System%20Design%20Overview.png)

## Current Backend Features

The following features have been implemented:

- ASP.NET Core 10 Web API
- PostgreSQL persistence using Entity Framework Core
- Initial database migration
- Ticket model with ID, title, description, type, priority, status, and creation date
- Ticket creation and retrieval
- Ticket status updates
- Validation for required fields and allowed statuses
- Automated API integration tests
- GitHub Actions for backend builds and tests

### API Endpoints

| Method | Endpoint               | Description              |
| ------ | ---------------------- | ------------------------ |
| GET    | `/health`              | Check API health         |
| POST   | `/tickets`             | Create a ticket          |
| GET    | `/tickets/{id}`        | Retrieve a ticket        |
| PATCH  | `/tickets/{id}/status` | Update a ticket's status |

### Ticket Creation

Example request to `POST /tickets`:

```json
{
  "title": "Login page not working",
  "description": "Users cannot log in.",
  "type": "Bug",
  "priority": "High"
}
```

A valid request returns HTTP 201 Created.

Tickets receive a unique ID and start with the status `Submitted`.

### Ticket Status Updates

The `PATCH /tickets/{id}/status` endpoint allows an existing ticket's status to be changed.

Example request:

```json
{
  "status": "In Development"
}
```

Supported statuses:

- Submitted
- Triaging
- Awaiting Assignment
- In Development
- In QA
- Completed
- Needs Information
- Rejected

The API validates the requested status and saves valid changes to PostgreSQL.

- Valid updates return HTTP 200.
- Invalid statuses return HTTP 400.
- Nonexistent tickets return HTTP 404.

Role-based permissions and restrictions on status transitions will be added later.

## Running the Backend Locally

### Requirements

- .NET 10 SDK
- Docker Desktop
- PostgreSQL 16 container
- Visual Studio Code or Visual Studio

### Start PostgreSQL

Start Docker Desktop, then run:

```powershell
docker start requesttask-postgres
```

Check that the database container is running:

```powershell
docker ps
```

The local development database uses port `5434`.

If setting up the project on a new computer, create a PostgreSQL container first:

```powershell
docker run --name requesttask-postgres -e POSTGRES_USER=requesttask -e POSTGRES_PASSWORD=YOUR_LOCAL_PASSWORD -e POSTGRES_DB=requesttask -p 127.0.0.1:5434:5432 -v requesttask_pgdata:/var/lib/postgresql/data -d postgres:16
```

Replace `YOUR_LOCAL_PASSWORD` with a development password.

### Configure the Connection

The application uses .NET User Secrets to keep database credentials outside the repository.

Set the connection string using:

```powershell
dotnet user-secrets set "ConnectionStrings:RequestTask" "Host=localhost;Port=5434;Database=requesttask;Username=requesttask;Password=YOUR_LOCAL_PASSWORD" --project backend/RequestTask.Api/RequestTask.Api.csproj
```

Use the same password configured for PostgreSQL.

Do not commit passwords or connection secrets to GitHub.

### Apply Database Migrations

Run:

```powershell
dotnet ef database update --project backend/RequestTask.Api/RequestTask.Api.csproj
```

This applies the existing database migrations.

The `dotnet-ef` command-line tool must be installed.

### Build the Backend

From the repository root:

```powershell
dotnet build backend/RequestTask.Api/RequestTask.Api.csproj
```

### Run the API

```powershell
dotnet run --project backend/RequestTask.Api/RequestTask.Api.csproj
```

The development API runs at:

http://localhost:5293

### Test the Health Endpoint

Open another PowerShell terminal:

```powershell
Invoke-RestMethod http://localhost:5293/health
```

Expected response:

```json
{
  "status": "Healthy",
  "application": "RequestTask API"
}
```

This endpoint confirms the API is responding. It does not currently check database connectivity.

## Automated Testing

RequestTask uses xUnit for backend integration testing.

Tests run against a temporary SQLite in-memory database so they do not modify PostgreSQL development data.

### Test Coverage

| Test                            | Expected Result                     |
| ------------------------------- | ----------------------------------- |
| Create valid ticket             | HTTP 201                            |
| Retrieve existing ticket        | HTTP 200                            |
| Reject empty ticket title       | HTTP 400                            |
| Retrieve nonexistent ticket     | HTTP 404                            |
| Check health endpoint           | HTTP 200                            |
| Update ticket with valid status | HTTP 200, status saved              |
| Reject invalid status           | HTTP 400, original status unchanged |
| Update nonexistent ticket       | HTTP 404                            |

Run the tests:

```powershell
dotnet test backend/RequestTask.Api.Tests/RequestTask.Api.Tests.csproj
```

**Latest local result: 8 tests passed, 0 failed.**

GitHub Actions is configured to run the backend build and automated tests.

## Development Progress

### Completed

**Issue #1 — Requirements and Architecture**

- Defined the initial requirements and system design.
- Documented the architecture and planned ticket workflow.

**Issue #3 — Backend Setup**

- Created the ASP.NET Core backend.
- Implemented the health-check endpoint.

**Issue #5 — Initial Ticket API**

- Added the Ticket model.
- Implemented ticket creation and retrieval.
- Added the first five automated tests.

**Issue #7 — PostgreSQL Persistence**

- Integrated PostgreSQL and Entity Framework Core.
- Created the database migration.
- Replaced temporary in-memory ticket storage.
- Verified tickets persist after API restarts.

### Current Development

**Issue #9 — Ticket Status Updates**

- Implemented `PATCH /tickets/{id}/status`.
- Added status validation and database updates.
- Added three automated tests.
- Confirmed all eight tests pass locally.
- Preparing the changes for a pull request.

## Next Steps

- Add valid workflow transition rules.
- Implement developer and team assignments.
- Add ticket activity history.
- Add authentication and role-based permissions.
- Begin AI-assisted ticket classification and routing.
- Develop the initial React frontend.

## Project Status

**Current Phase:** Core Backend Development

RequestTask currently supports ticket creation, retrieval, and status updates with PostgreSQL persistence.

The next phase will expand ticket management into developer assignments, workflow controls, and AI-assisted routing.

## Academic Project

RequestTask is being developed independently as part of AD371, with a focus on software architecture, modularity, maintainability, system design, and iterative development.
