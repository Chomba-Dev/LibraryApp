using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace App.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
   
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany(b => b.Borrowings)
            .HasForeignKey(b => b.BookId);
        
        builder.Entity<Borrowing>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId);
    }
}