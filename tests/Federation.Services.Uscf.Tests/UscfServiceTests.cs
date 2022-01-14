using System.Diagnostics.CodeAnalysis;
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
        PlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.NotNull(profile);
    }

    [Theory]
    [InlineData("13607491")]
    [InlineData("20021879")]
    [InlineData("30214499")]
    public async Task UscfIdShouldMatch(string uscfId)
    {
        PlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.Equal(uscfId, profile?.UscfId);
    }

    [Theory]
    [InlineData("13607491", "MARSHAL HAYES")]
    [InlineData("20021879", "JOEL HANKS")]
    [InlineData("30214499", "MR. COREY BOSTICK")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task NameShouldMatch(string uscfId, string name)
    {
        PlayerProfile? profile = await UscfService.GetPlayerProfileAsync(uscfId);

        Assert.Equal(name, profile?.PlayerName);
    }
}