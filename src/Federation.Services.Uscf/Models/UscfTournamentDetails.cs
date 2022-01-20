namespace Federation.Services.Uscf.Models;

public class UscfTournamentDetails
{
#nullable disable
    public string EventId { get; set; }

    public string EventName { get; set; }

    public string Location { get; set; }

    public string SponsoringAffiliateId { get; set; }

#nullable enable

    public DateTime ProcessedOn { get; set; }

    public DateTime EnteredOn { get; set; }

    public DateTime RatedOn { get; set; }

    public string? OrganizerId { get; set; }

    public int TotalPlayers { get; set; }
}