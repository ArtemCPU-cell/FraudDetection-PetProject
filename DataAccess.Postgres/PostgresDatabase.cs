using Contracts.Interfaces;
using Contracts.Models;
using Contracts.RiskEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postgres
{
    public class PostgresDatabase : DbContext, IDatabase
    {
        public PostgresDatabase(DbContextOptions<PostgresDatabase> options)
        : base(options)
        {
        }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<RiskAssessmentResult> RiskAssessmentResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RiskAssessmentResult>()
                .HasKey(x => x.TransactionId);

            modelBuilder.Entity<RiskAssessmentResult>()
                .Property(x => x.Reasons)
                .HasColumnType("jsonb");
        }


        public async Task<IEnumerable<string>> GetDeviceHistory(string userId, CancellationToken cancellationToken)
        {
            List<string> devices = [];
            await Transactions.ForEachAsync(transaction =>
            {
                if (transaction.DeviceId is not null && !devices.Contains(transaction.DeviceId))
                {
                    devices.Add(transaction.DeviceId);
                }
            }, cancellationToken);
            return devices;
        }

        public async Task<IEnumerable<string>> GetLocationHistory(string userId, CancellationToken cancellationToken)
        {
            List<string> locations = [];
            await Transactions.ForEachAsync(transaction =>
            {
                if (transaction.Country is not null && !locations.Contains(transaction.Country))
                {
                    locations.Add(transaction.Country);
                }
            }, cancellationToken);
            return locations;
        }
        public async Task<Transaction> GetTransaction(Guid transactionId, CancellationToken cancellationToken)
        {
            return await Transactions.FindAsync(new object[] { transactionId }, cancellationToken) ?? throw new Exception($"Transaction with ID {transactionId} not found.");
        }

        public async Task SaveTransaction(Transaction transaction, CancellationToken cancellationToken)
        {
            await Transactions.AddAsync(transaction, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task SaveRiskAssessmentResult(RiskAssessmentResult result, CancellationToken cancellationToken)
        {
            await RiskAssessmentResults.AddAsync(result, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }
    }
}
