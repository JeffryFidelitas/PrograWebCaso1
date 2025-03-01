using System.Linq;
using System.Security.Claims;

namespace Caso1.Core.Helpers
{
    public static class ManageClaims
    {
        public static bool TieneRol(this ClaimsPrincipal user, params string[] roles)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return false;

            return roles.Any(rol => user.IsInRole(rol));
        }
    }
}
