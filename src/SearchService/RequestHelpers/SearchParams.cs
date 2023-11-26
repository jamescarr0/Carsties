namespace SearchService.RequestHelpers;

public class SearchParams
{
    public string? SearchTerm { get; set; }
    public ushort PageNumber { get; set; } = 1;
    public ushort PageSize { get; set; } = 4;
    public string? Winner { get; set; }
    public string? Seller { get; set; }
    public string? OrderBy { get; set; }
    public string? Filter { get; set; }
}