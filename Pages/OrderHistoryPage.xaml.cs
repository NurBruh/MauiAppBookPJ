using MauiAppBookPJ.Models;
using System.Windows.Input;

namespace MauiAppBookPJ.Pages;

public partial class OrderHistoryPage : ContentPage
{
    public ICommand OpenPdfCommand { get; }

    public OrderHistoryPage()
    {
        InitializeComponent();
        BindingContext = this;

        OpenPdfCommand = new Command<string>(async (pdfPath) =>
        {
            if (File.Exists(pdfPath))
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(pdfPath)
                });
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл PDF не найден. Возможно, он был удалён.", "ОК");
            }
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (App.CurrentUser != null)
        {
            var orders = await App.DbService.GetOrderHistoryByUserAsync(App.CurrentUser.Id);
            orderHistoryCollection.ItemsSource = orders;
        }
    }
}
