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
            if (string.IsNullOrEmpty(symbol)) return new BadRequestObjectResult("Please pass a symbol on the query string");
            
            var quote = _webPageDownloader.GetQuoteForSecurity(symbol);
            if(quote is null) return new UnprocessableEntityObjectResult("Request could not be processed");

            return new OkObjectResult(quote);
        }
    }
}
