using Contracts.Models;
using Contracts.Interfaces;

namespace RiskEngineCore.Rules
{
    public class NewDeviceRule : IRiskRule
    {
        private const int NEW_DEVICE_RISK_SCORE = 30;
        public RiskRuleResult Evaluate(RiskContext riskContext)
        {
            if (!riskContext.DeviceHistory.Contains(riskContext.Transaction.DeviceId))
                return new RiskRuleResult("New device detected", NEW_DEVICE_RISK_SCORE);
            return new RiskRuleResult(null, 0);
        }
    }
}
