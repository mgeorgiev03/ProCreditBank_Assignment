using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private DatabaseContext context;
        private readonly ILogger<APIController> logger;
        
        public APIController(DatabaseContext _context, ILogger<APIController> _logger)
        {
            context = _context;
            logger = _logger;
        }

        [HttpGet] 
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                using (StreamReader reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                {
                    string content = await reader.ReadToEndAsync();
                    content = content.Replace("}", string.Empty);
                    MT799 mt799 = new MT799();

                    List<string> list = new List<string>();
                    list = content.Split('{', StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<string> blocks = new List<string>();

                    blocks = list[2].Split("\n:").ToList();

                    mt799.Block1 = blocks[1];
                    mt799.Block2 = blocks[2];
                    mt799.Block3 = blocks[3];

                    mt799.Id = await context.GetCount() + 1;

                    await context.InsertMessage(mt799);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                //logger.Log(LogLevel.Error, ex, ex.Message);
                logger.LogError(ex, ex.Message);

                return BadRequest();
            }

        }
    }
}
