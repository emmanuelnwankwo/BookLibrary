using AutoMapper;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using System.Security.Claims;

namespace BookLibrary.API.Services
{
    public abstract class BaseService
    {
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(ILogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}
