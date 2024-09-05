using System;
using MyApp.Models;

namespace MyApp.Repositories
{
    public interface IBooksRepository
    {
        Task<List<BooksTable>> GetBooksAsync();
        Task AddBookAsync(BooksTable book);
        Task UpdateBookAsync(BooksTable book);
        Task DeleteBookAsync(int id);
        Task<List<BooksTable>> SearchBooksAsync(string query);
    }

}

