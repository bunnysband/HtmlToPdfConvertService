using Microsoft.EntityFrameworkCore;

namespace HtmlToPdfConvertService.Models.Items
{
    public class ItemsDbContext : DbContext
    {
        public DbSet<ItemInfo> ItemsRepository { get; set; } = null!;
        public ItemsDbContext(DbContextOptions<ItemsDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
