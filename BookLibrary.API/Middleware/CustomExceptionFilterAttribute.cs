using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.API.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    new
                    {
                        Message = ((ValidationException)context.Exception)
                    });

                return;
            }

            var code = HttpStatusCode.BadRequest;
            string message = context.Exception.Message;

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (context.Exception is ApplicationException)
            {
                code = HttpStatusCode.InternalServerError;
            }
            if (context.Exception is ArgumentNullException ex)
            {
                message = ex.ParamName;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                Message = message,
            });
            
            //context.Result = new JsonResult(new
            //{
            //    Message = new[] { context.Exception.Message },
            //});
        }

        public class NotFoundException : Exception
        {
            public NotFoundException(string name, object key)
                : base($"Entity \"{name}\" | ({key}) was not found.")
            {
            }
        }
    }
}
