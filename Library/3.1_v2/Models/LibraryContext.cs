using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<LoanStatus> LoanStatuses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseNpgsql("Host=localhost;Port=5432;Database=library;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Role>(e =>
            {
                e.ToTable("roles");
                e.Property(r => r.Id).HasColumnName("id");
                e.Property(r => r.Name).HasColumnName("name");
            });

            mb.Entity<Genre>(e =>
            {
                e.ToTable("genres");
                e.Property(g => g.Id).HasColumnName("id");
                e.Property(g => g.Name).HasColumnName("name");
            });

            mb.Entity<Publisher>(e =>
            {
                e.ToTable("publishers");
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.Name).HasColumnName("name");
            });

            mb.Entity<LoanStatus>(e =>
            {
                e.ToTable("loan_statuses");
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.Name).HasColumnName("name");
            });

            mb.Entity<User>(e =>
            {
                e.ToTable("users");
                e.Property(u => u.Id).HasColumnName("id");
                e.Property(u => u.FullName).HasColumnName("full_name");
                e.Property(u => u.LibraryCard).HasColumnName("library_card");
                e.Property(u => u.Login).HasColumnName("login");
                e.Property(u => u.PasswordText).HasColumnName("password_text");
                e.Property(u => u.RoleId).HasColumnName("role_id");
                e.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            });

            mb.Entity<Book>(e =>
            {
                e.ToTable("books");
                e.Property(b => b.Id).HasColumnName("id");
                e.Property(b => b.Isbn).HasColumnName("isbn");
                e.Property(b => b.Title).HasColumnName("title");
                e.Property(b => b.Author).HasColumnName("author");
                e.Property(b => b.GenreId).HasColumnName("genre_id");
                e.Property(b => b.PublisherId).HasColumnName("publisher_id");
                e.Property(b => b.YearPublished).HasColumnName("year_published");
                e.Property(b => b.Pages).HasColumnName("pages");
                e.Property(b => b.TotalCopies).HasColumnName("total_copies");
                e.Property(b => b.AvailableCopies).HasColumnName("available_copies");
                e.Property(b => b.Annotation).HasColumnName("annotation");
                e.Property(b => b.CoverImage).HasColumnName("cover_image");
                e.HasOne(b => b.Genre).WithMany(g => g.Books).HasForeignKey(b => b.GenreId);
                e.HasOne(b => b.Publisher).WithMany(p => p.Books).HasForeignKey(b => b.PublisherId);
            });

            mb.Entity<BookLoan>(e =>
            {
                e.ToTable("book_loans");
                e.Property(l => l.Id).HasColumnName("id");
                e.Property(l => l.UserId).HasColumnName("user_id");
                e.Property(l => l.BookId).HasColumnName("book_id");
                e.Property(l => l.LoanDate).HasColumnName("loan_date");
                e.Property(l => l.ReturnDateExpected).HasColumnName("return_date_expected");
                e.Property(l => l.ReturnDateActual).HasColumnName("return_date_actual");
                e.Property(l => l.StatusId).HasColumnName("status_id");
                e.HasOne(l => l.User).WithMany(u => u.BookLoans).HasForeignKey(l => l.UserId);
                e.HasOne(l => l.Book).WithMany(b => b.BookLoans).HasForeignKey(l => l.BookId);
                e.HasOne(l => l.Status).WithMany(s => s.BookLoans).HasForeignKey(l => l.StatusId);
            });
        }
    }
}
