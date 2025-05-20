using System.Text;
using HtmlAgilityPack;

namespace BvbQuotes.Services;

public class WebPageDownloader
{
    public async Task<string> GetQuoteForSecurity(string security)
    {
        var htmlDoc = await DownloadHtmlForSecurity(security);
        return SelectQuoteForSecurity(htmlDoc);
    }
    
    public async Task<string> DownloadHtmlForSecurity(string security)
    {
        var url = GetFullUrl(security);
        
        
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
            "(KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");

        try
        {
            return await client.GetStringAsync(url);;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching page: {ex.Message}");
        }
        return string.Empty;
    }
    
    public string GetFullUrl(string security)
    {
        var uri = new Uri(BASE_URL);
        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Query = $"s={security}";
        
        return uriBuilder.Uri.ToString();
    }

    private string SelectQuoteForSecurity(string? htmlDocument)
    {
        // Load HTML into parser
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlDocument);

        // Select the <b> tag with class 'value'
        var valueNode = htmlDoc.DocumentNode.SelectSingleNode("//b[@class='value']");

        if (valueNode != null)
        {
            Console.WriteLine(valueNode.InnerText);
        }
        else
        {
            Console.WriteLine("Element not found.");
        }

        return valueNode.InnerText;
    }

    public const string BASE_URL = "https://bvb.ro/FinancialInstruments/Details/FinancialInstrumentsDetails.aspx";
}