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

        public Task DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BooksTable>> GetBooksAsync()
        {
            return await Task.FromResult(_context.BooksTable.ToList());
        }

        public Task<List<BooksTable>> SearchBooksAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBookAsync(BooksTable book)
        {
            throw new NotImplementedException();
        }
    }
}

