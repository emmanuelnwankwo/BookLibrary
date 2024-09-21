using BookLibrary.API.Models.Users;
using BookLibrary.API.Services;
using BookLibrary.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookLibrary.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController, Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(ILogger<BaseController> logger, IUserService userService): base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Create new user data
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost, AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> CreateUser([FromBody] AddUserRequest request)
        {
            var user = await _userService.CreateUser(request.Validate());
            return StatusCode(201, user);
        }

    }
}
