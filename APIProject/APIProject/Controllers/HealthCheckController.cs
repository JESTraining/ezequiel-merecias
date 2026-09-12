using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;

namespace APIProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService healthCheckService;

        public HealthCheckController(HealthCheckService _healthCheckService)
        {
            healthCheckService = _healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var report = await healthCheckService.CheckHealthAsync(cancellationToken);

            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(x => new
                {
                    name = x.Key,
                    status = x.Value.Status.ToString(),
                    description = x.Value.Description,
                    error = x.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy ? Ok(response)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
        }
    }
}
