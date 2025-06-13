using BvbQuotes.Functions.Configuration;
using BvbQuotes.Functions.Services;

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
    public void GetQuoteForSecurity_Should_Return_Result()
    {
        var result = _sut.GetQuoteForSecurity("TLV");
        Assert.NotNull(result);
    }
    
    [Fact]
    public void GetFullUrl_Should_Return_Result()
    {
        var testString = "TEST";
        var url = _sut.GetFullUrl(testString);
        var expected = $"{ApplicationSettings.WebConfiguration.BvbUrl}?s={testString}" ;
        
        Assert.Equal(expected, url);
    }

    [Fact]
    public void DownloadHtmlForSecurity_Should_Return_Result()
    {
        var result = _sut.DownloadHtmlForSecurity("TLV");
        Assert.NotNull(result);
    }
    
}