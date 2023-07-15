using BookStoreApi.Models;
using BookStoreApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Service;

public class BooksService
{
    // interface do mongo que permite acessar os dados de um tabela especifica
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(
        IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetBooksAsync() => 
        await _booksCollection.FindSync(_ => true).ToListAsync();

    public async Task<Book> GetBookAsync(string id) =>
        await _booksCollection.Find(book => book.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);
    
    public async Task UpdateAsync(string id, Book updateBook) =>
        await _booksCollection.ReplaceOneAsync(book => book.Id == id, updateBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(book => book.Id == id);
}
