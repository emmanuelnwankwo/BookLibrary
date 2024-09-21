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
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListTypeConverter<,>));

        }
    }

    public class PaginatedListTypeConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination,ResolutionContext context)
        {
            // Map each item from TSource to TDestination
            var mappedItems = source.Items.Select(item => context.Mapper.Map<TDestination>(item)).ToList();

            // Create a new PaginatedList<TDestination> with the mapped items
            return new PaginatedList<TDestination>(mappedItems, source.PageIndex, source.PageSize, source.TotalCount);
        }
    }

}
