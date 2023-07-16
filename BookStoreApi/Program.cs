using BookStoreApi;
using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreApi.Service;
using BookStoreApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IBookRepository, BookRepository>();

// Configuração do automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMemoryCache();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // remove a validação padrão nas models. Isso é necessário para conseguir pegar a lista de erros.
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions( 
        // os nomes das propriedades na resposta JSON serão iguais aos nomes das propriedades
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
