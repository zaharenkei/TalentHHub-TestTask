using Microsoft.Extensions.Options;
using TalentHHub_TestTask.Handlers.Models;
using TalentHHub_TestTask.Infrastructure.Settings;

namespace TalentHHub_TestTask.Handlers
{
    /// <summary>
    /// Query that calculates the total charge for a job.
    /// </summary>
    public class CalculateTotalChargeQuery : IHandler<CalculationRequest, CalculationResponse>
    {
        private readonly PriceSettings priceSettings;
        private readonly ILogger<CalculateTotalChargeQuery> logger;

        public CalculateTotalChargeQuery(
            IOptions<PriceSettings> settings,
            ILogger<CalculateTotalChargeQuery> logger)
        {
            priceSettings = settings.Value;
            this.logger = logger;
        }

        public ValueTask<CalculationResponse> Handle(CalculationRequest request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var responseItems = new List<ResponsePrintItem>();
            var totalSum = 0m;
            foreach (var requestItem in request.Items)
            {
                logger.LogDebug("Start processing item {Name}. [SalesTax: {IsSalesTaxApplied}] [ExtraMargin: {IsExtraMarginApplied}]"
                    , requestItem.Name, !requestItem.Exempt, request.ExtraMargin);

                var marginMultiplier = (priceSettings.Margin + (request.ExtraMargin ? priceSettings.ExtraMargin : 0)) / 100m;
                var itemMargin = requestItem.Cost * marginMultiplier;

                var taxMultiplier = (requestItem.Exempt ? 0 : priceSettings.SalesTax) / 100m;
                var itemTaxes = requestItem.Cost * taxMultiplier;

                totalSum += requestItem.Cost + itemMargin + itemTaxes;

                logger.LogDebug("Item {Name} will be processed with: [BaseCost: {Cost:C}][Tax: {Tax:C}][Margin: {Margin:C}]"
                    , requestItem.Name, requestItem.Cost, itemTaxes, itemMargin);

                responseItems.Add(new ResponsePrintItem(requestItem.Name, $"{requestItem.Cost + itemTaxes:C}"));
            }

            totalSum = (0.02m / 1.00m) * decimal.Round(totalSum * (1.00m / 0.02m)); // round to the nearest even cent

            return ValueTask.FromResult(new CalculationResponse($"{totalSum:C}", responseItems));
        }
    }
}