using System.Globalization;
using System.Text;
using AzureFunction.BvbQuotes;
using HtmlAgilityPack;

namespace BvbQuotes.Services;

public class WebPageDownloader
{
    public SecurityQuote GetQuoteForSecurity(string security)
    {
        var htmlDoc = DownloadHtmlForSecurity(security);
        return SelectQuoteForSecurity(htmlDoc);
    }
    
    public string DownloadHtmlForSecurity(string security)
    {
        var url = GetFullUrl(security);
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
            "(KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");


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
        var uri = new Uri(BASE_URL);
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Query = $"s={security}";
        
        return uriBuilder.Uri.ToString();
    }

    private SecurityQuote SelectQuoteForSecurity(string htmlDocument)
    {
        // Load HTML into parser
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlDocument);

        var quoteNote = htmlDoc.DocumentNode.SelectSingleNode("//b[@class='value']");
        var dateNote = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='date small']");

        var securityQuote = new SecurityQuote(
            DateTime.Parse(dateNote?.InnerText, new CultureInfo("ro-RO")),
            Double.Parse(quoteNote?.InnerText, new CultureInfo("ro-RO")));

        return securityQuote;
    }

    public const string BASE_URL = "https://bvb.ro/FinancialInstruments/Details/FinancialInstrumentsDetails.aspx";
}