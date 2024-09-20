using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Repositories;
using MyApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksrepo;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksRepository booksrepo,ILogger<BooksController> logger)
        {
            _booksrepo = booksrepo;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksrepo.GetBooksAsync();
            return View(books);
        }
        [HttpGet]
        public IActionResult AddBooks()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBooks(AddBookViewModel book)
        {
            if (ModelState.IsValid)
            {
                _logger.LogWarning("Model Validated");
                var obj = new BooksTable
                {
                    BookName = book.BookName,
                    Author = book.Author,
                    Price = book.Price

                };
                await _booksrepo.AddBookAsync(obj);
                return RedirectToAction("GetBooks");



            }
            return View(book);
        }
        [HttpGet]
        public async Task<IActionResult> EditBook(int id)
        {
            
            var book = await _booksrepo.GetBookByIdAsync(id);
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BooksTable book)
        {
            if (ModelState.IsValid)
            {
                await _booksrepo.UpdateBookAsync(book);
                return RedirectToAction("GetBooks");

            }
            return View(book);
        }
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _booksrepo.DeleteBookAsync(id);
            return RedirectToAction("GetBooks");
        }

        [HttpGet]
        public async Task<ActionResult> Search(string query)
        {
            var results = await _booksrepo.SearchBooksAsync(query);
            return View("Search", results);
        }
    }
}

