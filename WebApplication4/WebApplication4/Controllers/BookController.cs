using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication3.Logic;
using WebApplication3.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly List<Book> _books;

        public BooksController(IOptions<List<Book>> books)
        {
            _books = books.Value;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            return _books;
        }

        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            // Check if the book is null
            if (book == null)
            {
                return BadRequest("Book cannot be null.");
            }

            // Check if a book with the same ID already exists
            if (_books.Any(b => b.Id == book.Id))
            {
                return Conflict($"A book with ID {book.Id} already exists."); // Return 409 Conflict
            }


            _books.Add(book);

            // Обновляем appsettings.json
            var fileUpdater = new JsonFileUpdater("appsettings.json");
            fileUpdater.UpdateBooks(_books);

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateBook(int id, Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Genre = updatedBook.Genre;

            // Обновляем appsettings.json
            var fileUpdater = new JsonFileUpdater("appsettings.json");
            fileUpdater.UpdateBooks(_books);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _books.Remove(book);

            // Обновляем appsettings.json
            var fileUpdater = new JsonFileUpdater("appsettings.json");
            fileUpdater.UpdateBooks(_books);

            return NoContent();
        }
    }
}
