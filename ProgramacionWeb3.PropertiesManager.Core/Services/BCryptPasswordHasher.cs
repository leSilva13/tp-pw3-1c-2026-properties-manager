namespace ProgramacionWeb3.PropertiesManager.Core.Services
{
    internal static class BCryptPasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public static bool Verify(string password, string hashed)
        {
            try
            {
                return BCrypt.Net.BCrypt.EnhancedVerify(password, hashed);
            }
            catch
            {
                return false;
            }
        }
    }
}
