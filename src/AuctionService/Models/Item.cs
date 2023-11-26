using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Models;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public uint Year { get; set; }
    public required string Color { get; set; }
    public int Mileage { get; set; }
    public required string ImageUrl { get; set; }
    
    // Nav properties (FK)
    public Auction? Auction { get; set; }
    public Guid AuctionId { get; set; }
}