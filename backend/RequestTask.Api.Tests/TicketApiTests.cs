
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RequestTask.Api.Models;

namespace RequestTask.Api.Tests;

public class TicketApiTests : IClassFixture<TicketApiFactory>
{
    private readonly HttpClient _client;

    public TicketApiTests(TicketApiFactory factory)
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
public async Task UpdateTicketStatus_WithValidStatus_SavesChange()
{
    var request = new
    {
        Title = "Status update test",
        Description = "Testing ticket status changes.",
        Type = "Bug",
        Priority = "Medium"
    };

    // Create a ticket
    var createResponse =
        await _client.PostAsJsonAsync("/tickets", request);

    Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

    var ticket = await createResponse.Content.ReadFromJsonAsync<Ticket>();
    Assert.NotNull(ticket);

    // Change its status
    var updateResponse = await _client.PatchAsJsonAsync(
        $"/tickets/{ticket.Id}/status",
        new { Status = "In Development" });

    Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

    // Retrieve the ticket again
    var getResponse = await _client.GetAsync($"/tickets/{ticket.Id}");
    Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

    var updatedTicket =
        await getResponse.Content.ReadFromJsonAsync<Ticket>();

    Assert.NotNull(updatedTicket);
    Assert.Equal("In Development", updatedTicket.Status);
}


    [Fact]
    public async Task GetTicket_WithUnknownId_ReturnsNotFound()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/tickets/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


[Fact]
public async Task UpdateTicketStatus_WithInvalidStatus_ReturnsBadRequest()
{
    // Create a ticket
    var request = new
    {
        Title = "Invalid status test",
        Description = "Testing invalid ticket statuses.",
        Type = "Bug",
        Priority = "Medium"
    };

    var createResponse =
        await _client.PostAsJsonAsync("/tickets", request);

    var ticket = await createResponse.Content.ReadFromJsonAsync<Ticket>();
    Assert.NotNull(ticket);

    // Attempt an invalid status update
    var updateResponse = await _client.PatchAsJsonAsync(
        $"/tickets/{ticket.Id}/status",
        new { Status = "Banana" });

    Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);

    // Verify the original status was not changed
    var getResponse = await _client.GetAsync($"/tickets/{ticket.Id}");
    var savedTicket = await getResponse.Content.ReadFromJsonAsync<Ticket>();

    Assert.NotNull(savedTicket);
    Assert.Equal("Submitted", savedTicket.Status);
}


[Fact]
public async Task UpdateTicketStatus_WithUnknownId_ReturnsNotFound()
{
    // Generate an ID that doesn't belong to any ticket
    var id = Guid.NewGuid();

    // Attempt to update a nonexistent ticket
    var response = await _client.PatchAsJsonAsync(
        $"/tickets/{id}/status",
        new { Status = "In Development" });

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
}


    [Fact]
    public async Task HealthEndpoint_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
