using HtmlToPdfConvertService.Models.Items;

namespace HtmlToPdfConvertService.Models.Impl
{
    public class ItemsDbRepository : IItemsRepository
    {
        private readonly ItemsDbContext itemsDbContext;

        public ItemsDbRepository(ItemsDbContext itemsDbContext)
        {
            this.itemsDbContext = itemsDbContext;
        }

        public IReadOnlyCollection<ItemInfo> Items => itemsDbContext.ItemsRepository.ToHashSet();

        public void AddItem(ItemInfo item)
        {
            itemsDbContext.ItemsRepository.Add(item);
            itemsDbContext.SaveChanges();
        }

        public void DeleteItem(Guid itemId)
        {
            var itemToDelete = itemsDbContext.ItemsRepository.SingleOrDefault(item => item.GuidIdentity == itemId);
            if(itemToDelete != null)
            {
                itemsDbContext.ItemsRepository.Remove(itemToDelete);
                itemsDbContext.SaveChanges();
            }
        }

        public ItemInfo? GetItem(Guid itemId)
        {
            return itemsDbContext.ItemsRepository.SingleOrDefault(item => item.GuidIdentity == itemId);
        }

        public void UpdateItem(ItemInfo uddatedItem)
        {
            var itemToDelete = itemsDbContext.ItemsRepository.SingleOrDefault(item => item.GuidIdentity == uddatedItem.GuidIdentity);
            if (itemToDelete == null)
            {
                throw new InvalidOperationException($"Item {uddatedItem.Id} not found");
            }
            itemsDbContext.ItemsRepository.Update(itemToDelete);
            itemsDbContext.SaveChanges();
        }
    }
}
