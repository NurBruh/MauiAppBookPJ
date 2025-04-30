using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class EditBookPage : ContentPage
{
    private Book _book;
    private string selectedCover;

    public EditBookPage(Book book)
    {
        InitializeComponent();
        _book = book;

        genrePicker.ItemsSource = new List<string>
        {
            "Драма", "Приключения", "Фантастика", "Детектив", "Роман", "Фэнтези"
        };

        FillFields();
    }

    private void FillFields()
    {
        titleEntry.Text = _book.Title;
        authorEntry.Text = _book.Author;
        genrePicker.SelectedItem = _book.Genre;
        descriptionEditor.Text = _book.Description;
        selectedCover = _book.CoverImagePath;

        coverPreview.Source = selectedCover;
        coverPreview.IsVisible = true;
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
            genrePicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(descriptionEditor.Text) ||
            string.IsNullOrEmpty(selectedCover))
        {
            await DisplayAlert("Ошибка", "Заполните все поля и выберите обложку.", "OK");
            return;
        }

        _book.Title = titleEntry.Text;
        _book.Author = authorEntry.Text;
        _book.Genre = genrePicker.SelectedItem.ToString();
        _book.Description = descriptionEditor.Text;
        _book.CoverImagePath = selectedCover;

        await App.DbService.UpdateBookAsync(_book);
        await DisplayAlert("Успешно", "Книга обновлена", "OK");
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Удалить", "Вы уверены, что хотите удалить книгу?", "Да", "Нет");
        if (answer)
        {
            await App.DbService.DeleteBookAsync(_book);
            await Navigation.PopAsync();
        }
    }
}
