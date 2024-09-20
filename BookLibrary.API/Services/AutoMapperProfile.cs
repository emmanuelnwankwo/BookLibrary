using AutoMapper;
using BookLibrary.API.Models;
using BookLibrary.Domain.DTOs;

namespace BookLibrary.API.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddBookRequest, BookDto>();
        }
    }
}
