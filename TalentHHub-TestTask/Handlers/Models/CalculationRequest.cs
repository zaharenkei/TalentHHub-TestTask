namespace TalentHHub_TestTask.Handlers.Models;

public record CalculationRequest(bool ExtraMargin, IEnumerable<RequestPrintItem> Items);