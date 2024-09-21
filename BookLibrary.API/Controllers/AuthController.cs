using BookLibrary.API.Models.Users;
using BookLibrary.API.Services;
using BookLibrary.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController, AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService): base(logger)
        {
                _authService = authService;
        }

        /// <summary>
        /// Test API health
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new ServiceResponse("AuthController Up!!!"));
        }

        /// <summary>
        /// Login existing user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(LoginResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authService.Login(request.Validate());
            return Ok(user);
        }
    }
}
