namespace TalentHHub_TestTask.Handlers.Models;

public record CalculationResponse(string Total, IEnumerable<ResponsePrintItem> Items);