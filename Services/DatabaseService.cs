using SQLite;
using MauiAppBookPJ.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppBookPJ.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;
        private readonly string _dbPath;

        public DatabaseService(string dbPath)
        {
            _dbPath = dbPath;
        }

        // Инициализация базы данных
        public async Task InitAsync()
        {
            if (_db != null)
                return;

            _db = new SQLiteAsyncConnection(_dbPath);
            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<Book>();
            await _db.CreateTableAsync<Computer>();
            await _db.CreateTableAsync<Review>();
            await _db.CreateTableAsync<OrderHistory>();
        }

        // --- Users ---
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

        // --- Books ---
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

        // --- Reviews ---
        public async Task AddReviewAsync(Review review)
        {
            await InitAsync();
            if (review != null)
                await _db.InsertAsync(review);
        }

        public async Task<List<Review>> GetReviewsByComputerIdAsync(int computerId)
        {
            await InitAsync();
            return await _db.Table<Review>()
                .Where(r => r.ComputerId == computerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // --- Order History ---
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

        // --- Computers ---
        public async Task<List<Computer>> GetComputersAsync()
        {
            await InitAsync();
            return await _db.Table<Computer>().ToListAsync();
        }

        public async Task<List<Computer>> GetComputersInCartAsync()
        {
            await InitAsync();
            return await _db.Table<Computer>().Where(c => c.InCart).ToListAsync();
        }

        public async Task AddComputerAsync(Computer computer)
        {
            await InitAsync();
            await _db.InsertAsync(computer);
        }

        public async Task UpdateComputerAsync(Computer computer)
        {
            await InitAsync();
            await _db.UpdateAsync(computer);
        }

        public async Task DeleteComputerAsync(Computer computer)
        {
            await InitAsync();
            await _db.DeleteAsync(computer);
        }

        public async Task AddToCartAsync(Computer computer)
        {
            await InitAsync();
            computer.InCart = true;
            await _db.UpdateAsync(computer);
        }

        public async Task RemoveFromCartAsync(Computer computer)
        {
            await InitAsync();
            computer.InCart = false;
            await _db.UpdateAsync(computer);
        }

        public async Task CheckoutAsync()
        {
            await InitAsync();
            var cartItems = await GetComputersInCartAsync();
            foreach (var item in cartItems)
            {
                item.InCart = false;
                await _db.UpdateAsync(item);
            }
        }
    }
}
