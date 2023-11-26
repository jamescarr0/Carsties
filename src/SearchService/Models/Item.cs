using MongoDB.Entities;

namespace SearchService.Models;

public class Item : Entity
{
    public int ReservePrice { get; set; }
    public required string Seller { get; set; }
    public string? Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBig { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public DateTime AuctionEndDate { get; set; }
    public required string Status { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public uint Year { get; set; }
    public required string Color { get; set; }
    public int Mileage { get; set; }
    public required string ImageUrl { get; set; }
}