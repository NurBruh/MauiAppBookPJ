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

        // Получаем товары из корзины
        var computersInCart = CartService.CartItems;

        // Привязываем данные к CollectionView или ListView
        cartCollection.ItemsSource = computersInCart;

        // Если корзина пуста, отображаем сообщение
        if (computersInCart.Count == 0)
        {
            await DisplayAlert("Корзина пуста", "Ваша корзина пустая. Добавьте товары в корзину.", "ОК");
        }
    }

    private void OnCheckoutClicked(object sender, EventArgs e)
    {
        OnDownloadPdfClicked(sender, e);
    }


    private async void OnDownloadPdfClicked(object sender, EventArgs e)
    {
        var computers = cartCollection.ItemsSource.Cast<Computer>().ToList();

        if (computers.Count == 0)
        {
            await DisplayAlert("Внимание", "Корзина пуста.", "ОК");
            return;
        }

        if (App.CurrentUser == null)
        {
            await DisplayAlert("Ошибка", "Пользователь не авторизован.", "ОК");
            return;
        }

        PdfDocument document = new PdfDocument();
        PdfPage page = document.Pages.Add();
        page.Section.PageSettings.Orientation = PdfPageOrientation.Landscape; // Альбомная ориентация
        PdfGraphics graphics = page.Graphics;

        float y = 10;

        // 🖼 Логотип
        var logoPath = Path.Combine(FileSystem.Current.AppDataDirectory, "logo.png");
        if (File.Exists(logoPath))
        {
            using var imageStream = new FileStream(logoPath, FileMode.Open, FileAccess.Read);
            var logo = new PdfBitmap(imageStream);
            graphics.DrawImage(logo, new RectangleF(0, y, 80, 80));
            y += 90;
        }

        // 📢 Заголовок и дата
        graphics.DrawString("🖥 ComputerStore - Чек заказа",
            new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold),
            PdfBrushes.DarkBlue,
            new Syncfusion.Drawing.PointF(0, y));
        y += 30;

        graphics.DrawString($"Покупатель: {App.CurrentUser.Username}",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, y));
        y += 20;

        graphics.DrawString($"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, y));
        y += 30;

        // 🧾 Таблица
        string[] headers = { "Название", "Модель", "Тип", "CPU", "GPU", "RAM", "Storage", "Цена" };
        float[] columnWidths = { 100, 80, 60, 80, 80, 60, 80, 70 }; // ~610
        float rowHeight = 20;
        var borderPen = new PdfPen(PdfBrushes.LightGray, 0.5f);
        var headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
        var cellFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
        var headerBrush = PdfBrushes.White;
        var cellBrush = PdfBrushes.Black;

        float tableWidth = columnWidths.Sum();
        float x = 0;

        // 🟦 Заголовки
        graphics.DrawRectangle(PdfBrushes.SteelBlue, new RectangleF(x, y, tableWidth, rowHeight));
        for (int i = 0; i < headers.Length; i++)
        {
            graphics.DrawRectangle(borderPen, new RectangleF(x, y, columnWidths[i], rowHeight));
            graphics.DrawString(headers[i], headerFont, headerBrush, new RectangleF(x + 2, y + 3, columnWidths[i], rowHeight));
            x += columnWidths[i];
        }
        y += rowHeight;

        // 📄 Строки
        decimal total = 0;
        foreach (var pc in computers)
        {
            if (y > page.GetClientSize().Height - rowHeight - 40)
            {
                page = document.Pages.Add();
                page.Section.PageSettings.Orientation = PdfPageOrientation.Landscape;
                graphics = page.Graphics;
                y = 10;
            }

            x = 0;
            string[] row = {
            pc.Name,
            pc.Model,
            pc.Type,
            pc.CPU,
            pc.GPU,
            pc.RAM,
            pc.Storage,
            $"{pc.Price:C}"
        };

            for (int i = 0; i < row.Length; i++)
            {
                graphics.DrawRectangle(borderPen, new RectangleF(x, y, columnWidths[i], rowHeight));
                graphics.DrawString(row[i], cellFont, cellBrush, new RectangleF(x + 2, y + 3, columnWidths[i], rowHeight));
                x += columnWidths[i];
            }

            y += rowHeight;
            total += pc.Price;
        }

        // 💰 Итог
        y += 15;
        graphics.DrawString($"ИТОГО К ОПЛАТЕ: {total:C}",
            new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold),
            PdfBrushes.Black,
            new Syncfusion.Drawing.PointF(0, y));
        y += 30;

        graphics.DrawString("Спасибо за покупку!",
            new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Italic),
            PdfBrushes.DarkGreen,
            new Syncfusion.Drawing.PointF(0, y));

        // 💾 Сохранение
        var fileName = $"order_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        using (FileStream outputFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            document.Save(outputFile);

        document.Close(true);

        // 🗂 История заказа
        var order = new OrderHistory
        {
            UserId = App.CurrentUser.Id,
            BookTitles = string.Join("; ", computers.Select(c => c.Name)),
            PdfPath = filePath,
            CreatedAt = DateTime.Now
        };

        await Task.WhenAll(
            App.DbService.AddOrderHistoryAsync(order),
            Task.WhenAll(computers.Select(c =>
            {
                c.InCart = false;
                return App.DbService.UpdateComputerAsync(c);
            }))
        );

        CartService.CartItems.Clear();
        cartCollection.ItemsSource = null;

        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(filePath)
        });

        await Navigation.PopToRootAsync();
    }





}
