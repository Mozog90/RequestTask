
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddOpenApi();

var app = builder.Build();

// Enable OpenAPI documentation in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirect HTTP requests to HTTPS when configured
app.UseHttpsRedirection();

// RequestTask health-check endpoint
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        application = "RequestTask API"
    });
})
.WithName("GetHealth");

// Start the application
app.Run();
