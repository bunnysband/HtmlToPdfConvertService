# HtmlToPdfConvertService

Simple web service on ASP.NET MVC. Used MS SQL to store items status, used PuppeteerSharp nuget to convert html to pdf.

TODO:
- make unit tests
- add logging
- add errors processing
- make a way to clean data (file storage, database)
- separate ItemInfo model from ItemInfo dto to avoid database changes
- abstract file storage to be able store files eslewhere filesystem 
