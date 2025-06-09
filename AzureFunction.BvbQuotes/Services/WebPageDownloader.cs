using System.Globalization;
using AzureFunction.BvbQuotes.Configuration;
using AzureFunction.BvbQuotes.Functions;
using HtmlAgilityPack;

namespace AzureFunction.BvbQuotes.Services;

public class WebPageDownloader
{
    public SecurityQuote? GetQuoteForSecurity(string security)
    {
        var htmlDoc = DownloadHtmlForSecurity(security);
        return SelectQuoteForSecurity(htmlDoc);
    }
    
    public string DownloadHtmlForSecurity(string security)
    {
        var url = GetFullUrl(security);
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", ApplicationSettings.WebConfiguration.UserAgent);


        var result = string.Empty;
        try
        {
            var response = client.GetAsync(url).Result;
            result = response.Content.ReadAsStringAsync().Result;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching page: {ex.Message}");
        }
        return result;
    }
    
    public string GetFullUrl(string security)
    {
        var uri = new Uri(ApplicationSettings.WebConfiguration.BvbUrl);
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Query = $"s={security}";
        
        return uriBuilder.Uri.ToString();
    }

    private SecurityQuote? SelectQuoteForSecurity(string htmlDocument)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlDocument);

        var quoteNote = htmlDoc.DocumentNode.SelectSingleNode(ApplicationSettings.BusinessLogicConfiguration.QuoteCss);
        var dateNote = htmlDoc.DocumentNode.SelectSingleNode(ApplicationSettings.BusinessLogicConfiguration.DateCss);

        var culture = new CultureInfo(ApplicationSettings.WebConfiguration.Culture);

        var securityQuote = 
            DateTime.TryParse(dateNote?.InnerText, culture, DateTimeStyles.None, out var date) &&
            double.TryParse(quoteNote?.InnerText, NumberStyles.Any, culture, out var quote)
                ? new SecurityQuote(date, quote)
                : null;

        return securityQuote;
    }
    
}