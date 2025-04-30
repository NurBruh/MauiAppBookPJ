using CommunityToolkit.Maui.Alerts;
using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class BookDetailsPage : ContentPage
{
    private Book _book;

    public BookDetailsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Book book)
        {
            _book = book;

            titleLabel.Text = book.Title;
            authorLabel.Text = $"Автор: {book.Author}";
            genreLabel.Text = $"Жанр: {book.Genre}";
            descriptionLabel.Text = book.Description;
            coverImage.Source = book.CoverImagePath;

            favoriteButton.Text = _book.IsFavorite ? "✅ Удалить из избранного" : "⭐ В избранное";
            cartButton.Text = _book.InCart ? "✅ Удалить из корзины" : "🛒 В корзину";

            await LoadReviews();
        }
    }

    private async void OnToggleFavoriteClicked(object sender, EventArgs e)
    {
        await favoriteButton.ScaleTo(0.9, 80);
        await favoriteButton.ScaleTo(1.0, 80);

        _book.IsFavorite = !_book.IsFavorite;
        _book.UserId = App.CurrentUser?.Id; // Привязка книги к пользователю
        await App.DbService.UpdateBookAsync(_book);

        await Toast.Make(_book.IsFavorite ? "Добавлено в избранное" : "Удалено из избранного").Show();
        favoriteButton.Text = _book.IsFavorite ? "✅ Удалить из избранного" : "⭐ В избранное";
    }

    private async void OnToggleCartClicked(object sender, EventArgs e)
    {
        await cartButton.ScaleTo(0.9, 80);
        await cartButton.ScaleTo(1.0, 80);

        _book.InCart = !_book.InCart;
        _book.UserId = App.CurrentUser?.Id; 
        await App.DbService.UpdateBookAsync(_book);

        await Toast.Make(_book.InCart ? "Добавлено в корзину" : "Удалено из корзины").Show();
        cartButton.Text = _book.InCart ? "✅ Удалить из корзины" : "🛒 В корзину";
    }

    public async void OnLeaveReviewClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Проверка", "Нажата кнопка оставить отзыв", "ОК");

        if (_book == null || string.IsNullOrWhiteSpace(reviewEditor.Text))
        {
            await DisplayAlert("Ошибка", "Введите текст отзыва", "ОК");
            return;
        }

        var review = new Review
        {
            BookId = _book.Id,
            Username = App.CurrentUser?.Username ?? "Аноним",
            Text = reviewEditor.Text,
            CreatedAt = DateTime.Now
        };

        try
        {
            await App.DbService.AddReviewAsync(review);
            await DisplayAlert("OK", "Отзыв сохранён", "OK");
            reviewEditor.Text = string.Empty;
            await LoadReviews();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить отзыв: {ex.Message}", "ОК");
        }
    }

    public async Task LoadReviews()
    {
        try
        {
            var reviews = await App.DbService.GetReviewsByBookIdAsync(_book.Id);
            await DisplayAlert("Отзывы", $"Загружено: {reviews.Count} отзывов", "OK");
            reviewsCollection.ItemsSource = reviews;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить отзывы: {ex.Message}", "ОК");
        }
    }
}
