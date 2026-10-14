# RequestTask

RequestTask is an AI-assisted ticketing and workflow management system designed to help product and development teams organize, assign, and track software development requests.

The goal is to simplify how tickets move between teams by using AI to analyze incoming requests, identify relevant information, estimate complexity, and recommend where work should be assigned.

The system is intended to be flexible enough for different organizations and departments, including government agencies.

## Project Goals

- Simplify ticket submission and assignment.
- Reduce the need for manual ticket triage.
- Use AI to help classify and route development requests.
- Match tickets to appropriate teams and developer skill levels.
- Maintain clear ticket ownership throughout development.
- Support QA testing and feedback.
- Keep a history of ticket activity and decisions.
- Provide a modular architecture that can evolve over time.

## Planned Features

### Ticket Management

- Create tickets for bugs, feature requests, and application changes.
- Include descriptions, priorities, categories, and attachments.
- Track ticket status, ownership, and assignment history.
- Allow assigned users to update progress and add comments.
- Support returning or redirecting tickets with an explanation.

### AI-Assisted Ticket Analysis

- Extract relevant keywords from ticket descriptions.
- Suggest ticket categories and tags.
- Identify potentially duplicate tickets.
- Estimate ticket complexity and risk.
- Recommend appropriate teams or developer skill levels.
- Flag uncertain or high-risk tickets for human review.

### Intelligent Ticket Routing

- Route routine, low-risk tickets using AI recommendations and predefined rules.
- Send complex, high-risk, or uncertain tickets to a team lead for review.
- Consider developer skills and availability when assigning work.
- Allow authorized users to override routing decisions.
- Record routing decisions and reasons for reassignment.

### Developer Workflow

- Developers receive tickets assigned to them.
- Developers can accept, update, or return assignments.
- Tickets move through defined development statuses.
- Completed development work is submitted to QA for verification.

### QA Testing

- QA testers receive tickets awaiting verification.
- Testers can record results and provide feedback.
- Failed tickets return to development for corrections.
- Successfully verified tickets can be closed.
- Testing history is preserved for future reference.

### User Management

- Authentication and role-based authorization.
- Roles for product teams, developers, QA testers, team leads, and administrators.
- Permissions based on user responsibilities.
- Management of teams and department assignments.

## Ticket Workflow

1. **Product Team**
   - Submits a bug report, feature request, or application change.

2. **AI Analysis**
   - Analyzes the ticket description.
   - Extracts keywords and suggests categories.
   - Identifies potential duplicates.
   - Estimates complexity and risk.
   - Recommends an appropriate destination.

3. **Ticket Routing**
   - Routine tickets can move automatically to the appropriate development queue.
   - Assignment rules can consider skills, availability, and workload.
   - Complex, high-risk, or uncertain tickets are escalated to a team lead.
   - Team leads can approve, redirect, return, or reject tickets with an explanation.

4. **Development**
   - An assigned developer takes ownership of the ticket.
   - The developer investigates and implements the requested changes.
   - Incorrect assignments can be returned or redirected.
   - Completed work is submitted for testing.

5. **QA Testing**
   - A tester takes ownership and verifies the implementation.
   - Failed tests return the ticket to development with feedback.
   - Successful verification allows the ticket to proceed to completion.

6. **Completion**
   - The ticket is marked as completed.
   - Assignment history, development updates, and testing results are preserved.

AI assists with classification and routing, but authorized users can review and override its recommendations.

## Proposed Technology Stack

### Frontend

- React
- TypeScript

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core

### Database

- PostgreSQL

### AI Integration

- Microsoft.Extensions.AI
- Configurable AI model provider
- AI-assisted classification, keyword extraction, and routing recommendations

### Authentication and Authorization

- ASP.NET Core Identity
- Role-based access control

### Testing

- xUnit for backend unit and integration testing
- Playwright for end-to-end testing

### Development and Deployment

- Git and GitHub
- Docker and Docker Compose
- Visual Studio Code

## Proposed Architecture

RequestTask will initially use a modular monolith architecture.

The application will be organized into separate components responsible for:

- Ticket Management
- Workflow Management
- AI Processing
- User Management
- QA Management
- Notifications

The React frontend will communicate with the ASP.NET Core backend through REST APIs.

PostgreSQL will provide persistent storage for users, tickets, teams, assignments, AI recommendations, and activity history.

AI processing will be separated from core ticket management so that ticket creation and manual routing can continue if the AI service is unavailable.

## System Design Diagram

![RequestTask System Design](docs/images/system-design-overview.png)

## Initial Development Scope (MVP)

The first version will focus on building a complete ticket lifecycle.

Planned MVP functionality:

1. User authentication and basic roles.
2. Ticket creation and management.
3. Basic AI keyword extraction and category suggestions.
4. Rule-based ticket routing with human escalation.
5. Developer assignment and status updates.
6. QA verification and return-to-development workflow.
7. Ticket ownership and activity history.
8. Basic automated testing.

## Future Enhancements

- AI-assisted duplicate ticket detection.
- More advanced complexity and risk analysis.
- Developer workload and availability tracking.
- AI-generated test case suggestions.
- Configurable routing rules for different organizations.
- Dashboards and reporting.
- Background notifications.
- Performance monitoring and caching.
- Deployment and scalability improvements.

## Running the Backend Locally

### Requirements

- .NET 10 SDK
- Visual Studio Code or Visual Studio

### Build the API

From the root of the RequestTask repository, run:

```powershell
dotnet build backend/RequestTask.Api
```

### Start the API

```powershell
dotnet run --project backend/RequestTask.Api
```

The API will start using the address configured in
`backend/RequestTask.Api/Properties/launchSettings.json`.

### Test the Health Endpoint

Open a second terminal while the API is running:

```powershell
Invoke-RestMethod http://localhost:5293/health
```

Expected JSON response:

```json
{
  "status": "Healthy",
  "application": "RequestTask API"
}
```

The health endpoint confirms that the application is responding.
Database and external service health checks will be added later.

### Current Backend Status

- ASP.NET Core 10 Web API initialized
- Default weather forecast example removed
- Health-check endpoint implemented and tested
- Backend builds and runs locally
- Database integration planned for future development

## Project Status

**Current Phase:** Requirements and Architecture Planning

RequestTask is currently in the initial planning stage. The features and architectural components described in this README represent the intended design and will be implemented incrementally.

## Academic Project

RequestTask is being developed independently as part of AD371, with a focus on software architecture, modularity, maintainability, system design, and iterative development.
