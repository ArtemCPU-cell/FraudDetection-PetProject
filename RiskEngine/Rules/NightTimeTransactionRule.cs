using Contracts.Models;
using Contracts.Interfaces;

namespace RiskEngineCore.Rules
{
    public class NightTimeTransactionRule : IRiskRule
    {
        private const int NIGHT_TIME_RISK_SCORE = 20;
        public RiskRuleResult Evaluate(RiskContext riskContext)
        {
            if (riskContext.Transaction.CreatedAt.AddHours(3).Hour < 6 || riskContext.Transaction.CreatedAt.AddHours(3).Hour >= 22)
                return new RiskRuleResult("Transaction occurred during night hours", NIGHT_TIME_RISK_SCORE);
            return new RiskRuleResult(null, 0);
        }
    }
}
