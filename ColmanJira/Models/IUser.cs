namespace ColmanJira.Models
{
    public class IUser
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool isManager { get; set; }

        public IUser(string RoleId, string UserId, string Email, string Name, bool isManager)
        {
            this.RoleId = RoleId;
            this.UserId = UserId;
            this.Email = Email;
            this.Name = Name;
            this.isManager = isManager;
        }
    }
}
