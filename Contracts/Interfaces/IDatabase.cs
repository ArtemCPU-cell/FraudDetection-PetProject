using Contracts.Models;
using Contracts.RiskEngine.Models;

namespace Contracts.Interfaces
{
    public interface IDatabase
    {
        Task SaveTransaction(Transaction transaction, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetDeviceHistory(string userId, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetLocationHistory(string userId, CancellationToken cancellationToken);
        Task<Transaction> GetTransaction(Guid transactionId, CancellationToken cancellationToken);
        Task SaveRiskAssessmentResult(RiskAssessmentResult result, CancellationToken cancellationToken);
    }
}
