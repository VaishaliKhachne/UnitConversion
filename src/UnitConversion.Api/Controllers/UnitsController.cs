using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Contracts;
using UnitConversion.Application.Catalog;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/v1/units")]
public sealed class UnitsController : ControllerBase
{
    /// <summary>Lists every supported unit, grouped implicitly by category.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UnitResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var units = UnitCatalog.All.Select(u => new UnitResponse
        {
            Code = u.Code,
            DisplayName = u.DisplayName,
            Category = u.Category.ToString()
        });

        return Ok(units);
    }
}