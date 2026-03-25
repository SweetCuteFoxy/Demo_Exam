namespace LibraryV2.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int PublisherId { get; set; }
        public int YearPublished { get; set; }
        public int Pages { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? Annotation { get; set; }
        public string? CoverImage { get; set; }

        public Genre? Genre { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    }
}
