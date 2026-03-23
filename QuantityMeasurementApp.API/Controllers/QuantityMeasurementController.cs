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
        private readonly IQuantityMeasurementService _service;
        private readonly ILogger<QuantityMeasurementController> _logger;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            ILogger<QuantityMeasurementController> logger)
        {
            _service = service;
            _logger  = logger;
        }

        /// <summary>Compare two quantities</summary>
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            bool result = _service.Compare(
                input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "compare", resultString: result.ToString().ToLower()));
        }

        /// <summary>Convert first quantity to unit of second</summary>
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Convert(
                input.ThisQuantityDTO, input.ThatQuantityDTO.Unit!);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "convert",
                resultValue: result.Value,
                resultUnit: result.Unit?.ToString()));
        }

        /// <summary>Add two quantities</summary>
        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityInputDTO input)
        {
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Add(
                input.ThisQuantityDTO,
                input.ThatQuantityDTO,
                input.ThisQuantityDTO.Unit!);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "add",
                resultValue: result.Value,
                resultUnit: result.Unit?.ToString()));
        }

        /// <summary>Subtract second quantity from first</summary>
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityInputDTO input)
        {
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            var result = _service.Subtract(
                input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "subtract",
                resultValue: result.Value,
                resultUnit: result.Unit?.ToString()));
        }

        /// <summary>Divide first quantity by second</summary>
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            input.ThisQuantityDTO.ResolveUnit();
            input.ThatQuantityDTO.ResolveUnit();
            double result = _service.Divide(
                input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(QuantityMeasurementDTO.FromOperation(
                input.ThisQuantityDTO, input.ThatQuantityDTO,
                operation: "divide",
                resultValue: result));
        }

        /// <summary>Get all operation history</summary>
        [HttpGet("history")]
        public IActionResult GetAllHistory()
        {
            var records = _service.GetAllMeasurements();
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get history by operation type</summary>
        [HttpGet("history/operation/{operation}")]
        public IActionResult GetByOperation(string operation)
        {
            var records = _service.GetByOperation(operation.ToUpper());
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get history by measurement type</summary>
        [HttpGet("history/type/{measureType}")]
        public IActionResult GetByMeasureType(string measureType)
        {
            var records = _service.GetByMeasureType(measureType.ToUpper());
            return Ok(QuantityMeasurementDTO.FromEntityList(records));
        }

        /// <summary>Get total record count</summary>
        [HttpGet("count")]
        public IActionResult GetCount()
        {
            return Ok(new { count = _service.GetTotalCount() });
        }

        /// <summary>Delete all records — Admin only</summary>
        [HttpDelete("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAll()
        {
            _service.DeleteAll();
            return NoContent();
        }
    }
}
