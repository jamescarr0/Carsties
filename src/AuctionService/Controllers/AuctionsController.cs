using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {
        IQueryable<Auction> query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.ModifiedDate.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        Auction? auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        Auction auction = _mapper.Map<Auction>(createAuctionDto);

        // TODO: Add Current User As Seller.
        auction.Seller = "Test";
        await _context.AddAsync(auction);

        var newAuction = _mapper.Map<AuctionDto>(auction);
        
        // We have configured Mass Transit to use Entity Framework now this is part of the same transaction
        // If the service bus is down, a rollback happens, (ATOMICITY).  Messages are not published, and nothing
        // is saved to the database.  The message will be saved in the outbox.
        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));
        
        bool result = await _context.SaveChangesAsync() > 0;
        if (!result)
        {
            return BadRequest("Failed to create auction");
        }
        
        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        Auction? auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        // TODO: Check Seller == Username

        if (auction == null) return NotFound($"Auction with id {id} not found.");

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        // Publish before we save.  If we cant publish we will exit, and not save to the database ;-)
        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));
        
        int changesMade = await _context.SaveChangesAsync();
        return Ok("Success, saved changes: " + changesMade);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        Auction? auction = await _context.Auctions.FindAsync(id);
        // TODO:  Check seller == username

        if (auction == null) return NotFound();

        await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });
            
        _context.Auctions.Remove(auction);
        bool result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest();

        return Ok();
    }
}