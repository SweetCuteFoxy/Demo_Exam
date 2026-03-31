using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormEditLoan : Form
    {
        private ComboBox cmbUser = null!;
        private ComboBox cmbBook = null!;
        private DateTimePicker dtpLoan = null!;
        private DateTimePicker dtpReturn = null!;
        private DateTimePicker dtpActual = null!;
        private CheckBox chkReturned = null!;
        private Label lblErr = null!;

        private List<User> readers = new();
        private List<Book> books = new();
        private readonly int loanId;
        private readonly int origUserId;
        private readonly int origBookId;
        private readonly DateTime origLoanDate;
        private readonly DateTime origReturnExpected;
        private readonly DateTime? origReturnActual;

        public FormEditLoan(BookLoan loan)
        {
            loanId = loan.Id;
            origUserId = loan.UserId;
            origBookId = loan.BookId;
            origLoanDate = loan.LoanDate;
            origReturnExpected = loan.ReturnDateExpected;
            origReturnActual = loan.ReturnDateActual;

            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            Text = "Редактирование выдачи";
            Size = new Size(400, 360);
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

            Controls.Add(new Label { Text = "Дата выдачи:", Location = new Point(15, 115), AutoSize = true });
            dtpLoan = new DateTimePicker { Location = new Point(15, 135), Size = new Size(180, 24), Value = origLoanDate };
            Controls.Add(dtpLoan);

            Controls.Add(new Label { Text = "Вернуть до:", Location = new Point(15, 165), AutoSize = true });
            dtpReturn = new DateTimePicker { Location = new Point(15, 185), Size = new Size(180, 24), Value = origReturnExpected };
            Controls.Add(dtpReturn);

            chkReturned = new CheckBox
            {
                Text = "Возвращена",
                Location = new Point(15, 215),
                AutoSize = true,
                Checked = origReturnActual != null
            };
            chkReturned.CheckedChanged += (s, e) => dtpActual.Enabled = chkReturned.Checked;
            Controls.Add(chkReturned);

            dtpActual = new DateTimePicker
            {
                Location = new Point(140, 213),
                Size = new Size(180, 24),
                Value = origReturnActual ?? DateTime.Now,
                Enabled = origReturnActual != null
            };
            Controls.Add(dtpActual);

            lblErr = new Label { Text = "", ForeColor = Color.Red, Location = new Point(15, 245), AutoSize = true };
            Controls.Add(lblErr);

            var btnOk = new Button
            {
                Text = "Сохранить",
                Size = new Size(100, 30),
                Location = new Point(15, 275),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += BtnOk_Click;
            Controls.Add(btnOk);

            var btnCancel = new Button { Text = "Отмена", Size = new Size(80, 30), Location = new Point(125, 275) };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            using var db = new LibraryContext();
            readers = db.Users.Include(u => u.Role).Where(u => u.Role!.Name != "Администратор").ToList();
            books = db.Books.OrderBy(b => b.Title).ToList();

            int userIdx = -1, bookIdx = -1;
            for (int i = 0; i < readers.Count; i++)
            {
                var u = readers[i];
                cmbUser.Items.Add($"{u.LibraryCard} — {u.FullName}");
                if (u.Id == origUserId) userIdx = i;
            }
            if (userIdx >= 0) cmbUser.SelectedIndex = userIdx;
            else if (cmbUser.Items.Count > 0) cmbUser.SelectedIndex = 0;

            for (int i = 0; i < books.Count; i++)
            {
                var b = books[i];
                cmbBook.Items.Add($"{b.Isbn} — {b.Title} (всего {b.TotalCopies})");
                if (b.Id == origBookId) bookIdx = i;
            }
            if (bookIdx >= 0) cmbBook.SelectedIndex = bookIdx;
            else if (cmbBook.Items.Count > 0) cmbBook.SelectedIndex = 0;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            lblErr.Text = "";
            if (cmbUser.SelectedIndex < 0 || cmbBook.SelectedIndex < 0)
            {
                lblErr.Text = "Выберите читателя и книгу";
                return;
            }
            if (dtpReturn.Value.Date < dtpLoan.Value.Date)
            {
                lblErr.Text = "Дата возврата не может быть раньше даты выдачи";
                return;
            }
            if (chkReturned.Checked && dtpActual.Value.Date < dtpLoan.Value.Date)
            {
                lblErr.Text = "Дата фактического возврата не может быть раньше даты выдачи";
                return;
            }

            var newUser = readers[cmbUser.SelectedIndex];
            var newBook = books[cmbBook.SelectedIndex];
            bool wasReturned = origReturnActual != null;
            bool nowReturned = chkReturned.Checked;

            using var db = new LibraryContext();
            var loan = db.BookLoans.Include(l => l.Book).FirstOrDefault(l => l.Id == loanId);
            if (loan == null) { Close(); return; }

            // Handle book availability changes
            if (loan.BookId != newBook.Id)
            {
                // Returning copy to old book (if loan was active)
                if (!wasReturned)
                {
                    var oldBook = db.Books.Find(loan.BookId);
                    if (oldBook != null) oldBook.AvailableCopies++;
                }
                // Taking copy from new book (if loan stays active)
                if (!nowReturned)
                {
                    var dbNewBook = db.Books.Find(newBook.Id);
                    if (dbNewBook == null || dbNewBook.AvailableCopies <= 0)
                    {
                        lblErr.Text = "Нет доступных экземпляров этой книги";
                        return;
                    }
                    dbNewBook.AvailableCopies--;
                }
            }
            else
            {
                // Same book — handle return status change
                var dbBook = db.Books.Find(loan.BookId);
                if (dbBook != null)
                {
                    if (!wasReturned && nowReturned)
                        dbBook.AvailableCopies++;
                    else if (wasReturned && !nowReturned)
                    {
                        if (dbBook.AvailableCopies <= 0)
                        {
                            lblErr.Text = "Нет доступных экземпляров";
                            return;
                        }
                        dbBook.AvailableCopies--;
                    }
                }
            }

            loan.UserId = newUser.Id;
            loan.BookId = newBook.Id;
            loan.LoanDate = dtpLoan.Value;
            loan.ReturnDateExpected = dtpReturn.Value;
            loan.ReturnDateActual = nowReturned ? dtpActual.Value : null;

            var status = db.LoanStatuses.FirstOrDefault(s => s.Name == (nowReturned ? "Возвращена" : "На руках"));
            if (status != null) loan.StatusId = status.Id;

            db.SaveChanges();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
