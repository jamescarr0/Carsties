namespace Shared.Contracts;

public class AuctionUpdated
{
    public required string Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public uint Year { get; set; }
    public string? Color { get; set; }
    public uint Mileage { get; set; }
}