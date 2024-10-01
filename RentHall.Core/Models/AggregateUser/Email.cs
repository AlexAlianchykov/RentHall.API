using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace RentHall.Core.Models.AggregateUser
{
    public class Email: ValueObject
    {
        private Email(string email)
        {
            EmailUser = email;
        }
        public string EmailUser { get; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>("Email cannot be empty");
            }
            else if (!IsValidEmail(email))
            {
                return Result.Failure<Email>("Invalid email format");
            }

            var newEmail = new Email(email);
            return Result.Success(newEmail);
        }

        private static bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return EmailUser;
        }
    }
}
