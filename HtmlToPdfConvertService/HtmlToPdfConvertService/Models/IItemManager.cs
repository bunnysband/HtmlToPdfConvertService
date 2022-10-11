using HtmlToPdfConvertService.Models.Items;

namespace HtmlToPdfConvertService.Models
{
    public interface IItemManager
    {
        string ConvertFile(Guid identity, string filePath);
        string GetConvertedFile(Guid fileId);

        public void CreateNewItem(Guid identity, string name);

        ItemInfo? GetItem(Guid idebtity);
    }
}
