using System.Globalization;
using System.Text;
using HtmlAgilityPack;

namespace BvbQuotes.Services;

public class WebPageDownloader
{
    public double GetQuoteForSecurity(string security)
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

    private double SelectQuoteForSecurity(string htmlDocument)
    {
        // Load HTML into parser
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlDocument);

        var valueNode = htmlDoc.DocumentNode.SelectSingleNode("//b[@class='value']");

        return Double.Parse(valueNode?.InnerText, new CultureInfo("ro-RO"));
    }

    public const string BASE_URL = "https://bvb.ro/FinancialInstruments/Details/FinancialInstrumentsDetails.aspx";
}