using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace RentHall.Core.Models.AggregateUser
{
    public class User
    {
        private User(
            Guid id,
            string name,
            Email email,
            string hashPassword,
            string role,
            DateTime createDate)
        {
            Id = id;
            Name = name;
            Email = email;
            HashPassword = hashPassword;
            Role = role;
            CreateDate = createDate;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Email Email { get; }
        public string HashPassword { get; }
        public string Role {  get; }
        public DateTime CreateDate { get; }

        public static Result<User> Create( 
            Guid id,
            string name,
            Email email,
            string hashPassword,
            string role,
            DateTime createDate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<User>("Name cannot be empty");
            }
            else if (name.Length < 3 || name.Length > 50)
            {
                return Result.Failure<User>("Very long/short name");
            }
            else if (!IsValidName(name))
            {
                return Result.Failure<User>("The name contains invalid characters");
            }

            var newUser = new User(id, name, email, hashPassword, role, createDate);
            return Result.Success(newUser);
        }

        private static bool IsValidName(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9]+(\s[a-zA-Zа-яА-ЯёЁ0-9]+)*$");
            return regex.IsMatch(name);
        }
    }
}
