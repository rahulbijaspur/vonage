using System.Text.Json;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionMonitorController : ControllerBase
    {
        private readonly DataContext context;
        public SessionMonitorController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost("created")]
        public async Task<ActionResult> created([FromBody]JsonElement data)
        {
            if (data.ToString().Length>0)
            {
                var condata = new ConnectionCreated();
                condata.connectiondata=data.ToString();
                await context.AddAsync(condata);
                await context.SaveChangesAsync();
                return Ok("Connection data added");

            }else{
                return BadRequest("Unsuccessful");
            }
        }
    }
}