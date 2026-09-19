

namespace Contracts.RiskEngine.Models
{
    public class RiskAssessmentResult
    {
        public Guid TransactionId { get; private set; }
        public int Rating { get; private set; }
        public List<string> Reasons { get; private set; }
        public RiskAssessmentResult(Guid transactionId, int rating, List<string> reasons)
        {
            TransactionId = transactionId;
            Rating = rating;
            Reasons = reasons;
        }
    }
}
