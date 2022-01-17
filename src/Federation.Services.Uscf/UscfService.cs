using System.Reflection;
using System.Web;
using Federation.Services.Core;
using Federation.Services.Uscf.Models;

namespace Federation.Services.Uscf;

public class UscfService
{
    private readonly HttpClient httpClient;

    private static readonly string PlayerProfileSchema =
        File.ReadAllText(Path.Join(Assembly.GetExecutingAssembly().Location, "..",
            $"JsonConfigs{Path.DirectorySeparatorChar}PlayerProfile.json"));

    private static readonly HtmlParser<UscfPlayerProfile> ProfileParser = new(PlayerProfileSchema);

    public UscfService()
    {
        httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        var currentAssembly = Assembly.GetExecutingAssembly();

        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            $".NET {currentAssembly.ImageRuntimeVersion}; {currentAssembly.GetName()}");
    }

    public async ValueTask<UscfPlayerProfile?> GetPlayerProfileAsync(string uscfId,
        CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response =
            await httpClient.GetAsync($"https://www.uschess.org/msa/MbrDtlMain.php?{HttpUtility.UrlEncode(uscfId)}",
                cancellationToken);

        return !response.IsSuccessStatusCode
            ? default
            : ProfileParser.Parse(await response.Content.ReadAsStreamAsync());
    }
}