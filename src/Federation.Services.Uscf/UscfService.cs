using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using Federation.Services.Core;
using Federation.Services.Uscf.Models;
using Newtonsoft.Json.Linq;
using OpenScraping;
using OpenScraping.Config;

namespace Federation.Services.Uscf;

public class UscfService
{
    private readonly HttpClient httpClient;

    private static readonly string PlayerProfileSchema =
        File.ReadAllText(Path.Join(Assembly.GetExecutingAssembly().Location, "..",
            $"JsonConfigs{Path.DirectorySeparatorChar}PlayerProfile.json"));

    private static readonly string TournamentSchema =
        File.ReadAllText(Path.Join(Assembly.GetExecutingAssembly().Location, "..",
            $"JsonConfigs{Path.DirectorySeparatorChar}PlayerTournament.json"));

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

    public async IAsyncEnumerable<UscfPlayerTournament> GetPlayerTournamentsAsync(string uscfId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response =
            await httpClient.GetAsync(
                $"https://www.uschess.org/msa/MbrDtlTnmtHst.php?{HttpUtility.UrlEncode(uscfId)}.0",
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            yield break;
        }

        ConfigSection config = StructuredDataConfig.ParseJsonString(TournamentSchema);
        StructuredDataExtractor extractor = new(config);

        JContainer container = extractor.Extract(await response.Content.ReadAsStringAsync());
        if (container.HasValues)
        {
            PropertyInfo[] writeableProperties =
                typeof(UscfPlayerTournament).GetProperties().Where(t => t.CanWrite).ToArray();

            int maxIterations = container.Values().Max(t => t.Count());

            // Since there are values, there must be at least one...
            if (maxIterations <= 0)
            {
                var obj = new UscfPlayerTournament();

                foreach (PropertyInfo property in writeableProperties)
                {
                    JToken? propertyToken = container.SelectToken(property.Name);
                    object? propertyValue = propertyToken?.ToObject(property.PropertyType);

                    property.SetValue(obj, propertyValue);
                }

                yield return obj;
                yield break;
            }

            for (int i = 0; i < maxIterations; i++)
            {
                var obj = new UscfPlayerTournament();

                foreach (PropertyInfo property in writeableProperties)
                {
                    JToken? propertyToken = container.SelectToken($"{property.Name}[{i}]");
                    object? propertyValue = propertyToken?.ToObject(property.PropertyType);

                    property.SetValue(obj, propertyValue);
                }

                yield return obj;
            }
        }
    }
}