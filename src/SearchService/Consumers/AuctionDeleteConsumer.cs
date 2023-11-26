using MassTransit;
using MongoDB.Entities;
using SearchService.Models;
using Shared.Contracts;

namespace SearchService.Consumers;

public class AuctionDeleteConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine("Deleting Auction: " + context.Message.Id);
        
        await DB.DeleteAsync<Item>(context.Message.Id);
    }
}