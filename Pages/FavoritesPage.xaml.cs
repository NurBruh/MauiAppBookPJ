using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var allBooks = await App.DbService.GetBooksAsync();
        var favorites = allBooks
            .Where(b => b.IsFavorite && b.UserId == App.CurrentUser?.Id)
            .ToList();

        favoritesCollection.ItemsSource = favorites;
    }
}
