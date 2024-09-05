using System;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Repositories
{
    public class BookRepository : IBooksRepository
	{
        private readonly ILogger<BookRepository> _logger;
		private readonly AppDbContext _context;
		public BookRepository(AppDbContext context,ILogger<BookRepository> logger)
		{
			_context = context;
            _logger = logger;
		}

        public async Task AddBookAsync(BooksTable book)
        {
            _logger.LogInformation("Add Book method started");
            _context.BooksTable.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.BooksTable.FindAsync(id);
            if (book != null)
            {
                _context.BooksTable.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BooksTable>> GetBooksAsync()
        {
            return await Task.FromResult(_context.BooksTable.ToList());
        }

        public async Task<List<BooksTable>> SearchBooksAsync(string query)
        {
            return await Task.FromResult(
                string.IsNullOrEmpty(query)
                    ? _context.BooksTable.ToList()
                    : _context.BooksTable.Where(b => b.BookName.Contains(query)).ToList());
        }

        public async Task UpdateBookAsync(BooksTable book)
        {
            _context.BooksTable.Update(book);
            await _context.SaveChangesAsync();
        }
        public async Task<BooksTable> GetBookByIdAsync(int id)
        {
            return await _context.BooksTable.FindAsync(id);
        }
    }
}

