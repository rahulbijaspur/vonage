using System;

namespace api.Entities
{
    public class TokboxSession
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public User CreatedBy { get; set; }
        public DateTime TimeStamp { get; set; }=DateTime.UtcNow;
        public bool isArchive { get; set; }
        public string archiveId { get; set; }

    }
}