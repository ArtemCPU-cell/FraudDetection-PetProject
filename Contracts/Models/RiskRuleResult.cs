
namespace Contracts.Models
{
    public class RiskRuleResult
    {
        public string? ViolatedRule { get; private set; } = null;
        public int TotalRiskScore { get; private set; } = 0;

        public RiskRuleResult(string? violatedRule, int totalRiskScore)
        {
            ViolatedRule = violatedRule;
            TotalRiskScore = totalRiskScore;
        }
    }
}