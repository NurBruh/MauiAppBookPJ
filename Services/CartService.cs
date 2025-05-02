using System.Collections.ObjectModel;
using MauiAppBookPJ.Models;
using System.Linq;

namespace MauiAppBookPJ.Services
{
    public static class CartService
    {
        // Изменил тип с PC на Computer
        public static ObservableCollection<Computer> CartItems { get; } = new ObservableCollection<Computer>();

        // Метод для добавления в корзину
        public static void AddToCart(Computer computer)
        {
            // Проверяем, не добавлен ли уже этот товар в корзину
            if (!CartItems.Any(c => c.Id == computer.Id))
            {
                CartItems.Add(computer);
            }
        }

        // Метод для удаления товара из корзины
        public static void RemoveFromCart(Computer computer)
        {
            CartItems.Remove(computer);
        }

        // Метод для подсчета общей стоимости
        public static decimal GetTotalPrice()
        {
            return CartItems.Sum(computer => computer.Price); // Предполагается, что в классе Computer есть свойство Price
        }

        // Метод для очистки корзины
        public static void ClearCart()
        {
            CartItems.Clear();
        }
    }
}
