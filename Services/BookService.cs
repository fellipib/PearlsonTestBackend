using MongoDB.Driver;
using PearlsonTestBackend.Interfaces;

namespace PearlsonTestBackend.Services
{
    public class BookService :IBookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _books = database.GetCollection<Book>(collectionName);

        }

        public async Task<List<Book>> GetBooksAsync()
        {
            try
            {
            return await _books.Find(book => true).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                return await _books.Find(book => book.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            try
            {
                int? nextId = 1;
                var lastBook = _books.AsQueryable().OrderByDescending(b => b.Id).FirstOrDefault();
                if (lastBook != null && lastBook.Id != null)
                {
                    nextId = lastBook.Id + 1;
                }
                book.Id = nextId;
                await _books.InsertOneAsync(book);
                return book;
            }
            catch (Exception) 
            {

                throw;
            }
        }

        public async Task<bool> UpdateBookAsync(Book updatedBook)
        { try
            {
                var result = await _books.ReplaceOneAsync(book => book.Id == updatedBook.Id, updatedBook);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try { 
            var result = await _books.DeleteOneAsync(book => book.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception) 
            {
                throw;
            }

        }
    }
}
