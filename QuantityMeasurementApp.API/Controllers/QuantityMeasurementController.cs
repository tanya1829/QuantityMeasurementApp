using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/quantities")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService            _service;
        private readonly ILogger<QuantityMeasurementController> _logger;

        public QuantityMeasurementController(
            IQuantityMeasurementService            service,
            ILogger<QuantityMeasurementController> logger)
        {
            _service = service;
            _logger  = logger;
        }

        /// <summary>Compare two quantities</summary>
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("Compare operation requested");
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            bool result = _service.Compare(input.ThisQuantityDTO, input.ThatQuantityDTO);
            _logger.LogInformation("Compare result: {Result}", result);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "compare", resultString: result.ToString().ToLower()));
        }

        /// <summary>Convert first quantity to unit of second</summary>
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("Convert operation requested");
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Convert(input.ThisQuantityDTO, input.ThatQuantityDTO.Unit!);
            _logger.LogInformation("Convert result: {Value} {Unit}", result.Value, result.Unit);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation:   "convert",
                resultValue: result.Value,
                resultUnit:  result.Unit?.ToString()));
        }

        /// <summary>Add two quantities</summary>
        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("Add operation requested");
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Add(
                input.ThisQuantityDTO,
                input.ThatQuantityDTO,
                input.ThisQuantityDTO.Unit!);
            _logger.LogInformation("Add result: {Value} {Unit}", result.Value, result.Unit);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation:   "add",
                resultValue: result.Value,
                resultUnit:  result.Unit?.ToString()));
        }

        /// <summary>Subtract second quantity from first</summary>
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("Subtract operation requested");
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Subtract(input.ThisQuantityDTO, input.ThatQuantityDTO);
            _logger.LogInformation("Subtract result: {Value} {Unit}", result.Value, result.Unit);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation:   "subtract",
                resultValue: result.Value,
                resultUnit:  result.Unit?.ToString()));
        }

        /// <summary>Divide first quantity by second</summary>
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation("Divide operation requested");
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            double result = _service.Divide(input.ThisQuantityDTO, input.ThatQuantityDTO);
            _logger.LogInformation("Divide result: {Result}", result);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation:   "divide",
                resultValue: result));
        }

        /// <summary>Get all operation history</summary>
        [HttpGet("history")]
        public IActionResult GetAllHistory()
        {
            _logger.LogInformation("Get all history requested");
            var records = _service.GetAllMeasurements();
            _logger.LogInformation("Returning {Count} records", records.Count);
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get history by operation type</summary>
        [HttpGet("history/operation/{operation}")]
        public IActionResult GetByOperation(string operation)
        {
            _logger.LogInformation("Get history by operation: {Operation}", operation);
            var records = _service.GetByOperation(operation.ToUpper());
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get history by measurement type</summary>
        [HttpGet("history/type/{measureType}")]
        public IActionResult GetByMeasureType(string measureType)
        {
            _logger.LogInformation("Get history by type: {MeasureType}", measureType);
            var records = _service.GetByMeasureType(measureType.ToUpper());
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get total record count</summary>
        [HttpGet("count")]
        public IActionResult GetCount()
        {
            var count = _service.GetTotalCount();
            _logger.LogInformation("Total record count: {Count}", count);
            return Ok(new { count });
        }

        /// <summary>Delete all records — Admin only</summary>
        [HttpDelete("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAll()
        {
            _logger.LogWarning("Delete all records requested by: {User}", User.Identity?.Name);
            _service.DeleteAll();
            return NoContent();
        }
    }
}
