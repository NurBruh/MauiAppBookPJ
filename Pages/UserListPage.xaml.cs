using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class UserListPage : ContentPage
{
    public UserListPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (App.CurrentUser?.Username != "admin")
        {
            await DisplayAlert("Ошибка", "Только администратор может просматривать список пользователей", "ОК");
            await Navigation.PopAsync();
            return;
        }

        var users = await App.DbService.GetAllUsersAsync();
        usersCollection.ItemsSource = users;
    }
}
