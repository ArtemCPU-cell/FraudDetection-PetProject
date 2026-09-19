using Contracts.Interfaces;
using Contracts.Models;


namespace RiskEngineCore.Rules
{
    public class NewCountryRule : IRiskRule
    {
        private const int NEW_COUNTRY_RISK_SCORE = 40;
        public RiskRuleResult Evaluate(RiskContext riskContext)
        {
            if (!riskContext.LocationHistory.Contains(riskContext.Transaction.Country))
                return new RiskRuleResult("New country detected", NEW_COUNTRY_RISK_SCORE);
            return new RiskRuleResult(null, 0);
        }
    }
}

