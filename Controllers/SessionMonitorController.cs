using System.Text.Json;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Mvc;

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
            
            string str = data.ToString();
            if (str.ToString().Length>0)
            {
                var condata = new ConnectionCreated();
                condata.ConnectionCreatedData=data.ToString();
                await context.AddAsync(condata);
                await context.SaveChangesAsync();
                return Ok("Connection data added");

            }else{
                return BadRequest("Unsuccessful");
            }
        }
        [HttpPost("destroyed")]
        public async Task<ActionResult> destroyed([FromBody] JsonElement data)
        {
            string str = data.ToString();
            if (str.Length>1){
                var desdata = new ConnectionDestroyed();
                desdata.ConnectionDestroyedData= str;
                await context.AddAsync(desdata);
                await context.SaveChangesAsync();
                return Ok("connnection destroyed data added");
            }else{
                return BadRequest("Unsuccessful");
            }
        }
    }
}