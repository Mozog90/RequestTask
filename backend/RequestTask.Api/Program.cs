
using System.Collections.Concurrent;
using RequestTask.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Temporary storage until PostgreSQL is implemented
var tickets = new ConcurrentDictionary<Guid, Ticket>();

// Health-check endpoint
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        application = "RequestTask API"
    });
})
.WithName("GetHealth");

// Create a new ticket
app.MapPost("/tickets", (CreateTicketRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Description) ||
        string.IsNullOrWhiteSpace(request.Type))
    {
        return Results.BadRequest(
            "Title, description, and type are required.");
    }

    var ticket = new Ticket
    {
        Title = request.Title.Trim(),
        Description = request.Description.Trim(),
        Type = request.Type.Trim(),
        Priority = string.IsNullOrWhiteSpace(request.Priority)
            ? "Medium"
            : request.Priority.Trim()
    };

    tickets[ticket.Id] = ticket;

    return Results.Created($"/tickets/{ticket.Id}", ticket);
})
.WithName("CreateTicket");

// Retrieve a ticket by ID
app.MapGet("/tickets/{id:guid}", (Guid id) =>
{
    if (tickets.TryGetValue(id, out var ticket))
    {
        return Results.Ok(ticket);
    }

    return Results.NotFound("Ticket not found.");
})
.WithName("GetTicket");

app.Run();

// Request body for creating tickets
record CreateTicketRequest(
    string? Title,
    string? Description,
    string? Type,
    string? Priority
);


public partial class Program { }
