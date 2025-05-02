using MauiAppBookPJ.Models;
using MauiAppBookPJ.Pages;
using MauiAppBookPJ.Services;

namespace MauiAppBookPJ;

public partial class App : Application
{
    public static DatabaseService DbService { get; private set; }
    public static User CurrentUser { get; set; }

    public App()
    {
        InitializeComponent();

        DbService = new DatabaseService(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "books.db3"));

        // Установка стартовой страницы приложения
        MainPage = new SplashPage();

        // Если нужно тестировать AddReviewPage напрямую, раскомментируйте строку ниже:
        // MainPage = new NavigationPage(new AddReviewPage(computerId: 1)); // Пример передачи ComputerId
    }
}

public static class CartService
{
    public static List<Computer> CartItems { get; set; } = new();

    public static void AddToCart(Computer item) => CartItems.Add(item);

    public static void RemoveFromCart(Computer item) => CartItems.Remove(item);

    public static decimal GetTotalPrice() => CartItems.Sum(item => item.Price);
}
