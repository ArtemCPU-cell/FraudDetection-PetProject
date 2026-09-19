using Contracts.Models;
using Contracts.RiskEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Interfaces
{
    public interface IRiskEngine
    {
        public Task<RiskAssessmentResult> TransactionCheck(Transaction transaction, CancellationToken cancellationToken);
    }
}
