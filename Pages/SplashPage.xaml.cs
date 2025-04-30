namespace MauiAppBookPJ.Pages;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        AnimateLogo();
    }

    private async void AnimateLogo()
    {
        await Task.Delay(300);
        await logoImage.FadeTo(1, 1000, Easing.CubicIn);
        await logoImage.ScaleTo(1.1, 1000, Easing.CubicOut);
        await logoImage.ScaleTo(1, 800, Easing.CubicIn);

        await Task.Delay(800);

        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
