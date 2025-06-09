using BvbQuotes.Functions.Configuration;
using BvbQuotes.Functions.Services;
using BvbQuotes.Tests.Helpers;

namespace BvbQuotes.Tests.Services;

[Collection("AppSettings collection")]
public class WebPageDownloaderTests
{
    private readonly WebPageDownloader _sut;
    
    public WebPageDownloaderTests()
    {
        _sut = new WebPageDownloader();
    }

    [Fact]
    public void Wpd_Should_Return_Result()
    {
        var result = _sut.GetQuoteForSecurity("TLV");
        Assert.NotNull(result);
    }
    
    // [Fact]
    // public void Wpd_GetFullUrl()
    // {
    //     var result = _sut.GetFullUrl();
    //     Assert.NotNull(result);
    // }
}