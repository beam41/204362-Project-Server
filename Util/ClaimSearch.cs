using MheanMaa.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MheanMaa.Util
{
    public class ClaimSearch
    {
        public static string GetClaim(ClaimsPrincipal user, ClaimEnum type)
        {
            return user.Claims.Where(claim => claim.Type == type.ToString()).FirstOrDefault().Value;
        }
    }
}
