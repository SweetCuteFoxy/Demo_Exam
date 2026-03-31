using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormBooks : Form
    {
        private User? currentUser;
        private DataGridView dgv = null!;
        private TextBox txtSearch = null!;
        private ComboBox cmbGenre = null!;
        private Label lblCount = null!;

        private string role = "Гость";
        private List<Book> allBooks = new();

        public FormBooks(User? user)
        {
            currentUser = user;
            if (user?.Role != null) role = user.Role.Name;
            InitUI();
            LoadBooks();
        }

        private void InitUI()
        {
            Text = "Читай-Город — Каталог";
            Size = new Size(1050, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(74, 111, 165) };
            Controls.Add(topPanel);

            var lblUser = new Label
            {
                Text = currentUser != null ? $"{currentUser.FullName} ({role})" : "Гость",
                ForeColor = Color.White,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                Location = new Point(12, 12),
                AutoSize = true
            };
            topPanel.Controls.Add(lblUser);

            var btnLogout = new Button
            {
                Text = "Выход",
                Size = new Size(70, 28),
                Location = new Point(Width - 100, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => { DialogResult = DialogResult.Retry; Close(); };
            topPanel.Controls.Add(btnLogout);

            var filterPanel = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(240, 248, 255) };
            Controls.Add(filterPanel);

            bool canFilter = role == "Библиотекарь" || role == "Администратор";

            txtSearch = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(200, 24),
                PlaceholderText = "Поиск по названию/автору...",
                Enabled = canFilter
            };
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(txtSearch);

            cmbGenre = new ComboBox
            {
                Location = new Point(220, 10),
                Size = new Size(150, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = canFilter
            };
            cmbGenre.SelectedIndexChanged += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(cmbGenre);

            lblCount = new Label { Location = new Point(385, 13), AutoSize = true, ForeColor = Color.Gray };
            filterPanel.Controls.Add(lblCount);

            int btnX = 520;

            if (role == "Библиотекарь" || role == "Администратор")
            {
                var btnLoans = new Button
                {
                    Text = "Выдачи",
                    Size = new Size(80, 26),
                    Location = new Point(btnX, 9),
                    BackColor = Color.FromArgb(74, 111, 165),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnLoans.FlatAppearance.BorderSize = 0;
                btnLoans.Click += (s, e) => { new FormLoans(currentUser).ShowDialog(); LoadBooks(); };
                filterPanel.Controls.Add(btnLoans);
                btnX += 90;
            }

            if (role == "Администратор")
            {
                var btnAdd = new Button
                {
                    Text = "Добавить",
                    Size = new Size(85, 26),
                    Location = new Point(btnX, 9),
                    BackColor = Color.SeaGreen,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnAdd.FlatAppearance.BorderSize = 0;
                btnAdd.Click += (s, e) =>
                {
                    if (new FormEditBook(null).ShowDialog() == DialogResult.OK) LoadBooks();
                };
                filterPanel.Controls.Add(btnAdd);

                var btnEdit = new Button
                {
                    Text = "Изменить",
                    Size = new Size(85, 26),
                    Location = new Point(btnX + 90, 9),
                    BackColor = Color.FromArgb(74, 111, 165),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnEdit.FlatAppearance.BorderSize = 0;
                btnEdit.Click += BtnEdit_Click;
                filterPanel.Controls.Add(btnEdit);

                var btnDel = new Button
                {
                    Text = "Удалить",
                    Size = new Size(75, 26),
                    Location = new Point(btnX + 180, 9),
                    BackColor = Color.IndianRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnDel.FlatAppearance.BorderSize = 0;
                btnDel.Click += BtnDelete_Click;
                filterPanel.Controls.Add(btnDel);
            }

            if (role == "Читатель")
            {
                var btnMy = new Button
                {
                    Text = "Мои книги",
                    Size = new Size(90, 26),
                    Location = new Point(btnX, 9),
                    BackColor = Color.FromArgb(74, 111, 165),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnMy.FlatAppearance.BorderSize = 0;
                btnMy.Click += (s, e) => { if (currentUser != null) new FormMyLoans(currentUser).ShowDialog(); };
                filterPanel.Controls.Add(btnMy);
            }

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Times New Roman", 10)
            };
            Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadBooks()
        {
            using var db = new LibraryContext();
            allBooks = db.Books.Include(b => b.Author).Include(b => b.Genre).Include(b => b.Publisher).ToList();

            cmbGenre.Items.Clear();
            cmbGenre.Items.Add("Все жанры");
            foreach (var g in db.Genres.OrderBy(g => g.Name))
                cmbGenre.Items.Add(g.Name);
            cmbGenre.SelectedIndex = 0;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var list = allBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var q = txtSearch.Text.Trim().ToLower();
                list = list.Where(b =>
                    b.Title.ToLower().Contains(q) ||
                    (b.Author?.Name ?? "").ToLower().Contains(q) ||
                    b.Isbn.ToLower().Contains(q));
            }

            if (cmbGenre.SelectedIndex > 0)
            {
                var g = cmbGenre.SelectedItem?.ToString();
                list = list.Where(b => b.Genre?.Name == g);
            }

            var result = list.ToList();
            lblCount.Text = $"Найдено: {result.Count}";

            dgv.DataSource = result.Select(b => new
            {
                b.Isbn,
                Название = b.Title,
                Автор = b.Author?.Name ?? "",
                Жанр = b.Genre?.Name ?? "",
                Издательство = b.Publisher?.Name ?? "",
                Год = b.YearPublished,
                Стр = b.Pages,
                Всего = b.TotalCopies,
                Доступно = b.AvailableCopies,
                _id = b.Id
            }).ToList();

            if (dgv.Columns.Contains("_id"))
                dgv.Columns["_id"].Visible = false;
        }

        private void BtnEdit_Click(object? s, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["_id"].Value;
            using var db = new LibraryContext();
            var book = db.Books.Include(b => b.Author).Include(b => b.Genre).Include(b => b.Publisher).FirstOrDefault(b => b.Id == id);
            if (book == null) return;
            if (new FormEditBook(book).ShowDialog() == DialogResult.OK) LoadBooks();
        }

        private void BtnDelete_Click(object? s, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["_id"].Value;
            var title = dgv.SelectedRows[0].Cells["Название"].Value?.ToString();

            if (MessageBox.Show($"Удалить \"{title}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using var db = new LibraryContext();
            var book = db.Books.Find(id);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
                LoadBooks();
            }
        }
    }
}
