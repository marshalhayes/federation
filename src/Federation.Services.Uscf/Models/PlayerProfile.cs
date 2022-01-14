namespace Federation.Services.Uscf.Models;

public class PlayerProfile
{
    public string? UscfId { get; set; }

    public string? PlayerName { get; set; }

    public string? RegularRating { get; set; }
    public string? QuickRating { get; set; }
    public string? BlitzRating { get; set; }
}