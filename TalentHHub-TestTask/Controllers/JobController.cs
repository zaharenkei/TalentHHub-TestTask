using Microsoft.AspNetCore.Mvc;
using TalentHHub_TestTask.Handlers;
using TalentHHub_TestTask.Handlers.Models;

namespace TalentHHub_TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IHandler<CalculationRequest, CalculationResponse> calculateTotalChargeHandler;

        public JobController(IHandler<CalculationRequest, CalculationResponse> calculateTotalChargeHandler)
        {
            this.calculateTotalChargeHandler = calculateTotalChargeHandler;
        }

        /// <summary>
        /// Calculates the total charge for job with exact pricing for every job item.
        /// </summary>
        /// <param name="request">Job data with raw prices to be processed.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Total price together with updated job items.</returns>
        [HttpPost, Route("totalCharge")]
        public async Task<CalculationResponse> CalculateTotalCharge(CalculationRequest request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await calculateTotalChargeHandler.Handle(request, token);
        }
    }
}