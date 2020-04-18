using MheanMaa.Enums;
using System.Linq;
using System.Security.Claims;

namespace MheanMaa.Utils
{
    public class ClaimSearch
    {
        public static string GetClaim(ClaimsPrincipal user, ClaimEnum type)
        {
            return user.Claims.Where(claim => claim.Type == type.ToString()).FirstOrDefault().Value;
        }
    }
}
