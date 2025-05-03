using SQLite;

namespace MauiAppBookPJ.Models;

public class Review
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int? ComputerId { get; set; } // может быть null, если это отзыв не на компьютер
    public int BookId { get; set; }
    public string Username { get; set; }

    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
