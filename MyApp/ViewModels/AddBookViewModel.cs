using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.ViewModels
{
	public class AddBookViewModel
    {
		
        [Required(ErrorMessage = "Please enter the Book Name")]
        public string BookName { get; set; }
        [Required(ErrorMessage = "Please enter the Author Name")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Please enter valid price")]
        public Decimal Price { get; set; }

    }
}