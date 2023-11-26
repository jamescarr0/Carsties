using AutoMapper;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;
using Shared.Contracts;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("Consuming Auction Created " + context.Message.Id);
        var item = _mapper.Map<Item>(context.Message);
        await item.SaveAsync();
    }
}