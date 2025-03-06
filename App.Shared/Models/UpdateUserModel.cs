namespace App.Shared.Models;

public class UpdateUserModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}