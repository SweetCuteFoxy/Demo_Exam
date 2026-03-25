namespace LibraryV2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LibraryCard { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordText { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    }
}
