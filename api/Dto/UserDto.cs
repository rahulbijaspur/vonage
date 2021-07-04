using OpenTokSDK;

namespace api.Dto
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public Session Session { get; protected set; }
        public OpenTok OpenTok { get; protected set; }
    }
}