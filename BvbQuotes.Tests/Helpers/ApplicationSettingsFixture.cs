using BvbQuotes.Functions.Configuration;

namespace BvbQuotes.Tests.Helpers;

public class ApplicationSettingsFixture
{
    public ApplicationSettingsFixture()
    {
        // Set the static properties before tests run
        ApplicationSettings.WebConfiguration = new WebConfiguration
        {
            BvbUrl = "https://bvb.ro/FinancialInstruments/Details/FinancialInstrumentsDetails.aspx",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
            Culture = "ro-RO"
        };

        ApplicationSettings.BusinessLogicConfiguration = new BusinessLogicConfiguration
        {
            QuoteCss = "//b[@class='value']",
            DateCss = "//span[@class='date small']"
        };
    }
}

[CollectionDefinition("AppSettings collection")]
public class AppSettingsCollection : ICollectionFixture<ApplicationSettingsFixture>{}