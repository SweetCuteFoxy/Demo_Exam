using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormIssueLoan : Form
    {
        private ComboBox cmbUser = null!;
        private ComboBox cmbBook = null!;
        private DateTimePicker dtpReturn = null!;
        private Label lblErr = null!;
        private List<User> readers = new();
        private List<Book> books = new();

        public FormIssueLoan()
        {
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            Text = "Выдать книгу";
            Size = new Size(400, 280);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            Controls.Add(new Label { Text = "Читатель:", Location = new Point(15, 15), AutoSize = true });
            cmbUser = new ComboBox { Location = new Point(15, 35), Size = new Size(350, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbUser);

            Controls.Add(new Label { Text = "Книга:", Location = new Point(15, 65), AutoSize = true });
            cmbBook = new ComboBox { Location = new Point(15, 85), Size = new Size(350, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbBook);

            Controls.Add(new Label { Text = "Вернуть до:", Location = new Point(15, 115), AutoSize = true });
            dtpReturn = new DateTimePicker { Location = new Point(15, 135), Size = new Size(180, 24), Value = DateTime.Now.AddDays(30) };
            Controls.Add(dtpReturn);

            lblErr = new Label { Text = "", ForeColor = Color.Red, Location = new Point(15, 165), AutoSize = true };
            Controls.Add(lblErr);

            var btnOk = new Button
            {
                Text = "Выдать",
                Size = new Size(90, 30),
                Location = new Point(15, 190),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += BtnOk_Click;
            Controls.Add(btnOk);

            var btnCancel = new Button { Text = "Отмена", Size = new Size(80, 30), Location = new Point(115, 190) };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            using var db = new LibraryContext();
            readers = db.Users.Include(u => u.Role).Where(u => u.Role!.Name != "Администратор").ToList();
            books = db.Books.Where(b => b.AvailableCopies > 0).OrderBy(b => b.Title).ToList();

            foreach (var u in readers) cmbUser.Items.Add($"{u.LibraryCard} — {u.FullName}");
            if (cmbUser.Items.Count > 0) cmbUser.SelectedIndex = 0;

            foreach (var b in books) cmbBook.Items.Add($"{b.Isbn} — {b.Title} (ост. {b.AvailableCopies})");
            if (cmbBook.Items.Count > 0) cmbBook.SelectedIndex = 0;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            lblErr.Text = "";
            if (cmbUser.SelectedIndex < 0 || cmbBook.SelectedIndex < 0)
            {
                lblErr.Text = "Выберите читателя и книгу";
                return;
            }
            if (dtpReturn.Value.Date < DateTime.Now.Date)
            {
                lblErr.Text = "Дата возврата не может быть раньше сегодняшнего дня";
                return;
            }

            var user = readers[cmbUser.SelectedIndex];
            var book = books[cmbBook.SelectedIndex];

            using var db = new LibraryContext();
            var status = db.LoanStatuses.FirstOrDefault(s => s.Name == "На руках");
            if (status == null) return;

            var dbBook = db.Books.Find(book.Id);
            if (dbBook == null || dbBook.AvailableCopies <= 0)
            {
                lblErr.Text = "Нет доступных экземпляров";
                return;
            }

            db.BookLoans.Add(new BookLoan
            {
                UserId = user.Id,
                BookId = book.Id,
                LoanDate = DateTime.Now,
                ReturnDateExpected = dtpReturn.Value,
                StatusId = status.Id
            });
            dbBook.AvailableCopies--;
            db.SaveChanges();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
