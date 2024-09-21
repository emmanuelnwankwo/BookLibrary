using AutoMapper;
using BookLibrary.API.Models.Users;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using static BookLibrary.Domain.Shared.Enums;

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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetAsync(x => x.Email == email);
            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }
            return user;
        }

        public async Task<UserDto> CreateUser(AddUserRequest request)
        {
            var userExist = await _userRepository.GetAsync(x => x.Email == request.Email);

            if (userExist != null)
                throw new ArgumentNullException("User already exist");

            var userInst = new User();
            var userDto = _mapper.Map<UserDto>(request);
            userDto.Role = UserRole.User;

            var user = userInst.AddUser(userDto);
            user.SetPassword(request.Password);

            await _userRepository.InsertAsync(user);
            await _userRepository.SaveChangesAsync();

            return userDto;
        }
    }
}
