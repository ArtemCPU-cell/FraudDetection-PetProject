
namespace Contracts.Models
{
    public class Transaction
    {
        private Transaction()
        {
        }

        public Transaction(
            string userId,
            decimal amount,
            string currency,
            string? deviceId,
            string country)
        {
            TransactionId = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            DeviceId = deviceId;
            UserId = userId;
            Country = country;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid TransactionId { get; private set; }
        public string UserId { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = null!;
        public string? DeviceId { get; private set; }
        public string Country { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
    }
}
