using LibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2
{
    public class FormIssueLoan : Form
    {
        private ComboBox cmbUser = null!;
        private ComboBox cmbBook = null!;
        private DateTimePicker dtpReturn = null!;
        private Label lblError = null!;
        private List<User> readers = new();
        private List<Book> availableBooks = new();

        public FormIssueLoan()
        {
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            Text = "Выдать книгу";
            Size = new Size(420, 300);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            Controls.Add(new Label { Text = "Читатель:", Location = new Point(20, 20), AutoSize = true });

            cmbUser = new ComboBox
            {
                Location = new Point(20, 42),
                Size = new Size(360, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Times New Roman", 10)
            };
            Controls.Add(cmbUser);

            Controls.Add(new Label { Text = "Книга:", Location = new Point(20, 75), AutoSize = true });

            cmbBook = new ComboBox
            {
                Location = new Point(20, 97),
                Size = new Size(360, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Times New Roman", 10)
            };
            Controls.Add(cmbBook);

            Controls.Add(new Label { Text = "Вернуть до:", Location = new Point(20, 130), AutoSize = true });

            dtpReturn = new DateTimePicker
            {
                Location = new Point(20, 152),
                Size = new Size(200, 26),
                Value = DateTime.Now.AddDays(30),
                Font = new Font("Times New Roman", 10)
            };
            Controls.Add(dtpReturn);

            lblError = new Label
            {
                Text = "",
                ForeColor = Color.Red,
                Location = new Point(20, 185),
                AutoSize = true,
                Font = new Font("Times New Roman", 9)
            };
            Controls.Add(lblError);

            var btnOk = new Button
            {
                Text = "Выдать",
                Size = new Size(100, 34),
                Location = new Point(20, 210),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var normalBg = btnOk.BackColor;
            btnOk.MouseEnter += (s, e) => btnOk.BackColor = Color.FromArgb(58, 90, 140);
            btnOk.MouseLeave += (s, e) => btnOk.BackColor = normalBg;
            btnOk.Click += BtnOk_Click;
            Controls.Add(btnOk);

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(100, 34),
                Location = new Point(130, 210),
                Font = new Font("Times New Roman", 10)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            using var db = new LibraryContext();
            readers = db.Users.Include(u => u.Role).Where(u => u.Role!.Name != "Администратор").ToList();
            availableBooks = db.Books.Where(b => b.AvailableCopies > 0).OrderBy(b => b.Title).ToList();

            cmbUser.Items.Clear();
            foreach (var u in readers)
                cmbUser.Items.Add($"{u.LibraryCard} - {u.FullName}");
            if (cmbUser.Items.Count > 0) cmbUser.SelectedIndex = 0;

            cmbBook.Items.Clear();
            foreach (var b in availableBooks)
                cmbBook.Items.Add($"{b.Isbn} - {b.Title} (доступно: {b.AvailableCopies})");
            if (cmbBook.Items.Count > 0) cmbBook.SelectedIndex = 0;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            if (cmbUser.SelectedIndex < 0 || cmbBook.SelectedIndex < 0)
            {
                lblError.Text = "Выберите читателя и книгу";
                return;
            }

            var user = readers[cmbUser.SelectedIndex];
            var book = availableBooks[cmbBook.SelectedIndex];

            using var db = new LibraryContext();
            var statusOnHand = db.LoanStatuses.FirstOrDefault(s => s.Name == "На руках");
            if (statusOnHand == null) return;

            var dbBook = db.Books.Find(book.Id);
            if (dbBook == null || dbBook.AvailableCopies <= 0)
            {
                lblError.Text = "Нет доступных экземпляров";
                return;
            }

            var loan = new BookLoan
            {
                UserId = user.Id,
                BookId = book.Id,
                LoanDate = DateTime.Now,
                ReturnDateExpected = dtpReturn.Value,
                StatusId = statusOnHand.Id
            };

            dbBook.AvailableCopies--;
            db.BookLoans.Add(loan);
            db.SaveChanges();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
