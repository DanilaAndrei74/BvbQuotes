using BvbQuotes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunction.BvbQuotes
{
    public class GetSecurityQuote
    {
        private readonly ILogger<GetSecurityQuote> _logger;

        public GetSecurityQuote(ILogger<GetSecurityQuote> logger)
        {
            _logger = logger;
        }

        [Function("quote")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var symbol = req.Query["symbol"].ToString() ?? string.Empty;

            var wpd = new WebPageDownloader();
            var quote = wpd.GetQuoteForSecurity(symbol);

            return new OkObjectResult(quote);
        }
    }
    
    public record SecurityQuote(DateTime Date, double Quote);
}
