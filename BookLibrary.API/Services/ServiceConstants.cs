using System.ComponentModel.DataAnnotations;

namespace BookLibrary.API.Services
{
    public static class ServiceConstants
    {
        public static void ValidateId(Guid id, string idName)
        {
            if (CheckGuidValue(id))
            {
                throw new ValidationException($"Invalid {idName}");
            }
        }

        public static bool CheckGuidValue(Guid? guid)
        {
            return (!guid.HasValue || guid.Value == Guid.Empty);
        }
    }
}
