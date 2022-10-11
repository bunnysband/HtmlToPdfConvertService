namespace HtmlToPdfConvertService.Models
{
    public interface IHtmlToPdfConverter
    {
        Task ConvertAsync(string htmlContent, string outputFile);
    }
}
