using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using TrackEasy.Pdf.Abstractions;
using TrackEasy.Pdf.Abstractions.Models;
using TrackEasy.Pdf.Templates;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Pdf;

internal sealed class PdfService(RazorRenderer razorRenderer, IConfiguration configuration) : IPdfService
{
    private readonly Lazy<Task<Browser>> _browser = new(async () =>
    {
        var execPath = configuration["chrome-path"] ?? "/usr/bin/chromium-browser";

        return (Browser)await Puppeteer.LaunchAsync(new LaunchOptions
        {
            ExecutablePath = execPath,
            Headless = true,
            Args = ["--no-sandbox", "--disable-dev-shm-usage"]
        });
    });

    public async Task<byte[]> GeneratePdfAsync(string htmlContent, CancellationToken cancellationToken)
    {
        var browser = await _browser.Value;

        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent, new NavigationOptions
        {
            WaitUntil = [WaitUntilNavigation.Networkidle0]
        });
        await page.EvaluateExpressionHandleAsync("document.fonts.ready");

        return await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            MarginOptions = new MarginOptions { Top = "20mm", Bottom = "20mm" }
        });
    }

    public async Task<byte[]> GenerateTicketPdfAsync(TicketModel model, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<TicketPdf>(parameters);
        return await GeneratePdfAsync(html, cancellationToken);
    }
}