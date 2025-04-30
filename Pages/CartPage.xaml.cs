using MauiAppBookPJ.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;

namespace MauiAppBookPJ.Pages;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var books = await App.DbService.GetBooksAsync();
        var userCart = books
            .Where(b => b.InCart && b.UserId == App.CurrentUser?.Id)
            .ToList();

        cartCollection.ItemsSource = userCart;
    }

    private void OnCheckoutClicked(object sender, EventArgs e)
    {
        OnDownloadPdfClicked(sender, e);
    }

    private async void OnDownloadPdfClicked(object sender, EventArgs e)
    {
        var books = cartCollection.ItemsSource.Cast<Book>().ToList();

        if (books.Count == 0)
        {
            await DisplayAlert("Внимание", "Корзина пуста.", "ОК");
            return;
        }

        PdfDocument document = new PdfDocument();
        PdfPage page = document.Pages.Add();
        PdfGraphics graphics = page.Graphics;

        float y = 10;

        var logoPath = Path.Combine(FileSystem.Current.AppDataDirectory, "logo.png");
        if (File.Exists(logoPath))
        {
            using FileStream imageStream = new FileStream(logoPath, FileMode.Open, FileAccess.Read);
            PdfBitmap logo = new PdfBitmap(imageStream);
            graphics.DrawImage(logo, new RectangleF(0, y, 80, 80));
            y += 90;
        }

        graphics.DrawString("📚 BookStore - Чек заказа",
            new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold),
            PdfBrushes.DarkBlue,
            new Syncfusion.Drawing.PointF(0, y));
        y += 30;

        graphics.DrawString($"Пользователь: {App.CurrentUser?.Username}",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, y));
        y += 20;

        graphics.DrawString($"Дата: {DateTime.Now:dd.MM.yyyy}",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, y));
        y += 30;

        foreach (var book in books)
        {
            graphics.DrawString("📖 " + book.Title + " — " + book.Author,
                new PdfStandardFont(PdfFontFamily.Helvetica, 12),
                PdfBrushes.Black,
                new Syncfusion.Drawing.PointF(0, y));
            y += 20;
        }

        y += 20;
        graphics.DrawString("Спасибо за заказ!",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Italic),
            PdfBrushes.DarkGreen,
            new Syncfusion.Drawing.PointF(0, y));

        var fileName = $"order_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        using (FileStream outputFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            document.Save(outputFile);

        document.Close(true);

        var order = new OrderHistory
        {
            UserId = App.CurrentUser.Id,
            BookTitles = string.Join("; ", books.Select(b => b.Title)),
            PdfPath = filePath,
            CreatedAt = DateTime.Now
        };

        await App.DbService.AddOrderHistoryAsync(order);

        foreach (var book in books)
        {
            book.InCart = false;
            await App.DbService.UpdateBookAsync(book);
        }

        cartCollection.ItemsSource = new List<Book>();

        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(filePath)
        });
    }

}
