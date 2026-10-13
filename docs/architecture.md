# RequestTask — System Architecture

## 1. Project Overview

RequestTask is an AI-assisted ticketing system designed to help product teams submit software development requests and route them to the appropriate development teams.

The goal is to reduce manual ticket triage while keeping responsibility, progress, and decisions visible throughout the development process.

The system will support multiple departments and organizations without being tied to a particular government agency or business.

## 2. User Roles

The application will initially support five roles:

- **Product Team:** Creates tickets for bugs, features, and application changes. Can view progress and provide additional information.
- **Developer:** Receives assignments, works on tickets, updates progress, and sends completed work to QA.
- **QA Tester:** Verifies completed work and either approves it or returns it for correction.
- **Team Lead:** Reviews complex or incorrectly routed tickets, manages exceptions, and can reassign or reject work with an explanation.
- **Administrator:** Manages users, teams, permissions, and routing settings.

## 3. Ticket Workflow

The proposed workflow is:

1. A product team member submits a ticket.
2. The system saves the ticket and requests AI analysis.
3. AI suggests keywords, categories, complexity, and an appropriate team.
4. Routing rules determine whether the ticket can be routed automatically or requires team lead review.
5. A developer receives an assignment and begins work.
6. The developer submits the completed work to QA.
7. QA tests the change and either verifies it or returns it to development.
8. Verified tickets are marked as completed.

Team leads will not need to approve every ticket. Routine tickets can be routed automatically, while uncertain or high-risk tickets require human review.

Team leads can redirect tickets, request additional information, or reject them with a documented reason.

## 4. Ticket Ownership and Status

Each ticket will have a current status, assigned team, and optional individual owner.

Planned statuses include:

- Submitted
- Triaging
- Awaiting Assignment
- In Development
- In QA
- Completed
- Needs Information
- Rejected

If QA identifies a problem, the ticket returns to development.

Ownership transfers when the next person accepts an assignment. Tickets awaiting assignment remain in a team queue.

All changes to ownership, assignment, and status will be recorded in an activity history.

## 5. Proposed Technology Stack

| Component          | Technology                                           |
| ------------------ | ---------------------------------------------------- |
| Frontend           | React and TypeScript                                 |
| Backend            | C# and ASP.NET Core Web API                          |
| Database           | PostgreSQL                                           |
| Database Access    | Entity Framework Core                                |
| Authentication     | ASP.NET Core Identity                                |
| AI                 | Microsoft.Extensions.AI with a configurable provider |
| Backend Testing    | xUnit                                                |
| End-to-End Testing | Playwright                                           |
| Containerization   | Docker                                               |

## 6. Application Architecture

The application will initially use a modular monolith architecture.

The main modules will be:

**Ticket Management**

- Creates and updates tickets.
- Stores comments, descriptions, and attachments.
- Maintains ticket history.

**Workflow and Routing**

- Controls valid ticket status changes.
- Manages team queues and developer assignments.
- Applies routing rules and handles escalation.

**AI Processing**

- Analyzes submitted ticket descriptions.
- Suggests keywords, categories, and team assignments.
- Provides complexity estimates and potential duplicate matches.

**User Management**

- Handles authentication and authorization.
- Maintains users, roles, and teams.

**QA Management**

- Records verification results.
- Returns failed tickets to development.
- Maintains testing history.

The frontend will communicate with the backend through REST APIs. The backend will interact with PostgreSQL and the AI integration module.

AI recommendations will be treated as suggestions, not instructions. The application will validate recommendations before using them and continue supporting manual routing when AI is unavailable.

## 7. Initial Database Design

The proposed database will include:

| Table             | Purpose                                      |
| ----------------- | -------------------------------------------- |
| Users             | Account and profile information              |
| Teams             | Development and QA teams                     |
| TeamMembers       | User and team relationships                  |
| Tickets           | Descriptions, status, type, and priority     |
| TicketAssignments | Assignment and ownership history             |
| TicketEvents      | Status changes, comments, and decisions      |
| AIAnalyses        | Keywords, categories, and AI recommendations |
| QAResults         | Testing outcomes and feedback                |

The exact relationships and additional fields will be defined during database implementation.

## 8. Security Considerations

The system will use authentication and role-based authorization to prevent unauthorized changes.

Ticket updates and routing decisions will be validated by the backend.

Sensitive credentials will not be stored in source code. AI processing will use synthetic development data, and sensitive ticket content will not be sent to external AI providers without appropriate controls.

## 9. Initial Development Milestones

**Milestone 1 — Planning**

- Define project requirements.
- Design the initial architecture.
- Document the ticket workflow and roles.

**Milestone 2 — Core Application**

- Create the backend API and database.
- Implement ticket creation, retrieval, and updates.
- Build an initial frontend.

**Milestone 3 — Workflow**

- Implement assignments and ticket status transitions.
- Add basic user authentication and permissions.
- Implement development-to-QA handoffs.

**Milestone 4 — AI Integration**

- Add keyword and category suggestions.
- Implement basic routing rules.
- Support escalation for uncertain tickets.

**Milestone 5 — Testing and Improvements**

- Add automated tests.
- Evaluate module boundaries and refactor where necessary.
- Prepare the application for deployment.

## 10. Future Architectural Improvements

Potential improvements include:

- Background processing for AI analysis and notifications.
- Configurable routing rules for different departments.
- AI-assisted duplicate detection.
- Developer workload management.
- Reporting dashboards.
- Containerized deployment and performance improvements.

The architecture will be evaluated and refined as development progresses.

## Current Status

The project is in the requirements and architecture planning phase. The components, workflows, and database structure described here are proposed designs and have not yet been implemented.
