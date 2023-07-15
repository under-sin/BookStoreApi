using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.ViewModel;

namespace BookStoreApi;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {   
        CreateMap<Book, BookViewModel>();
        CreateMap<BookViewModel, Book>();
    }
}
