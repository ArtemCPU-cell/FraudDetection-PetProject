

namespace Contracts.Models
{
    public class RiskContext()
    {
        public required Transaction Transaction { get; init; }
        public IEnumerable<string> DeviceHistory { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<string> LocationHistory { get; init; } = Enumerable.Empty<string>();
    }
}
