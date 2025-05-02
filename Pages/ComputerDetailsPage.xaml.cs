using MauiAppBookPJ.Models;
using MauiAppBookPJ.Services;
using System;
using System.Collections.Generic;

namespace MauiAppBookPJ.Pages
{
    public partial class ComputerDetailsPage : ContentPage
    {
        public Computer Computer { get; set; }
        private readonly DatabaseService _databaseService;

        // Конструктор страницы
        public ComputerDetailsPage(Computer computer)
        {
            InitializeComponent();
            Computer = computer;
            BindingContext = Computer;

            _databaseService = App.DbService; // Ссылка на сервис базы данных
            LoadReviews(); // Загружаем отзывы при инициализации страницы
        }

        // Метод для загрузки отзывов
        private async void LoadReviews()
        {
            try
            {
                // Получаем отзывы для текущего компьютера
                var reviews = await _databaseService.GetReviewsByComputerIdAsync(Computer.Id);
                if (reviews != null && reviews.Count > 0)
                {
                    ReviewsList.ItemsSource = reviews; // Привязываем список отзывов к элементу списка
                }
                else
                {
                    ReviewsList.ItemsSource = new List<Review>(); // Если нет отзывов, показываем пустой список
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если не удалось загрузить отзывы
                await DisplayAlert("Ошибка", "Не удалось загрузить отзывы: " + ex.Message, "ОК");
            }
        }

        // Метод для добавления нового отзыва
        private async void OnAddReviewClicked(object sender, EventArgs e)
        {
            string author = ReviewAuthorEntry.Text?.Trim(); // Получаем имя автора отзыва
            string content = ReviewContentEditor.Text?.Trim(); // Получаем текст отзыва

            // Проверка на пустые значения
            if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(content))
            {
                await DisplayAlert("Ошибка", "Введите имя и текст отзыва", "ОК");
                return;
            }

            var review = new Review
            {
                ComputerId = Computer.Id,
                Username = author,
                Text = content,
                CreatedAt = DateTime.Now // Устанавливаем дату создания отзыва
            };

            try
            {
                // Добавляем отзыв в базу данных
                await _databaseService.AddReviewAsync(review);

                // Очищаем поля ввода
                ReviewAuthorEntry.Text = "";
                ReviewContentEditor.Text = "";

                LoadReviews(); // Перезагружаем список отзывов
            }
            catch (Exception ex)
            {
                // Обработка ошибок при добавлении отзыва
                await DisplayAlert("Ошибка", "Не удалось добавить отзыв: " + ex.Message, "ОК");
            }
        }

        // Обработчик для кнопки "Оформить заказ"
        private async void OnOrderComputerClicked(object sender, EventArgs e)
        {
            try
            {
                // Добавляем компьютер в корзину
                CartService.AddToCart(Computer);

                // Переходим на страницу корзины
                await Navigation.PushAsync(new CartPage());
            }
            catch (Exception ex)
            {
                // Обработка ошибок при оформлении заказа
                await DisplayAlert("Ошибка", "Не удалось оформить заказ: " + ex.Message, "ОК");
            }
        }

        // Метод для перехода на страницу деталей отзыва
        private async void OnReviewTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var review = e.Item as Review;
                if (review != null)
                {
                    // Переход на страницу деталей отзыва
                    await Navigation.PushAsync(new ReviewDetailsPage(review));
                }
            }
        }
    }
}
