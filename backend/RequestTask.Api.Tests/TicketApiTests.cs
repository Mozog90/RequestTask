
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RequestTask.Api.Models;

namespace RequestTask.Api.Tests;

public class TicketApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TicketApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTicket_WithValidData_ReturnsCreated()
    {
        var request = new
        {
            Title = "Login page not working",
            Description = "Users cannot log in.",
            Type = "Bug",
            Priority = "High"
        };

        var response = await _client.PostAsJsonAsync("/tickets", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var ticket = await response.Content.ReadFromJsonAsync<Ticket>();

        Assert.NotNull(ticket);
        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal("Login page not working", ticket.Title);
        Assert.Equal("Submitted", ticket.Status);
        Assert.Equal("High", ticket.Priority);
    }

    [Fact]
    public async Task GetTicket_WithExistingId_ReturnsTicket()
    {
        var request = new
        {
            Title = "Add search feature",
            Description = "Users need a search option.",
            Type = "Feature",
            Priority = "Medium"
        };

        var createResponse = await _client.PostAsJsonAsync("/tickets", request);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        Assert.NotNull(createdTicket);

        var response = await _client.GetAsync($"/tickets/{createdTicket.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var retrievedTicket = await response.Content.ReadFromJsonAsync<Ticket>();

        Assert.NotNull(retrievedTicket);
        Assert.Equal(createdTicket.Id, retrievedTicket.Id);
        Assert.Equal("Add search feature", retrievedTicket.Title);
    }

    [Fact]
    public async Task CreateTicket_WithEmptyTitle_ReturnsBadRequest()
    {
        var request = new
        {
            Title = "",
            Description = "Testing validation",
            Type = "Bug",
            Priority = "High"
        };

        var response = await _client.PostAsJsonAsync("/tickets", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTicket_WithUnknownId_ReturnsNotFound()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/tickets/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
