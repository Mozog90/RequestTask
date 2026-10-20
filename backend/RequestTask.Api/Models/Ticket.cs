
namespace RequestTask.Api.Models;

public class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Type { get; init; } = string.Empty;

    public string Priority { get; init; } = "Medium";

    public string Status { get; init; } = "Submitted";

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
