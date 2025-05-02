using MauiAppBookPJ.Pages;

namespace MauiAppBookPJ;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BuildMenu();
    }

    public void BuildMenu()
    {
        Items.Clear();

        if (App.CurrentUser?.Username == "admin")
        {
            Items.Add(new FlyoutItem
            {
                Title = "Каталог ПК",
                Route = "pc",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(PcPage)),
                        Route = "pc"
                    }
                }
            });

            Items.Add(new FlyoutItem
            {
                Title = "👥 Пользователи",
                Route = "users",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(UserListPage)),
                        Route = "users"
                    }
                }
            });
        }
        else
        {
            Items.Add(new FlyoutItem
            {
                Title = "💻 ПК Каталог",
                Route = "pc",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(PcPage)),
                        Route = "pc"
                    }
                }
            });

            Items.Add(new FlyoutItem
            {
                Title = "🛒 Корзина",
                Route = "cart",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(CartPage)),
                        Route = "cart"
                    }
                }
            });

            Items.Add(new FlyoutItem
            {
                Title = "🧾 История заказов",
                Route = "orders",
                Items =
                {
                    new ShellContent
                    {
                        ContentTemplate = new DataTemplate(typeof(OrderHistoryPage)),
                        Route = "orders"
                    }
                }
            });
        }

        Items.Add(new MenuItem
        {
            Text = "🚪 Выйти",
            Command = new Command(() =>
            {
                App.CurrentUser = null;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            })
        });
    }

    private async void OnInstagramTapped(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync("https://www.instagram.com/senjumarru/");
    }

    private async void OnWhatsAppTapped(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync("https://wa.me/7073940282");
    }

    private async void OnTikTokTapped(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync("https://www.tiktok.com/@alinurrro?_t=ZS-8w1cwNZtNVU&_r=1");
    }
}
