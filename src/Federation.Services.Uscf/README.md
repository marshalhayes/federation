# Federation.Services.Uscf

Functionality specific to web scraping data from [uschess.org](https://uschess.org)

### Overview

This library includes a `UscfService` class that can be used to get everything from a player's profile information to
their tournament history.

There are three main methods on `UscfService`:

- `ValueTask<UscfPlayerProfile?> GetPlayerProfileAsync(string uscfId)`
- `IAsyncEnumerable<UscfPlayerTournament> GetPlayerTournamentsAsync(string uscfId)`
- `ValueTask<UscfTournamentDetails?> GetTournamentAsync(string eventId)`

To get started, create an instance of the `UscfService`. If you're using dependency injection, you can add this to as a
singleton.

```c#
builder.Services.AddSingleton<UscfService>();
```

Next, use it how you'd like. For example, a minimal API for getting a player's profile information by their `uscfId`:

```c#
app.MapGet("/players/{uscfId}", async (string uscfId, [FromServices] UscfService uscfService) => {
    UscfPlayerProfile? profile = await uscfService.GetPlayerProfileAsync(uscfId);
    
    return profile is null ? Results.NotFound() : Results.Ok(profile);
});
```