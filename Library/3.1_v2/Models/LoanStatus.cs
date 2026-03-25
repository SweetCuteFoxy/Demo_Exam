namespace LibraryV2.Models
{
    public class LoanStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    }
}
