using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class RateController
{
    [HttpGet("rates")]
    public async Task<IActionResult> GetRatesAsync()
    {
        var twte = await _baseRepository.GetRatesByExpressionAsync(r => r.IsPrivate == false);
        Console.WriteLine(twte);
        return new OkObjectResult(twte);
    }
}