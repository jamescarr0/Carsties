using AuctionService.DTOs;
using AuctionService.Models;
using AutoMapper;
using Shared.Contracts;

namespace AuctionService.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Item>();
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(dest => dest.Item,
                options => options.MapFrom(item => item));
        CreateMap<AuctionDto, AuctionCreated>();
        CreateMap<Auction, AuctionUpdated>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionUpdated>();
    }
}