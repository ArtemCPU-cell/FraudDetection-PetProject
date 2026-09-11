using System.Globalization;

namespace FraudDetectionAPI.Repositories
{
    public class Transaction
    {
        public Transaction(decimal amount, string currency, string? deviceid, string country)
        {
            Amount = amount;
            Currency = currency;
            DeviceId = deviceid;
            TransactionId = Guid.NewGuid().ToString();
            Country = country;
        }

        public decimal Amount { get; }
        public string Currency { get; }
        public string? DeviceId { get; }
        public string TransactionId { get; }
        public string Country { get; }
    }
}
