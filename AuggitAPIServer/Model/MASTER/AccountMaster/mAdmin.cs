namespace AuggitAPIServer.Model.MASTER.AccountMaster
{
    public class mAdmin
    {
        public Guid id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public bool is_verified { get; set; } = false;
        public string token { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
