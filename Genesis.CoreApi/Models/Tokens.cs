namespace Genesis.CoreApi.Models
{
    public class Tokens
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }

        public string[] Roles { get; set; }
    }
}
