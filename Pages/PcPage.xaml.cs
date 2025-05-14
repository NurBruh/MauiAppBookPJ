using MauiAppBookPJ.Models;
using MauiAppBookPJ.Services;
using System.Windows.Input;

namespace MauiAppBookPJ.Pages;

public partial class PcPage : ContentPage
{
    private int _carouselIndex = 0;
    private IDispatcherTimer _carouselTimer;
    private List<Computer> _allComputers = new();
    public ICommand PcTappedCommand { get; private set; }

    private readonly List<string> _types = new()
    {
        "Все ПК", "Gaming", "Office", "Home"
    };

    public PcPage()
    {
        InitializeComponent();  // Убедитесь, что вызов метода InitializeComponent есть в вашем конструкторе

        // Проверка видимости карусели для обычного пользователя
        carousel.IsVisible = App.CurrentUser?.Username != "admin";

        // Настройка автоперелистывания карусели, если она видима
        if (carousel.IsVisible)
        {
            _carouselTimer = Dispatcher.CreateTimer();
            _carouselTimer.Interval = TimeSpan.FromSeconds(3);
            _carouselTimer.Tick += (s, e) =>
            {
                _carouselIndex = (_carouselIndex + 1) % 3;
                carousel.Position = _carouselIndex;
            };
            _carouselTimer.Start();
        }

        // Команда для выбора ПК, которая в зависимости от пользователя открывает нужную страницу
        PcTappedCommand = new Command<Computer>(async (selectedPc) =>
        {
            if (selectedPc != null)
            {
                if (App.CurrentUser?.Username == "admin")
                {
                    await Navigation.PushAsync(new EditComputerPage(selectedPc));
                }
                else
                {
                    var page = new ComputerDetailsPage(selectedPc);
                    await Navigation.PushAsync(page);
                }
            }
        });

        // Привязка контекста данных
        BindingContext = this;

        bool isAdmin = App.CurrentUser?.Username == "admin";

        // Управление видимостью элементов в зависимости от роли пользователя
        addPcButton.IsVisible = isAdmin;
        searchEntry.IsVisible = !isAdmin;
        typeFilterPicker.IsVisible = !isAdmin;

        // Если это не админ, настраиваем фильтр по типу ПК
        if (!isAdmin)
            typeFilterPicker.ItemsSource = _types;
    }

    // Загрузка данных ПК при отображении страницы
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _allComputers = await App.DbService.GetComputersAsync();
        ApplyFilters();
    }

    // Обработчик изменения текста в поисковом поле
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    // Обработчик изменения фильтра по типу ПК
    private void OnTypeFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    // Применение фильтров на основе поиска и типа ПК
    private void ApplyFilters()
    {
        string search = searchEntry.Text?.ToLower() ?? "";
        string selectedType = typeFilterPicker.SelectedItem as string;

        var filtered = _allComputers
            .Where(pc =>
                (string.IsNullOrEmpty(search) || pc.Name.ToLower().Contains(search)) &&
                (string.IsNullOrEmpty(selectedType) || selectedType == "Все ПК" ||
                 (!string.IsNullOrEmpty(pc.Type) && pc.Type.Equals(selectedType, StringComparison.OrdinalIgnoreCase)))
            )
            .ToList();

        pcCollection.ItemsSource = filtered;
    }


    // Обработчик нажатия кнопки добавления нового ПК
    private async void OnAddPcClicked(object sender, EventArgs e)
    {
        await addPcButton.ScaleTo(0.92, 80);
        await addPcButton.ScaleTo(1.0, 80);
        await Navigation.PushAsync(new AddComputerPage());
    }

    // Обработчик выбора ПК из коллекции
    private async void OnPcSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Computer selectedPc)
        {
            pcCollection.SelectedItem = null;

            if (App.CurrentUser?.Username == "admin")
            {
                await Navigation.PushAsync(new EditComputerPage(selectedPc));
            }
            else
            {
                await Navigation.PushAsync(new ComputerDetailsPage(selectedPc));
            }
        }
    }

    // Обработчик добавления ПК в корзину
    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Computer computer)
        {
            CartService.AddToCart(computer);
            await DisplayAlert("Корзина", "Товар добавлен в корзину", "ОК");
        }
    }
}
