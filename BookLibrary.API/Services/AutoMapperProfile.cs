using AutoMapper;
using BookLibrary.API.Models;
using BookLibrary.API.Models.Users;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;

namespace BookLibrary.API.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddBookRequest, BookDto>();
            CreateMap<Book, BookDto>();
            CreateMap<AddUserRequest, UserDto>();
            CreateMap<User, UserDto>();
            //CreateMap<Book, PaginatedList<BookDto>>();
            //CreateMap<BookDto, PaginatedList<Book>>();
        }
    }
}
