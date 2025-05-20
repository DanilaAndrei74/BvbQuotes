using BvbQuotes.Services;

namespace BvbQuotes;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddOpenApi();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        

        app.MapGet("/security/{symbol}", (string symbol) =>
            {
                var wpd = new WebPageDownloader();
                return wpd.GetQuoteForSecurity(symbol);
            })
            .WithName("GetFinancialInstrument");

        app.Run();
    }
}