using Contracts.Interfaces;
using Contracts.Models;

namespace RiskEngineCore
{
    public sealed class RiskContextFactory(IDatabase database)
    {
        public async Task<RiskContext> CreateRiskContext(Transaction transaction, CancellationToken cancellationToken)
        {
            var deviceHistory = database.GetDeviceHistory(transaction.UserId, cancellationToken);
            var locationHistory = database.GetLocationHistory(transaction.UserId, cancellationToken);
            await Task.WhenAll(deviceHistory, locationHistory);
            return new RiskContext
            {
                Transaction = transaction,
                DeviceHistory = await deviceHistory,
                LocationHistory = await locationHistory
            };
        }
    }
}
