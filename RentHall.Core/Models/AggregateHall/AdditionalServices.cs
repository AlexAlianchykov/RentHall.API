using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateUser;
using System.Text.RegularExpressions;

namespace RentHall.Core.Models.AggregateHall
{
    public class AdditionalServices
    {
        private AdditionalServices(Guid id, string name, Price price, DateTime dateTime)
        {
            Id = id;
            Name = name;
            Price = price;
            CreateDate = dateTime;
        }
        public Guid Id { get; }
        public string Name { get; }
        public Price Price { get; }
        public DateTime CreateDate { get; }

        public static Result<AdditionalServices> Create(
            Guid id,
            string name,
            Price price,
            DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<AdditionalServices>("Name cannot be empty");
            }
            else if (name.Length < 3 || name.Length > 15)
            {
                return Result.Failure<AdditionalServices>("Very long/short name");
            }
            else if (!IsValidName(name))
            {
                return Result.Failure<AdditionalServices>("The name contains invalid characters");
            }

            var newServices = new AdditionalServices(id, name, price, dateTime);
            return Result.Success(newServices);
        }
        private static bool IsValidName(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9]+(\s[a-zA-Zа-яА-ЯёЁ0-9]+)*$");
            return regex.IsMatch(name);
        }
    }
}
