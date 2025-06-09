using System.ComponentModel.DataAnnotations;

namespace BvbQuotes.Functions.Configuration;

public class ApplicationSettings
{
    [Required]
    public static WebConfiguration WebConfiguration { get; set; }
    
    [Required]
    public static BusinessLogicConfiguration BusinessLogicConfiguration { get; set; }
}

public class WebConfiguration
{
    [Required]
    public string BvbUrl { get; set; }
    [Required]
    public string UserAgent { get; set; }

    public string Culture { get; set; } = "ro-RO";
}

public class BusinessLogicConfiguration
{
    [Required]
    public string QuoteCss { get; set; }
    
    [Required]
    public string DateCss { get; set; }
}