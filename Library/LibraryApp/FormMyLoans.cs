using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormMyLoans : Form
    {
        private User currentUser;
        private DataGridView dgv = null!;

        public FormMyLoans(User user)
        {
            currentUser = user;
            InitUI();
            LoadLoans();
        }

        private void InitUI()
        {
            Text = "Мои книги";
            Size = new Size(750, 400);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            Controls.Add(dgv);
        }

        private void LoadLoans()
        {
            using var db = new LibraryContext();
            var data = db.BookLoans
                .Include(l => l.Book)
                .Include(l => l.Status)
                .Where(l => l.UserId == currentUser.Id)
                .OrderByDescending(l => l.LoanDate)
                .Select(l => new
                {
                    Книга = l.Book!.Title,
                    ISBN = l.Book.Isbn,
                    Выдана = l.LoanDate.ToShortDateString(),
                    Вернуть_до = l.ReturnDateExpected.ToShortDateString(),
                    Возвращена = l.ReturnDateActual != null ? l.ReturnDateActual.Value.ToShortDateString() : "—",
                    Статус = l.Status!.Name
                })
                .ToList();

            dgv.DataSource = data;
        }
    }
}
