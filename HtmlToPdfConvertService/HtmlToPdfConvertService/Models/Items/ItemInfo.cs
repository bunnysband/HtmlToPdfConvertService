namespace HtmlToPdfConvertService.Models.Items
{
    public class ItemInfo
    {
        public ItemInfo (Guid guidIdentity, string name)
        {
            GuidIdentity = guidIdentity;
            Name = name;
            DateCreated = DateTime.Now;
        }

        public int Id { get; set; }
        public Guid GuidIdentity { get; set; }

        public DateTime DateCreated { get; }
        public string Name { get; set; }
        public string? HtmlContent { get; set; }
        public string? PdfFilePath { get; set; }

    }
}
