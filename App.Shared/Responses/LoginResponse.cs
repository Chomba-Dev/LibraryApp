namespace App.Shared.Responses
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}