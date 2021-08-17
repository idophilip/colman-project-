using Microsoft.AspNetCore.Identity;

namespace ColmanJira.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public virtual TaskStatus Status { get; set; }
        public long TypeId { get; set; }
        public virtual TaskType Type { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}