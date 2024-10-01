using CSharpFunctionalExtensions;

namespace RentHall.Core.Models.AggregateHall
{
    public class Price : ValueObject
    {
        private static readonly List<string> ValidHryvniaCodes = new List<string> { "UAH", "ГРН", "ГРИВНА", "HRYVNIA", "ГРИВЕН", "ГРИВЕНЬ" };
        private Price(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; }
        public string Currency { get; }

        public static Result<Price> Create(decimal amount, string currency)
        {
            currency.ToUpper();

            if (amount < 0)
            {
                return Result.Failure<Price>("The amount of money cannot be negative");
            }
            else if (string.IsNullOrWhiteSpace(currency))
            {
                return Result.Failure<Price>("Currency cannot be empty");
            }
            else if (!ValidHryvniaCodes.Contains(currency))
            {
                return Result.Failure<Price>("This currency is not used");
            }

            var newPrice = new Price(amount, currency);
            return Result.Success(newPrice);
        }

        public Price Add(Price other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Money in different currencies");
            }

            return new Price(Amount + other.Amount, Currency);
        }
        
        public Price Subtract(Price other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Money in different currencies");
            }

            if (Amount < other.Amount)
            {
                throw new InvalidOperationException("The result is negative");
            }

            return new Price(Amount - other.Amount, Currency);
        }

        public Price Multiply(decimal multiplier)
        {
            if (multiplier < 0)
            {
                throw new ArgumentException("The multiplier cannot be negative");
            }

            return new Price(Amount * multiplier, Currency);
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
