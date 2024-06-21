using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

QuestPDF.Settings.License = LicenseType.Community;

var articles = new ArticleData().GetArticles();

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(16));

        page.Header()
            .Text("Hello PDF!")
            .SemiBold().FontSize(40).FontColor(Colors.Red.Medium);

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(x =>
            {
                x.Spacing(20);

                x.Item().Text(Placeholders.LoremIpsum());
                x.Item().Image(Placeholders.Image(200, 70));

                x.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);
                        columns.RelativeColumn(2);
                        columns.ConstantColumn(60);
                        columns.RelativeColumn(1);
                    });

                    // by using custom 'Element' method, we can reuse visual configuration
                    table.Cell().Row(1).Column(1).Element(Block).Text("ID").SemiBold();
                    table.Cell().Row(1).Column(2).Element(Block).AlignLeft().PaddingLeft(6).Text("Product Name").SemiBold();
                    table.Cell().Row(1).Column(3).Element(Block).Text("Stock").SemiBold();
                    table.Cell().Row(1).Column(4).Element(Block).Text("Price").SemiBold();

                    uint rowIndex = 2;
                    foreach (var article in articles)
                    {
                        table.Cell().Row(rowIndex).Column(1).Element(Entry).Text(article.ArticleId.ToString());
                        table.Cell().Row(rowIndex).Column(2).Element(Entry).AlignLeft().Text(article.ProductName);
                        table.Cell().Row(rowIndex).Column(3).Element(Entry).Text(article.Stock.ToString());
                        table.Cell().Row(rowIndex).Column(4).Element(Entry).AlignRight().Text(article.Price.ToString("C"));

                        rowIndex++;
                    }

                    // for simplicity, you can also use extension method described in the "Extending DSL" section
                    static IContainer Block(IContainer container)
                    {
                        return container
                            .Border(1)
                            .Background(Colors.Grey.Lighten3)
                            .ShowOnce()
                            .MinWidth(50)
                            .MinHeight(20)
                            .AlignCenter()
                            .AlignMiddle();
                    }

                    static IContainer Entry(IContainer container)
                    {
                        return container
                            .Border(1)
                            .PaddingTop(1)
                            .PaddingBottom(1)
                            .PaddingLeft(6)
                            .PaddingRight(6)
                            .ShowOnce()
                            .AlignCenter()
                            .AlignMiddle();
                    }
                });
            });

        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
            });
    });
})
.ShowInPreviewer();

public class ArticleData
{
    public IEnumerable<Article> GetArticles()
    {
        yield return new Article(1, "iPhone 14", 3, 999m);
        yield return new Article(2, "Samsung Galaxy S23", 5, 799m);
        yield return new Article(3, "Tesla Model 3", 2, 39999m);
        yield return new Article(4, "Dell XPS 13 Laptop", 7, 1299m);
        yield return new Article(5, "Sony WH-1000XM4 Headphones", 4, 349m);
        yield return new Article(6, "Apple Watch Series 8", 6, 429m);
        yield return new Article(7, "Bose SoundLink Revolve+", 8, 299m);
        yield return new Article(8, "Nikon D850 Camera", 1, 2999m);
        yield return new Article(9, "PlayStation 5", 3, 499m);
        yield return new Article(10, "Microsoft Surface Pro 8", 2, 1099m);

    }
}

public record Article(int ArticleId, string ProductName, int Stock, decimal Price);