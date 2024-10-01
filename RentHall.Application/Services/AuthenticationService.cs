using CSharpFunctionalExtensions;
using RentHall.Application.AbstractionsServices;
using RentHall.Core.AbstractionsRepositories.UserAbstractions;
using RentHall.Core.Models.AggregateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentHall.Application.Services
{
    public class AuthenticationService
    {
        private IUserService _userService;
        private IPasswordHash _passwordHash;
        private IJwtProvider _jwtProvider;
        public AuthenticationService(
            IUserService userService,
            IPasswordHash passwordHash,
            IJwtProvider jwtProvider)
        {
            _userService = userService;
            _passwordHash = passwordHash;
            _jwtProvider = jwtProvider;
        }

        public Result<string> Login(string email, string password)
        {
            try
            {
                var newEmail = Email.Create(email).Value;
                var user = _userService.GetUser(newEmail.EmailUser).Value; // находим юзера 

                var result = _passwordHash.Verify(password, user.HashPassword); // проверяем правильный ли пароль 

                if (result == false)
                {
                    return Result.Failure<string>("Password or Email is incorrect");
                }

                var token = _jwtProvider.GenerateToken(user); // создаём токен

                return Result.Success(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
