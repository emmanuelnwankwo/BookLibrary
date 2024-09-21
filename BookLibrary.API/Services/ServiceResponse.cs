using System.Text.Json.Serialization;

namespace BookLibrary.API.Services
{
    public class ServiceResponse<T> : ServiceResponse where T : class
    {
        public ServiceResponse(string message)
            : base(message)
        {
        }

        public new T Data { get; set; }
    }

    public class ServiceResponse
    {
        public ServiceResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
