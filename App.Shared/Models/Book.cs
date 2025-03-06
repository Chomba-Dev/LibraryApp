using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class Book
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Author { get; set; } = string.Empty;
    
    public int Genre { get; set; }
    
    [Required]
    public int TotalCopies { get; set; }
    
    public int AvailableCopies { get; set; }
    
    public int Year { get; set; }
    
    public List<Borrowing> Borrowings { get; set; } = new();
}