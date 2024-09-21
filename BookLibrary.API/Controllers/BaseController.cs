using BookLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        public Guid UserId { get => GetUserId(); }
        public string Email { get => GetUserEmail(); }

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }


        private string GetUserEmail()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var email = identity.FindFirst(ClaimTypes.Email).Value;


            if (string.IsNullOrEmpty(email))
                throw new Exception("Unable to verify user");

            return email;
        }

        private Guid GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value; ;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("Unable to verify user");

            return Guid.Parse(userId);
        }
        
        private string GetUserRole()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userRole = identity.FindFirst(ClaimTypes.Role).Value;


            if (string.IsNullOrEmpty(userRole))
                throw new Exception("Unable to verify user");

            return userRole;
        }

        protected IActionResult ServiceRespons<T>(ServiceResponse<T> serviceResponse) where T : class
        {
            try
            {
                if (serviceResponse.Data == null)
                {
                    return NotFound(serviceResponse);
                }
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                return ProcessErrorResponse(serviceResponse.Message, ex);
            }
        }

        protected IActionResult ProcessErrorResponse(string errorResponse, Exception ex)
        {
            if (ex.GetType().Name == typeof(ArgumentNullException).Name || ex.GetType().Name == typeof(ArgumentException).Name)
            {
                return StatusCode(400, new ServiceResponse(errorResponse));
            }
            return StatusCode(500, new ServiceResponse(errorResponse));
        }
    }
}
