using MheanMaa.Enum;
using MheanMaa.Models;
using MheanMaa.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MheanMaa.Services
{
    public class AuthenticationService
    {
        private readonly IUserSettings _userSettings;
        private readonly UserService _userService;

        public AuthenticationService(IUserSettings userSettings, UserService userService)
        {
            _userSettings = userSettings;
            _userService = userService;
        }

        public UserReturn Authenticate(string username, string password)
        {
            User user = _userService.Find(username, password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_userSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimEnum.Id.ToString(), user.Id),
                    new Claim(ClaimEnum.UserType.ToString(), user.UserType),
                    new Claim(ClaimEnum.DeptNo.ToString(), user.DeptNo.ToString()),
                    new Claim(ClaimEnum.FirstName.ToString(), user.FirstName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            UserReturn userReturn = (UserReturn) user;
            userReturn.Token = tokenHandler.WriteToken(token);

            return userReturn;
        }

    }
}
