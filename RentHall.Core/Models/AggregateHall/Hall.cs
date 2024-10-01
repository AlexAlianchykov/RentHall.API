using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateUser;
using System;
using System.Text.RegularExpressions;

namespace RentHall.Core.Models.AggregateHall
{
    public class Hall
    {
        private Hall(
            Guid id,
            string name,
            int capacity,
            Price price,
            DateTime dateTime)
        {
            Id = id;
            Name = name;
            Сapacity = capacity;
            Price = price;
            CreateDate = dateTime;
        }

        public Guid Id { get;}
        public string Name { get;}
        public int Сapacity { get;}
        public Price Price { get;}
        public DateTime CreateDate { get;}

        public static Result<Hall> Create(
            Guid id,
            string name,
            int capacity,
            Price price,
            DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Hall>("Name cannot be empty");
            }
            else if (name.Length < 3 || name.Length > 30)
            {
                return Result.Failure<Hall>("Very long/short name");
            }
            else if (!IsValidName(name))
            {
                return Result.Failure<Hall>("The name contains invalid characters");
            }
            
            if (capacity < 0)
            {
                return Result.Failure<Hall>("The capacity of the hall is indicated incorrectly");
            }

            var newHall = new Hall(id, name, capacity, price, dateTime);
            return Result.Success(newHall);
        }
        private static bool IsValidName(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9]+(\s[a-zA-Zа-яА-ЯёЁ0-9]+)*$");
            return regex.IsMatch(name);
        }
    }
}
