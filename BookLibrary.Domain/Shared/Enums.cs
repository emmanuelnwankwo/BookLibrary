namespace BookLibrary.Domain.Shared
{
    public static class Enums
    {
        public enum BookStatus
        {
            Available,
            Reserved,
            Borrowed,
            Return
        }

        public enum BookGenre
        {
            Finance,
            Fiction,
            Business,
            War
        }

        public enum OrderBy
        {
            Ascending,
            Descending
        }

        public enum UserRole
        {
            User,
            Admin
        }
    }
}
