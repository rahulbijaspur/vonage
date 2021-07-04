using System;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTokSDK;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly DataContext _context;
        public IOptions<TokboxSettings> Config { get; }
        public Session Session { get; protected set; }
        public OpenTok OpenTok { get; protected set; }
        private readonly IOptions<TokboxSettings> _config;

        public SessionController(DataContext context, IOptions<TokboxSettings> config)
        {
            _config = config;

            _context = context;
        }

        
        [HttpPost("CreateSession")]
        public async Task<ActionResult<string>> CreateSession(string username, string location = "", MediaMode mediaMode = MediaMode.ROUTED, ArchiveMode archiveMode = ArchiveMode.ALWAYS)
        {
            User user = _context.User.SingleOrDefault(x => x.Username == username);
            if (user == null)
            {
                return BadRequest("user not recognized");
            }
            TokboxSession s = new TokboxSession();
            OpenTokService();
            string sessionId = OpenTok.CreateSession(location, mediaMode, archiveMode).Id;
            s.SessionId = sessionId;
            s.CreatedBy = user;
            if (mediaMode == MediaMode.ROUTED)
            {
                s.isArchive = true;
            }
            else
            {
                s.isArchive = false;
            }
            await _context.AddAsync(s);
            await _context.SaveChangesAsync();
            return sessionId;

        }
        [HttpPost("GetToken")]
        public async Task<ActionResult<string>> GetToken(string username, string sessionId, Role role = Role.PUBLISHER)
        {
            if (!UserExists(username).Result)
            {
                return BadRequest("Not registered");
            }
            double inOneWeek = (DateTime.UtcNow.Add(TimeSpan.FromDays(7)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            OpenTokService();
            string token = OpenTok.GenerateToken(sessionId, role, expireTime: inOneWeek);
            TokboxTokens tok = new TokboxTokens();
            tok.token = token;
            tok.tokboxSession = await GetSessionInfo(sessionId);
            tok.user = await GetUserByUsername(username);
            await _context.AddAsync(tok);
            await _context.SaveChangesAsync();
            return token;
        }

        [HttpPost("StartArchive")]
        public ActionResult<Guid> StartArchive(string sessionId, string name = "", bool hasVideo = true, bool hasAudio = true, OutputMode outputMode = OutputMode.COMPOSED, string resolution = null, ArchiveLayout layout = null)
        {
            return OpenTok.StartArchive(sessionId, name, hasAudio, hasVideo, outputMode, resolution, layout).Id;

        }

        [HttpPost("GetArchive")]
        public ActionResult<string> GetArchive(string archiveId)
        {
            OpenTokService();
            Archive archive =OpenTok.GetArchive(archiveId);
            return archive.Url;
        }

        
        // [HttpPost("GetArchiveList")]
        private async Task<bool> UserExists(string username)
        {
            return await _context.User.AnyAsync(x => x.Username == username.ToLower());
        }

        private async Task<TokboxSession> GetSessionInfo(string sessionId)
        {
            return await _context.Session.SingleOrDefaultAsync(x => x.SessionId == sessionId);
        }
        private async Task<User> GetUserByUsername(string username)
        {
            return await _context.User.SingleOrDefaultAsync(x => x.Username == username);
        }
        private void OpenTokService()
        {
            int apiKey = 0;
            string apiSecret = null;

            try
            {
                string apiKeyString = _config.Value.API_KEY;
                apiSecret = _config.Value.API_SECRET;
                apiKey = Convert.ToInt32(apiKeyString);
            }

            catch (Exception ex)
            {
                Console.Write(ex);

            }

            finally
            {
                if (apiKey == 0 || apiSecret == null)
                {
                    Console.WriteLine(
                        "The OpenTok API Key and API Secret were not set in the application configuration. " +
                        "Set the values in App.config and try again. (apiKey = {0}, apiSecret = {1})", apiKey, apiSecret);
                    Console.ReadLine();
                    Environment.Exit(-1);
                }
            }

            this.OpenTok = new OpenTok(apiKey, apiSecret);
        }
    }
}