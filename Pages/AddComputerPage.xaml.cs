using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class AddComputerPage : ContentPage
{
    private string selectedImage;

    public AddComputerPage()
    {
        InitializeComponent();
    }

    private void OnImageTapped(object sender, TappedEventArgs e)
    {
        if (e?.Parameter is string image)
        {
            selectedImage = image;
            imagePreview.Source = selectedImage;
            imagePreview.IsVisible = true;
        }
    }

    private async void OnSaveComputerClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(computerNameEntry.Text) ||
            string.IsNullOrWhiteSpace(computerTypeEntry.Text) ||
            string.IsNullOrWhiteSpace(computerDescriptionEditor.Text) ||
            string.IsNullOrWhiteSpace(modelEntry.Text) ||
            string.IsNullOrWhiteSpace(cpuEntry.Text) ||
            string.IsNullOrWhiteSpace(gpuEntry.Text) ||
            string.IsNullOrWhiteSpace(ramEntry.Text) ||
            string.IsNullOrWhiteSpace(storageEntry.Text) ||
            !decimal.TryParse(priceEntry.Text, out var price) ||
            string.IsNullOrEmpty(selectedImage))
        {
            await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля и выберите изображение.", "OK");
            return;
        }

        var newComputer = new Computer
        {
            Name = computerNameEntry.Text,
            Type = computerTypeEntry.Text,
            Description = computerDescriptionEditor.Text,
            Model = modelEntry.Text,
            CPU = cpuEntry.Text,
            GPU = gpuEntry.Text,
            RAM = ramEntry.Text,
            Storage = storageEntry.Text,
            Price = price,
            ImagePath = selectedImage
        };

        await App.DbService.AddComputerAsync(newComputer);
        await DisplayAlert("Успешно", "Компьютер добавлен", "OK");
        await Navigation.PopAsync();
    }
}
