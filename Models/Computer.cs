using SQLite;

namespace MauiAppBookPJ.Models
{
    public class Computer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string CPU { get; set; }
        public string GPU { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; } // Добавлено свойство Price

        // Флаг для отслеживания, находится ли товар в корзине
        public bool InCart { get; set; }

        // Свойство для отображения спецификаций
        public string Specifications => $"{CPU}, {GPU}, {RAM} RAM, {Storage} Storage";
    }
}
