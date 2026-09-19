using Contracts.Interfaces;
using Contracts.Models;

namespace RiskEngineCore
{
    public sealed class RiskContextFactory(IDatabase database)
    {
        public async Task<RiskContext> CreateRiskContext(Transaction transaction, CancellationToken cancellationToken)
        {
            return new RiskContext
            {
                Transaction = transaction,
                DeviceHistory = await database.GetDeviceHistory(transaction.UserId, cancellationToken),
                LocationHistory = await database.GetLocationHistory(transaction.UserId, cancellationToken)
            };
        }
    }
}
