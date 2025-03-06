using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Shared.Models;

public class Borrowing
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Book { get; set; }
    [Required]
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    
    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

    public DateTime? ReturnDate { get; set; }

}