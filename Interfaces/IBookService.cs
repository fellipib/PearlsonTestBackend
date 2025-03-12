namespace PearlsonTestBackend.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book updatedBook);
        Task<bool> DeleteBookAsync(int id);

    }
}