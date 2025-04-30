using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class AddBookPage : ContentPage
{
    private string selectedCover;

    public AddBookPage()
    {
        InitializeComponent();
    }

    private void OnCoverTapped(object sender, TappedEventArgs e)
    {
        if (e?.Parameter is string cover)
        {
            selectedCover = cover;
            coverPreview.Source = selectedCover;
            coverPreview.IsVisible = true;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text) ||
            string.IsNullOrWhiteSpace(authorEntry.Text) ||
            string.IsNullOrWhiteSpace(descriptionEditor.Text) ||
            genrePicker.SelectedItem == null ||
            string.IsNullOrEmpty(selectedCover))
        {
            await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля и выберите обложку.", "ОК");
            return;
        }

        var book = new Book
        {
            Title = titleEntry.Text,
            Author = authorEntry.Text,
            Genre = genrePicker.SelectedItem.ToString(),
            Description = descriptionEditor.Text,
            CoverImagePath = selectedCover,
            
        };

        await App.DbService.AddBookAsync(book);
        await DisplayAlert("Успешно", "Книга добавлена!", "ОК");
        await Navigation.PopAsync();
    }
}
