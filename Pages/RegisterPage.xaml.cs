using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string password = passwordEntry.Text?.Trim();
        string confirm = confirmEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Ошибка", "Введите логин и пароль", "ОК");
            return;
        }

        if (password != confirm)
        {
            await DisplayAlert("Ошибка", "Пароли не совпадают", "ОК");
            return;
        }

        var user = new User
        {
            Username = username,
            Password = password
        };

        await App.DbService.AddUserAsync(user);
        await DisplayAlert("Успешно", "Пользователь зарегистрирован", "ОК");

        await Navigation.PopAsync(); 
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); 
    }
}
