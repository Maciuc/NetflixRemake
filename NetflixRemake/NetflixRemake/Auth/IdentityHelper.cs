using System.Security.Claims;

namespace Backend.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserID(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public static Claim GetUser(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier);
        }

        public static string GetUserTenant(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("tenant");
        }

        public static string GetRole(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Role);
        }

        public static List<string> GetRoles(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
