
using Contracts.Interfaces;
using Contracts.Models;
using Contracts.RiskEngine.Models;

namespace RiskEngineCore
{
    public class RiskEngine(IEnumerable<IRiskRule> riskRules, IDatabase database, RiskContextFactory riskContextFactory) : IRiskEngine
    {
        private readonly List<IRiskRule> RiskRules = riskRules.ToList();
        private readonly IDatabase Database = database;
        private readonly RiskContextFactory RiskContextFactory = riskContextFactory;

        async public Task<RiskAssessmentResult> TransactionCheck(Transaction transaction, CancellationToken cancellationToken)
        {
            var riskContext = await RiskContextFactory.CreateRiskContext(transaction, cancellationToken);
            int rating = 0;
            List<string> reasons = new List<string>();
            foreach (IRiskRule rule in RiskRules)
            {
                var result = rule.Evaluate(riskContext);
                rating += result.TotalRiskScore;
                if (result.ViolatedRule != null)
                {
                    reasons.Add(result.ViolatedRule);
                }
            }
            var assessmentResult = new RiskAssessmentResult(transaction.TransactionId, rating, reasons);
            await Database.SaveRiskAssessmentResult(assessmentResult, cancellationToken);
            return assessmentResult;
        }
    }
}
