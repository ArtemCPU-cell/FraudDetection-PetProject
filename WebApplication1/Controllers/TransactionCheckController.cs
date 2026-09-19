using Microsoft.AspNetCore.Mvc;
using Contracts.Models;
using System.Text.Json;
using Contracts.Interfaces;

namespace FraudDetectionAPI.Controllers
{
    [Route("transaction/check")]
    [ApiController]
    public class TransactionCheckController : ControllerBase
    {
        private readonly IRiskEngine riskEngine;
        private readonly IDatabase database;

        public TransactionCheckController(IRiskEngine riskEngine, IDatabase database)
        {
            this.riskEngine = riskEngine;
            this.database = database;
        }

        [HttpPost]
        public async Task<IActionResult> PostCheckTransaction([FromBody] Transaction transaction, CancellationToken cancellationToken)
        {
            await database.SaveTransaction(transaction, cancellationToken);
            return Ok(JsonSerializer.Serialize(await riskEngine.TransactionCheck(transaction, cancellationToken)));
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionCheck([FromQuery] Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction = await database.GetTransaction(transactionId, cancellationToken);
            return Ok(JsonSerializer.Serialize(transaction));
        }
    }
}
