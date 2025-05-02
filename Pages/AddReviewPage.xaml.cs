using MauiAppBookPJ.Models;
using MauiAppBookPJ.Services;

namespace MauiAppBookPJ.Pages;

public partial class AddReviewPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly int _computerId;

    public AddReviewPage(DatabaseService dbService, int computerId)
    {
        InitializeComponent();
        _dbService = dbService;
        _computerId = computerId;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(TextEditor.Text))
        {
            await DisplayAlert("Ошибка", "Заполните все поля.", "ОК");
            return;
        }

        var review = new Review
        {
            Username = UsernameEntry.Text,
            Text = TextEditor.Text,
            CreatedAt = DateTime.Now,
            ComputerId = _computerId
        };

        await _dbService.AddReviewAsync(review);
        await DisplayAlert("Успешно", "Отзыв добавлен", "ОК");
        await Navigation.PopAsync();
    }
}
