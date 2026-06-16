using System.Security.Claims;

namespace ProgramacionWeb3.PropertiesManager.Web.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            string userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }

            return 0;
        }
    }
}
