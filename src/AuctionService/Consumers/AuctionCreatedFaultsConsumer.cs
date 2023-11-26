using MassTransit;
using Shared.Contracts;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultsConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("TODO: Consuming faulty auction creation.  Read faulty messages here and act on them.");
    }
}