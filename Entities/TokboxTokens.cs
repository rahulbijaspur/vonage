namespace api.Entities
{
    public class TokboxTokens
    {
        public int Id { get; set; }
        public string  token { get; set; }
        public User user { get; set; }
        public TokboxSession tokboxSession { get; set; }
    }
}