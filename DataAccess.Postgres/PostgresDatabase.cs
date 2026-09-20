using Contracts.Interfaces;
using Contracts.Models;
using Contracts.RiskEngine.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Postgres
{
    public class PostgresDatabase : IDatabase
    {
        private readonly IDbContextFactory<PostgresDbContext> factory;
        public PostgresDatabase(IDbContextFactory<PostgresDbContext> factory)
        {
            this.factory = factory;
        }

        public async Task<IEnumerable<string>> GetDeviceHistory(string userId, CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            return await db.Transactions
                .Where(x => x.UserId == userId)
                .Where(x => x.DeviceId != null)
                .Select(x => x.DeviceId!)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetLocationHistory(string userId, CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            return await db.Transactions
                .Where(x => x.UserId == userId)
                .Where(x => x.Country != null)
                .Select(x => x.Country!)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
        public async Task<Transaction> GetTransaction(Guid transactionId, CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            return await db.Transactions.FindAsync(new object[] { transactionId }, cancellationToken) ?? throw new Exception($"Transaction with ID {transactionId} not found.");
        }

        public async Task SaveTransaction(Transaction transaction, CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            await db.Transactions.AddAsync(transaction, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveRiskAssessmentResult(RiskAssessmentResult result, CancellationToken cancellationToken)
        {
            await using var db = await factory.CreateDbContextAsync(cancellationToken);

            await db.RiskAssessmentResults.AddAsync(result, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
