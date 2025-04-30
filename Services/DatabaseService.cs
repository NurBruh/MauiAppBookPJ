using SQLite;
using MauiAppBookPJ.Models;

namespace MauiAppBookPJ.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _db;
    private readonly string _dbPath;

    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
    }

    public async Task InitAsync()
    {
        if (_db != null)
            return;

        _db = new SQLiteAsyncConnection(_dbPath);
        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Book>();
        await _db.CreateTableAsync<Review>();
        await _db.CreateTableAsync<OrderHistory>();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        await InitAsync();
        return await _db.Table<User>().ToListAsync();
    }

    public async Task<User> GetUserByCredentialsAsync(string username, string password)
    {
        await InitAsync();
        return await _db.Table<User>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task AddUserAsync(User user)
    {
        await InitAsync();
        await _db.InsertAsync(user);
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        await InitAsync();
        return await _db.Table<Book>().ToListAsync();
    }

    public async Task AddBookAsync(Book book)
    {
        await InitAsync();
        await _db.InsertAsync(book);
    }

    public async Task UpdateBookAsync(Book book)
    {
        await InitAsync();
        await _db.UpdateAsync(book);
    }

    public async Task DeleteBookAsync(Book book)
    {
        await InitAsync();
        await _db.DeleteAsync(book);
    }

  
    public async Task AddReviewAsync(Review review)
    {
        await InitAsync();
        await _db.InsertAsync(review);
    }

    public async Task<List<Review>> GetReviewsByBookIdAsync(int bookId)
    {
        await InitAsync();
        return await _db.Table<Review>()
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

   
    public async Task AddOrderHistoryAsync(OrderHistory order)
    {
        await InitAsync();
        await _db.InsertAsync(order);
    }

    public async Task<List<OrderHistory>> GetOrderHistoryByUserAsync(int userId)
    {
        await InitAsync();
        return await _db.Table<OrderHistory>()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}
