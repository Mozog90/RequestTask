
using Microsoft.EntityFrameworkCore;
using RequestTask.Api.Data;
using RequestTask.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Connect Entity Framework Core to PostgreSQL
builder.Services.AddDbContext<RequestTaskDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("RequestTask")
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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

// Create a ticket and save it to PostgreSQL
app.MapPost("/tickets", async (
    CreateTicketRequest request,
    RequestTaskDbContext db) =>
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

    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    return Results.Created($"/tickets/{ticket.Id}", ticket);
})
.WithName("CreateTicket");

// Retrieve a ticket from PostgreSQL
app.MapGet("/tickets/{id:guid}", async (
    Guid id,
    RequestTaskDbContext db) =>
{
    var ticket = await db.Tickets.FindAsync(id);

    if (ticket is null)
    {
        return Results.NotFound("Ticket not found.");
    }

    return Results.Ok(ticket);
})
.WithName("GetTicket");

app.Run();

record CreateTicketRequest(
    string? Title,
    string? Description,
    string? Type,
    string? Priority
);

public partial class Program { }
