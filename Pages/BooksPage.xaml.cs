using MauiAppBookPJ.Models;
using System.Globalization;
using System.Windows.Input;



namespace MauiAppBookPJ.Pages;

public partial class BooksPage : ContentPage
{
    private int _carouselIndex = 0;
    private IDispatcherTimer _carouselTimer;
    private List<Book> _allBooks = new();
    public ICommand BookTappedCommand { get; }

    private readonly List<string> _genres = new()
    {
        "Все жанры", "Драма", "Приключения", "Фантастика", "Детектив", "Комедия", "Ужасы"
    };

    public BooksPage()
    {
        InitializeComponent();
   
        carousel.IsVisible = App.CurrentUser?.Username != "admin";

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

        BookTappedCommand = new Command<Book>(async (selectedBook) =>
        {
            if (selectedBook != null)
            {
                if (App.CurrentUser?.Username == "admin")
                {
                    await Navigation.PushAsync(new EditBookPage(selectedBook)); 
                }
                else
                {
                    var page = new BookDetailsPage();
                    page.BindingContext = selectedBook;
                    await Navigation.PushAsync(page);
                }
            }
        });

        BindingContext = this;

      
        bool isAdmin = App.CurrentUser?.Username == "admin";
        addBookButton.IsVisible = isAdmin;
        searchEntry.IsVisible = !isAdmin;
        genreFilterPicker.IsVisible = !isAdmin;

        if (!isAdmin)
            genreFilterPicker.ItemsSource = _genres;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _allBooks = await App.DbService.GetBooksAsync();
        ApplyFilters();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    } 

    private void OnGenreFilterChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        string search = searchEntry.Text?.ToLower() ?? "";
        string selectedGenre = genreFilterPicker.SelectedItem as string;

        var filtered = _allBooks
            .Where(book =>
                (string.IsNullOrEmpty(search) || book.Title.ToLower().Contains(search)) &&
                (string.IsNullOrEmpty(selectedGenre) || selectedGenre == "Все жанры" || book.Genre == selectedGenre)
            )
            .ToList();

        booksCollection.ItemsSource = filtered;
    }

    private async void OnAddBookClicked(object sender, EventArgs e)
    {
        await addBookButton.ScaleTo(0.92, 80);
        await addBookButton.ScaleTo(1.0, 80);
        await Navigation.PushAsync(new AddBookPage());
    }

    private async void OnBookSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Book selectedBook)
        {
            booksCollection.SelectedItem = null;

            if (App.CurrentUser?.Username == "admin")
            {
                await Navigation.PushAsync(new EditBookPage(selectedBook));
            }
            else
            {
                await Navigation.PushAsync(new BookDetailsPage
                {
                    BindingContext = selectedBook
                });
            }
        }
    }
    
}
