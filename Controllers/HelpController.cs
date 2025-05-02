using Microsoft.AspNetCore.Mvc;
using AiAssistantApi.Models;
using AiAssistantApi.Services;

namespace AiAssistantApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpController : ControllerBase
    {
        private readonly HelpRouter _router;

        public HelpController()
        {
            _router = new HelpRouter();
        }

        [HttpGet("start")]
        public IActionResult StartHelp([FromQuery] HelpType helpType)
        {
            try
            {
                var result = _router.StartWorkflow(helpType);
                return Ok(result);
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, $"[TODO] {ex.Message}");
            }
        }
    }
}
