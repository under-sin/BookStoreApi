using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BookRepository(
        IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<IEnumerable<Book>> GetAllAsync() =>
        await _booksCollection.FindSync(_ => true).ToListAsync();

    public async Task<Book> GetByIdAsync(string id) =>
        await _booksCollection.Find(book => book.Id == id).FirstOrDefaultAsync();

    public async Task CreateBookAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateBookAsync(Book updateBook) =>
        await _booksCollection.ReplaceOneAsync(book => book.Id == updateBook.Id, updateBook);

    public async Task RemoveBookAsync(string id) =>
        await _booksCollection.DeleteOneAsync(book => book.Id == id);

    public async Task<List<Book>> GetPagedAsync(int pageNumber, int pageSize)
    {
        // order por id de forma ascending
        var sort = Builders<Book>.Sort.Ascending(book => book.Id);

        var books = await _booksCollection
            .Find(FilterDefinition<Book>.Empty)
            .Sort(sort)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return books;
    }
}
