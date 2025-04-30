using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string password = passwordEntry.Text?.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Ошибка", "Введите логин и пароль", "ОК");
            return;
        }

        if (username == "admin" && password == "admin")
        {
            App.CurrentUser = new User { Id = 0, Username = "admin", Password = "admin" };

            App.Current.MainPage = new AppShell();
            ((AppShell)Shell.Current).BuildMenu();
            return;
        }

        var user = await App.DbService.GetUserByCredentialsAsync(username, password);

        if (user != null)
        {
            App.CurrentUser = user;
            App.Current.MainPage = new AppShell();
            ((AppShell)Shell.Current).BuildMenu();
        }
        else
        {
            await DisplayAlert("Ошибка", "Неверный логин или пароль", "ОК");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}
