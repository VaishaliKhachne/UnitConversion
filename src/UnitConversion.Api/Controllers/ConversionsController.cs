using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Contracts;
using UnitConversion.Application.Services;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/v1/conversions")]
public sealed class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public ConversionsController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    /// <summary>Converts a numeric value from one unit to another.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Convert([FromBody] ConversionRequest request)
    {
        var result = _conversionService.Convert(request.From, request.To, request.Value);

        return Ok(new ConversionResponse
        {
            From = request.From,
            To = request.To,
            InputValue = request.Value,
            ConvertedValue = result
        });
    }
}