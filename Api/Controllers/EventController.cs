using Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IServiceAggregator _serviceAggregator;
        public EventController(IServiceAggregator serviceAggregator)
        {
            _serviceAggregator = serviceAggregator;
        }
        [HttpGet("consume")]
        public async Task<IActionResult> ConsumeEvent(CancellationToken cancellationToken)
        {
            await _serviceAggregator.MessageConsumer
                .ConsumeAsync("class-created", "class-service-group", cancellationToken);

            return Ok("Consumer started");
        }
    }
}
