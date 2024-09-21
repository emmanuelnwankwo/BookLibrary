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
            var email = GetClaim(ClaimTypes.Email);

            return email;
        }

        private Guid GetUserId()
        {
            var userId = GetClaim(ClaimTypes.NameIdentifier);

            return Guid.Parse(userId);
        }
        
        private string GetUserRole()
        {
            var userRole = GetClaim(ClaimTypes.Role);

            return userRole;
        }

        private string GetClaim(string claim)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var data = identity.FindFirst(claim).Value;
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("Unable to verify user");

            return data;
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
