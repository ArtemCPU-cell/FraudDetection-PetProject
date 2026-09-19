using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IRiskRule
    {
        RiskRuleResult Evaluate(RiskContext riskContext);
    }
}
