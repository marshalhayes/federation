using Federation.Services.Uscf;
using Federation.Services.Uscf.Models;
using Microsoft.AspNetCore.Mvc;

namespace Federation.App.Controllers;

[Route("/api/[controller]/{federation}")]
[ApiController]
public class FederationsController : ControllerBase
{
    private readonly UscfService uscfService;

    public FederationsController(UscfService uscfService)
    {
        this.uscfService = uscfService;
    }

    [HttpGet("{playerId}")]
    public async ValueTask<IActionResult> GetPlayerById(string federation, string playerId,
        CancellationToken cancellationToken)
    {
        UscfPlayerProfile? player = await uscfService.GetPlayerProfileAsync(playerId, cancellationToken);

        return player is null ? NotFound() : Ok(player);
    }

    [HttpGet("{playerId}/tournaments")]
    public async ValueTask<IActionResult> GetPlayerTournaments(string federation, string playerId,
        int offset = 0, int limit = 10, CancellationToken cancellationToken = default)
    {
        List<UscfPlayerTournament> tournaments = await uscfService
            .GetPlayerTournamentsAsync(playerId, cancellationToken).ToListAsync(cancellationToken);

        return Ok(new
        {
            Total = tournaments.Count,
            Results = tournaments.Skip(offset).Take(limit)
        });
    }
}