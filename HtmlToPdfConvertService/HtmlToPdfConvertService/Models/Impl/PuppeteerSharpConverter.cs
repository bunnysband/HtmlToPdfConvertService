using PuppeteerSharp;

namespace HtmlToPdfConvertService.Models.Impl
{
    internal class PuppeteerSharpConverter : IHtmlToPdfConverter
    {
        public async Task ConvertAsync(string htmlContent, string outputFile)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException(nameof(htmlContent));
            }
            if (string.IsNullOrEmpty(outputFile))
            {
                throw new ArgumentException("Output file cannot be null or empty");
            }
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(
                new LaunchOptions
                {
                    Headless = true
                });
            using var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlContent);
            var result = await page.GetContentAsync();
            await page.PdfAsync(outputFile);
        }
    }
}
