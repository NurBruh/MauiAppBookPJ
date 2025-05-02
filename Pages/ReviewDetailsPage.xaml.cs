using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Pages
{
    public partial class ReviewDetailsPage : ContentPage
    {
        public Review Review { get; set; }

        public ReviewDetailsPage(Review review)
        {
            InitializeComponent(); // Этот метод инициализирует компоненты XAML для страницы
            Review = review;
            BindingContext = Review; // Привязываем объект Review к текущей странице для отображения данных
        }
    }
}
