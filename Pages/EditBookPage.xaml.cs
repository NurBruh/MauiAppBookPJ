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
            "�����", "�����������", "����������", "��������", "�����", "�������"
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
            await DisplayAlert("������", "��������� ��� ���� � �������� �������.", "OK");
            return;
        }

        _book.Title = titleEntry.Text;
        _book.Author = authorEntry.Text;
        _book.Genre = genrePicker.SelectedItem.ToString();
        _book.Description = descriptionEditor.Text;
        _book.CoverImagePath = selectedCover;

        await App.DbService.UpdateBookAsync(_book);
        await DisplayAlert("�������", "����� ���������", "OK");
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("�������", "�� �������, ��� ������ ������� �����?", "��", "���");
        if (answer)
        {
            await App.DbService.DeleteBookAsync(_book);
            await Navigation.PopAsync();
        }
    }
}
