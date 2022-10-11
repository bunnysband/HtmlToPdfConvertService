using HtmlToPdfConvertService.Models.Items;

namespace HtmlToPdfConvertService.Models
{
    public interface IItemsRepository
    {
        public IReadOnlyCollection<ItemInfo> Items { get; }

        public ItemInfo? GetItem(Guid itemId);
        public void DeleteItem(Guid itemId);

        public void UpdateItem(ItemInfo item);
        public void AddItem(ItemInfo item);
    }
}
