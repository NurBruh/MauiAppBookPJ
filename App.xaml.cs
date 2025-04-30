using MauiAppBookPJ.Pages;
using MauiAppBookPJ.Services;

namespace MauiAppBookPJ;

public partial class App : Application
{
    public static DatabaseService DbService { get; private set; }

    
    public static Models.User CurrentUser { get; set; }

    public App()
    {
        InitializeComponent();

        DbService = new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "books.db3"));


        
        MainPage = new SplashPage();

    }
}
