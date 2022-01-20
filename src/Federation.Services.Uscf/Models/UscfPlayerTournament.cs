namespace Federation.Services.Uscf.Models;

public class UscfPlayerTournament
{
#nullable disable
    public string EventId { get; set; }

    public string EventName { get; set; }

    public string SectionName { get; set; }
#nullable enable

    public DateTime EndDate { get; set; }
}