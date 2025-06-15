using BvbQuotes.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BvbQuotes.Functions.Functions
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
            var symbol = req.Query["symbol"].ToString();
            _logger.LogInformation($"Getting information for {symbol}");
            
            if (string.IsNullOrEmpty(symbol)) return new BadRequestObjectResult("Please pass a symbol on the query string");
            
            var quote = _webPageDownloader.GetQuoteForSecurity(symbol);
            if (quote is null)
            {
                _logger.LogError($"There was a problem while getting information for {symbol}");
                return new UnprocessableEntityObjectResult("Request could not be processed");
            }

            _logger.LogInformation($"Returning information for {symbol}: quoted at {quote.Quote} at the time of {quote.Date}");
            return new OkObjectResult(quote);
        }
    }
}
