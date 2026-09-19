using Contracts.Models;
using Contracts.Interfaces;

namespace RiskEngineCore.Rules
{
    public class LargeAmountRule : IRiskRule
    {
        private const decimal THRESHOLD = 100000m;
        private const int BASE_RISK_SCORE = 25;
        public RiskRuleResult Evaluate(RiskContext riskContext)
        {
            var transaction = riskContext.Transaction;
            if (transaction.Amount > THRESHOLD)
                return new RiskRuleResult("Large amount detected", (int)(BASE_RISK_SCORE + (transaction.Amount / THRESHOLD) * 5));
            return new RiskRuleResult(null, 0);
        }
    }
}
