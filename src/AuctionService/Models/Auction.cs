namespace AuctionService.Models;

public class Auction
{
    public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public required string Seller { get; set; }
    public string? Winner { get; set; }
    public int? SoldAmount { get; set; }
    public int? CurrentHighBig { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public DateTime AuctionEndDate { get; set; }
    public Status Status { get; set; }
    public required Item Item { get; set; }
}