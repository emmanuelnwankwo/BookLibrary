namespace BookLibrary.Domain.Shared
{
    public class GeneralConfig
    {
        public JWT Jwt { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int TokenExpireTimeInSeconds { get; set; }
    }
}