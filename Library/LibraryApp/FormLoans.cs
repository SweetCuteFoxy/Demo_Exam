using System.Reflection;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormLoans : Form
    {
        private User? currentUser;
        private DataGridView dgv = null!;
        private ComboBox cmbStatus = null!;
        private TextBox txtSearch = null!;
        private List<BookLoan> allLoans = new();
        private string _sortColumn = "";
        private bool _sortAsc = true;

        public FormLoans(User? user)
        {
            currentUser = user;
            InitUI();
            LoadLoans();
        }

        private void InitUI()
        {
            Text = "Выдачи книг";
            Size = new Size(900, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var top = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(240, 248, 255) };
            Controls.Add(top);

            txtSearch = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(220, 24),
                PlaceholderText = "Поиск по читателю или книге..."
            };
            txtSearch.TextChanged += (s, e) => Filter();
            top.Controls.Add(txtSearch);

            cmbStatus = new ComboBox
            {
                Location = new Point(240, 10),
                Size = new Size(140, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Все статусы", "На руках", "Возвращена" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => Filter();
            top.Controls.Add(cmbStatus);

            string r = currentUser?.Role?.Name ?? "";
            if (r == "Библиотекарь" || r == "Администратор")
            {
                var btnIssue = new Button
                {
                    Text = "Выдать книгу",
                    Size = new Size(110, 26),
                    Location = new Point(410, 9),
                    BackColor = Color.FromArgb(74, 111, 165),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnIssue.FlatAppearance.BorderSize = 0;
                btnIssue.Click += (s, e) =>
                {
                    if (new FormIssueLoan().ShowDialog() == DialogResult.OK) LoadLoans();
                };
                top.Controls.Add(btnIssue);

                var btnReturn = new Button
                {
                    Text = "Вернуть",
                    Size = new Size(80, 26),
                    Location = new Point(530, 9),
                    BackColor = Color.SeaGreen,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnReturn.FlatAppearance.BorderSize = 0;
                btnReturn.Click += BtnReturn_Click;
                top.Controls.Add(btnReturn);

                var btnEdit = new Button
                {
                    Text = "Изменить",
                    Size = new Size(90, 26),
                    Location = new Point(620, 9),
                    BackColor = Color.FromArgb(74, 111, 165),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnEdit.FlatAppearance.BorderSize = 0;
                btnEdit.Click += BtnEdit_Click;
                top.Controls.Add(btnEdit);

                var btnDelete = new Button
                {
                    Text = "Удалить",
                    Size = new Size(80, 26),
                    Location = new Point(718, 9),
                    BackColor = Color.IndianRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnDelete.FlatAppearance.BorderSize = 0;
                btnDelete.Click += BtnDelete_Click;
                top.Controls.Add(btnDelete);
            }

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
            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
            Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadLoans()
        {
            using var db = new LibraryContext();
            allLoans = db.BookLoans
                .Include(l => l.User)
                .Include(l => l.Book)
                .Include(l => l.Status)
                .OrderByDescending(l => l.LoanDate)
                .ToList();
            Filter();
        }

        private void Filter()
        {
            var list = allLoans.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var q = txtSearch.Text.Trim().ToLower();
                list = list.Where(l =>
                    (l.User?.FullName ?? "").ToLower().Contains(q)
                    || (l.User?.LibraryCard ?? "").ToLower().Contains(q)
                    || (l.Book?.Title ?? "").ToLower().Contains(q)
                    || (l.Book?.Isbn ?? "").ToLower().Contains(q));
            }
            if (cmbStatus.SelectedIndex > 0)
            {
                string statusFilter = cmbStatus.SelectedItem?.ToString() ?? "";
                list = list.Where(l =>
                {
                    string real = l.ReturnDateActual != null ? "Возвращена" : "На руках";
                    return real == statusFilter;
                });
            }

            var result = list.Select(l => new
            {
                ID = l.Id,
                Читатель = l.User?.FullName ?? "",
                Билет = l.User?.LibraryCard ?? "",
                Книга = l.Book?.Title ?? "",
                Выдана = l.LoanDate.ToShortDateString(),
                Вернуть_до = l.ReturnDateExpected.ToShortDateString(),
                Возвращена = l.ReturnDateActual?.ToShortDateString() ?? "—",
                Статус = l.ReturnDateActual != null ? "Возвращена" : "На руках",
                _Просрочена = l.ReturnDateActual != null
                    ? (l.ReturnDateActual.Value.Date > l.ReturnDateExpected.Date ? "late" : "ok")
                    : (DateTime.Now.Date > l.ReturnDateExpected.Date ? "overdue" : "")
            }).ToList();

            dgv.DataSource = result;

            if (dgv.Columns.Contains("_Просрочена"))
                dgv.Columns["_Просрочена"]!.Visible = false;

            if (!string.IsNullOrEmpty(_sortColumn) && dgv.Columns.Contains(_sortColumn))
                SortAndRebind(_sortColumn, _sortAsc);
        }

        private void Dgv_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dgv.Columns.Count) return;
            var col = dgv.Columns[e.ColumnIndex].Name;
            if (col == "_Просрочена") return;

            if (_sortColumn == col)
                _sortAsc = !_sortAsc;
            else
            {
                _sortColumn = col;
                _sortAsc = true;
            }
            SortAndRebind(col, _sortAsc);
        }

        private void SortAndRebind(string column, bool asc)
        {
            if (dgv.DataSource is not System.Collections.IList src || src.Count == 0) return;
            var itemType = src[0]!.GetType();
            var prop = itemType.GetProperty(column, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return;

            var sorted = asc
                ? src.Cast<object>().OrderBy(x => prop.GetValue(x)).ToList()
                : src.Cast<object>().OrderByDescending(x => prop.GetValue(x)).ToList();

            var listType = typeof(List<>).MakeGenericType(itemType);
            var typedList = Activator.CreateInstance(listType) as System.Collections.IList;
            foreach (var item in sorted) typedList!.Add(item);
            dgv.DataSource = typedList;

            if (dgv.Columns.Contains("_Просрочена"))
                dgv.Columns["_Просрочена"]!.Visible = false;
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgv.Columns.Contains("_Просрочена")) return;

            var val = dgv.Rows[e.RowIndex].Cells["_Просрочена"].Value?.ToString();
            if (val == "late" || val == "overdue")
                e.CellStyle.BackColor = Color.FromArgb(255, 204, 204);
            else if (val == "ok")
                e.CellStyle.BackColor = Color.FromArgb(212, 237, 218);
        }

        private void BtnReturn_Click(object? s, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["ID"].Value;

            using var db = new LibraryContext();
            var loan = db.BookLoans.Include(l => l.Book).FirstOrDefault(l => l.Id == id);
            if (loan == null || loan.ReturnDateActual != null)
            {
                MessageBox.Show("Эта книга уже возвращена");
                return;
            }

            loan.ReturnDateActual = DateTime.Now;
            var returned = db.LoanStatuses.FirstOrDefault(s => s.Name == "Возвращена");
            if (returned != null) loan.StatusId = returned.Id;
            if (loan.Book != null) loan.Book.AvailableCopies++;
            db.SaveChanges();
            LoadLoans();
        }

        private void BtnEdit_Click(object? s, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["ID"].Value;

            using var db = new LibraryContext();
            var loan = db.BookLoans.Include(l => l.User).Include(l => l.Book).FirstOrDefault(l => l.Id == id);
            if (loan == null) return;

            if (new FormEditLoan(loan).ShowDialog() == DialogResult.OK)
                LoadLoans();
        }

        private void BtnDelete_Click(object? s, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["ID"].Value;

            if (MessageBox.Show("Удалить запись о выдаче?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var db = new LibraryContext();
            var loan = db.BookLoans.Include(l => l.Book).FirstOrDefault(l => l.Id == id);
            if (loan == null) return;

            if (loan.ReturnDateActual == null && loan.Book != null)
                loan.Book.AvailableCopies++;

            db.BookLoans.Remove(loan);
            db.SaveChanges();
            LoadLoans();
        }
    }
}
