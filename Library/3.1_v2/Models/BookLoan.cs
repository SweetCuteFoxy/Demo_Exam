namespace LibraryV2.Models
{
    public class BookLoan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDateExpected { get; set; }
        public DateTime? ReturnDateActual { get; set; }
        public int StatusId { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }
        public LoanStatus? Status { get; set; }
    }
}
