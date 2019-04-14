namespace BandEr.Infrastructure.Auth.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string
                    Rol = "rol",
                    Id = "id";
            }

            public static class JwtClaims
            {
                public const string UserAccess = "user_access";
                public const string ApiKeyAccess = "apikey_access";
            }
        }
    }
}