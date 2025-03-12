using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using PearlsonTestBackend.Interfaces;


namespace PearlsonTestBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
       

        private readonly ILogger<BookController> _logger;        
        private readonly IBookService _bookService;


        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [HttpGet]
        public  async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                var books = await _bookService.GetBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            try {
                if (book == null)
                {
                    return BadRequest("Dados de livro inválidos");
                }

                var createdBook = await _bookService.CreateBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut()]
        public async Task<ActionResult<Book>> UpdateBook([FromBody] Book book)
        {
            try
            {
                {
                    if (book == null)
                    {
                        return BadRequest("Dados de livro inválidos");
                    }

                    var success = await _bookService.UpdateBookAsync(book);
                    if (!success)
                    {
                        return NotFound();
                    }

                    return Ok(book);
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return Ok("Livro apagado");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}

