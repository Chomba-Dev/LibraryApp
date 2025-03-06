using Microsoft.AspNetCore.Identity;
namespace App.Shared.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}