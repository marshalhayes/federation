using System.Net;
using Federation.Services.Uscf;
using Federation.Services.Uscf.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<UscfService>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapGet("/api/federations/uscf/{playerId}",
    async (string playerId, UscfService uscfService, CancellationToken cancellationToken, ILogger<Program> logger) =>
    {
        try
        {
            UscfPlayerProfile? player = await uscfService.GetPlayerProfileAsync(playerId, cancellationToken);

            return player is null ? Results.NotFound() : Results.Ok(player);
        }
        catch (Exception exception)
        {
            logger.LogError("An exception of type {} was thrown while attempting to retrieve player with id={}",
                exception.GetType(), playerId);

            switch (exception)
            {
                case TimeoutException:
                    return Results.Problem(statusCode: (int) HttpStatusCode.GatewayTimeout);
                case OperationCanceledException:
                    return Results.BadRequest("Client disconnected");
            }
        }

        return Results.Problem(statusCode: (int) HttpStatusCode.GatewayTimeout);
    }
);

await app.RunAsync();