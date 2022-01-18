using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Federation.Services.Uscf.Models;
using Xunit;

namespace Federation.Services.Uscf.Tests;

public class UscfServiceTests
{
    private static readonly UscfService UscfService = new();

    [Theory]
    [InlineData("13607491")]
    [InlineData("20021879")]
    [InlineData("30214499")]
    public async Task ShouldHaveProfile(string uscfId)
    {
        UscfPlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.NotNull(profile);
    }

    [Theory]
    [InlineData("13607491")]
    [InlineData("20021879")]
    [InlineData("30214499")]
    public async Task PlayerIdShouldMatch(string uscfId)
    {
        UscfPlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.Equal(uscfId, profile?.PlayerId);
    }

    [Theory]
    [InlineData("13607491", "MARSHAL HAYES")]
    [InlineData("20021879", "JOEL HANKS")]
    [InlineData("30214499", "MR. COREY BOSTICK")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task NameShouldMatch(string uscfId, string name)
    {
        UscfPlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.Equal(name, profile?.PlayerName);
    }

    [Theory]
    [InlineData("13607491")]
    [InlineData("20021879")]
    [InlineData("30214499")]
    public async Task RatingsShouldHaveSomeValue(string uscfId)
    {
        UscfPlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.True(!string.IsNullOrWhiteSpace(profile?.RegularRating),
            $"{nameof(UscfPlayerProfile.RegularRating)} should have some value");

        Assert.True(!string.IsNullOrWhiteSpace(profile?.QuickRating),
            $"{nameof(UscfPlayerProfile.QuickRating)} should have some value");

        Assert.True(!string.IsNullOrWhiteSpace(profile?.BlitzRating),
            $"{nameof(UscfPlayerProfile.BlitzRating)} should have some value");
    }

    [Theory]
    [InlineData("13607491")]
    public async Task GenderShouldHaveSomeValue(string uscfId)
    {
        UscfPlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.True(!string.IsNullOrWhiteSpace(profile?.Gender),
            $"{nameof(UscfPlayerProfile.Gender)} should have some value");
    }

    [Theory]
    [InlineData("13607491")]
    public async Task ShouldHaveSomeTournaments(string uscfId)
    {
        IAsyncEnumerable<UscfPlayerTournament> tournaments = UscfService.GetPlayerTournamentsAsync(uscfId);

        List<UscfPlayerTournament> results = await tournaments.ToListAsync();

        Assert.NotEmpty(results);
    }
}