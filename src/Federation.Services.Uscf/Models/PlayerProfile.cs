namespace Federation.Services.Uscf.Models;

public class PlayerProfile
{
    public string? UscfId { get; set; }

    public string? PlayerName { get; set; }

    public string? RegularRating { get; set; }
    public string? QuickRating { get; set; }
    public string? BlitzRating { get; set; }

    public string? OnlineRegularRating { get; set; }
    public string? OnlineQuickRating { get; set; }
    public string? OnlineBlitzRating { get; set; }

    public string? State { get; set; }

    public string? Gender { get; set; }

    public DateTime? ExpiresOn { get; set; }

    public DateTime? LastChangeDate { get; set; }

    public int? OverallRanking { get; set; }
    public int? OverallRankingOutOf { get; set; }

    public int? StateRanking { get; set; }
    public int? StateRankingOutOf { get; set; }
}