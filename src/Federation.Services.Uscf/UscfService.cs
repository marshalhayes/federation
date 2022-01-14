using System.Reflection;
using System.Web;
using Federation.Services.Core;
using Federation.Services.Uscf.Models;

namespace Federation.Services.Uscf;

public class UscfService
{
    private readonly HttpClient httpClient;

    private static readonly string PlayerProfileSchema =
        File.ReadAllText(Path.Join(Assembly.GetExecutingAssembly().Location, "..", "JsonConfigs/PlayerProfile.json"));

    public UscfService()
    {
        httpClient = new HttpClient();

        var currentAssembly = Assembly.GetExecutingAssembly();

        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            $".NET {currentAssembly.ImageRuntimeVersion}; {nameof(Federation)}.{nameof(Services)}.{nameof(Uscf)}");
    }

    public async ValueTask<PlayerProfile?> GetPlayerProfileAsync(string uscfId,
        CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response =
            await httpClient.GetAsync($"https://www.uschess.org/msa/MbrDtlMain.php?{HttpUtility.UrlEncode(uscfId)}",
                cancellationToken);

        if (!response.IsSuccessStatusCode) return default;

        HtmlParser<PlayerProfile> parser = new(PlayerProfileSchema);

        return parser.Parse(await response.Content.ReadAsStreamAsync(cancellationToken));
    }
}