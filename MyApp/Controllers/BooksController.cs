using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Repositories;
using MyApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksrepo;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksRepository booksrepo,ILogger<BooksController> logger)
        {
            _booksrepo = booksrepo;
            _logger = logger;
        }
        [Authorize(Roles = "user, admin")]
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksrepo.GetBooksAsync();
            return View(books);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddBooks()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
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
    }
}

