using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using PearlsonTestBackend.Controllers;
using PearlsonTestBackend;
using PearlsonTestBackend.Interfaces;

namespace Tests.Tests;

public class BookControllerTests
{   
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<ILogger<BookController>> _mockLogger;
    private readonly BookController _controller;

    public BookControllerTests()
    {
        _mockBookService = new Mock<IBookService>();
        _mockLogger = new Mock<ILogger<BookController>>();
        _controller = new BookController(_mockLogger.Object, _mockBookService.Object);
    }

    [Fact]
    public async Task GetBooks_ShouldReturnListOfBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Nome = "Livro 1231233", SKU = "SKU1231233", Preco = 112230 },
            new Book { Id = 2, Nome = "Book aaaa", SKU = "SKUaaaa", Preco = 212310 }
        };
        _mockBookService.Setup(service => service.GetBooksAsync()).ReturnsAsync(books);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(2, ((List<Book>)returnValue).Count);
    }

    [Fact]
    public async Task GetBook_ShouldReturnBook_WhenIdIsValid()
    {
        // Arrange
        var book = new Book { Id = 1, Nome = "Livro 1231233", SKU = "SKU1231233", Preco = 112230 };
        _mockBookService.Setup(service => service.GetBookByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _controller.GetBook(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(book, okResult.Value);
    }

    [Fact]
    public async Task GetBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        _mockBookService.Setup(service => service.GetBookByIdAsync(99)).ReturnsAsync((Book)null);

        // Act
        var result = await _controller.GetBook(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateBook_ShouldReturnCreatedBook()
    {
        // Arrange
        var book = new Book { Id = 1, Nome = "Livro 1231233", SKU = "SKU1231233", Preco = 112230 };
        _mockBookService.Setup(service => service.CreateBookAsync(book)).ReturnsAsync(book);

        // Act
        var result = await _controller.CreateBook(book);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(book, createdAtActionResult.Value);
        Assert.Equal("GetBook", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task UpdateBook_ShouldReturnUpdatedBook_WhenSuccess()
    {
        // Arrange
        var book = new Book { Id = 1, Nome = "Updated Book", SKU = "SKU789", Preco = 120 };
        _mockBookService.Setup(service => service.UpdateBookAsync(book)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateBook(book);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(book, okResult.Value);
    }

    [Fact]
    public async Task UpdateBook_ShouldReturnNotFound_WhenUpdateFails()
    {
        // Arrange
        var book = new Book { Id = 99, Nome = "Nonexistent Book" };
        _mockBookService.Setup(service => service.UpdateBookAsync(book)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateBook(book);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        _mockBookService.Setup(service => service.DeleteBookAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBook(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("Livro apagado", okResult.Value);
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        _mockBookService.Setup(service => service.DeleteBookAsync(99)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteBook(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);


    }
}