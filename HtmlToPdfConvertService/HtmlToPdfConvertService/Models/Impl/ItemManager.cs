using HtmlToPdfConvertService.Models.Items;
using Polly;
using System.Reflection.Metadata.Ecma335;

namespace HtmlToPdfConvertService.Models.Impl
{
    internal class ItemManager : IItemManager
    {
        private readonly IHtmlToPdfConverter fileCoverter;
        private readonly IItemsRepository itemsRepository;
        private readonly string fileStorageFolderPath;

        public ItemManager(
            IHtmlToPdfConverter fileCoverter,
            IItemsRepository itemsRepository,
            IConfiguration appConfiguration)
        {
            this.fileCoverter = fileCoverter;
            this.itemsRepository = itemsRepository;
            fileStorageFolderPath = appConfiguration["FileStorageFolderPath"];
            if (!Directory.Exists(fileStorageFolderPath))
            {
                Directory.CreateDirectory(fileStorageFolderPath);
            }
        }

        public string GetConvertedFile(Guid fileId)
        {
            return GetConvertedFileInternal(fileId);
        }

        public void CreateNewItem(Guid identity, string name)
        {
            var item = new ItemInfo(identity, name);
            itemsRepository.AddItem(item);
        }

        public async Task ConvertFileAsync(Guid identity, string uploadedFilePath)
        {
            await ConvertFileAsyncInternal(identity, uploadedFilePath);
        }

        public ItemInfo? GetItem(Guid id)
        {
            return itemsRepository.GetItem(id);
        }

        private string GetConvertedFileInternal(Guid fileId)
        {
            var item = itemsRepository.GetItem(fileId);
            if (item == null || string.IsNullOrEmpty(item.PdfFilePath))
            {
                throw new InvalidOperationException($"Converted file path for item {fileId} not found");
            }

            return Policy.HandleResult<string>(path => !File.Exists(path))
                .WaitAndRetry(5, i => TimeSpan.FromSeconds(5))
                .Execute(() => item.PdfFilePath);
            
        }

        private string GetFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private async Task ConvertFileAsyncInternal(Guid identity, string fileToConvertPath)
        {
            var item = itemsRepository.GetItem(identity);
            if(item == null)
            {
                throw new InvalidOperationException($"Cannot find item {identity}");
            }
            var fileContentToConvert = GetFileContent(fileToConvertPath);
            item.HtmlContent = fileContentToConvert;
            itemsRepository.UpdateItem(item);
            var newFilePath = GetNewFilePath(fileToConvertPath);
            await fileCoverter.ConvertAsync(item.HtmlContent, newFilePath)
                .ContinueWith(task =>
                {  
                    item.PdfFilePath = newFilePath;
                    itemsRepository.UpdateItem(item);
                });
        }

        private string GetNewFilePath(string oldFilePath)
        {
            var oldFileName = Path.GetFileName(oldFilePath); 
            var newFileName = Path.ChangeExtension(oldFileName, ".pdf");
            return Path.Combine(fileStorageFolderPath, newFileName);
        }

    }
}
