using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private DatabaseContext context;
        public APIController(DatabaseContext _context)
        {
            context = _context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, World!");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (StreamReader reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                {
                    string content = await reader.ReadToEndAsync();
                    MT799 mt799 = new MT799();
                    mt799.Message = content;


                    //get last message what if its the first message?
                    mt799.Id = await context.GetCount() + 1;

                    await context.InsertMessage(mt799);
                }

                return Ok();
            }

            else
                return BadRequest();

        }
    }
}
