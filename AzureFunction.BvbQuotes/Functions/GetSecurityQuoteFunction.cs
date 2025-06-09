using AzureFunction.BvbQuotes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunction.BvbQuotes.Functions
{
    public class GetSecurityQuoteFunction
    {
        private readonly ILogger<GetSecurityQuoteFunction> _logger;
        private readonly WebPageDownloader _webPageDownloader;

        public GetSecurityQuoteFunction(ILogger<GetSecurityQuoteFunction> logger, WebPageDownloader webPageDownloader)
        {
            _logger = logger;
            _webPageDownloader = webPageDownloader;
        }

        [Function("quote")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var symbol = req.Query["symbol"].ToString() ?? string.Empty;
            var quote = _webPageDownloader.GetQuoteForSecurity(symbol);

            return new OkObjectResult(quote);
        }
    }
}
