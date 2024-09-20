using AutoMapper;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;

namespace BookLibrary.API.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(ILogger<UserService> logger, IMapper mapper, IUserRepository userRepository) 
            : base(logger, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _userRepository.GetAsync(x => x.Email == email);
            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }
            return user;
        }
    }
}
