using LibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2
{
    public class FormBooks : Form
    {
        private readonly User? currentUser;
        private DataGridView dgvBooks = null!;
        private TextBox txtSearch = null!;
        private ComboBox cmbGenre = null!;
        private ComboBox cmbSort = null!;
        private Label lblUserInfo = null!;
        private Label lblCount = null!;

        private readonly string userRole;
        private List<Book> allBooks = new();

        public FormBooks(User? user)
        {
            currentUser = user;
            userRole = user?.Role?.Name ?? "Гость";
            InitUI();
            LoadBooks();
        }

        private Button CreateStyledButton(string text, Point loc, Size size, Color bg, Color fg, FontStyle style = FontStyle.Regular)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Times New Roman", 9, style),
                Cursor = Cursors.Hand,
                BackColor = bg,
                ForeColor = fg
            };
            btn.FlatAppearance.BorderSize = 0;

            var normalBg = bg;
            var hoverBg = Color.FromArgb(
                Math.Max(bg.R - 20, 0),
                Math.Max(bg.G - 20, 0),
                Math.Max(bg.B - 20, 0));
            btn.MouseEnter += (s, e) => btn.BackColor = hoverBg;
            btn.MouseLeave += (s, e) => btn.BackColor = normalBg;

            return btn;
        }

        private void InitUI()
        {
            Text = "Библиотека Читай-Город - Каталог книг";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(240, 248, 255)
            };
            Controls.Add(topPanel);

            lblUserInfo = new Label
            {
                Text = currentUser?.FullName ?? "Гость",
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 111, 165),
                AutoSize = true,
                Location = new Point(Width - 350, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(lblUserInfo);

            var lblRole = new Label
            {
                Text = $"[{userRole}]",
                Font = new Font("Times New Roman", 9),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(Width - 350, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(lblRole);

            var btnLogout = CreateStyledButton("Выход",
                new Point(Width - 110, 12), new Size(80, 30),
                Color.FromArgb(74, 111, 165), Color.White);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Click += (s, e) => { DialogResult = DialogResult.Retry; Close(); };
            topPanel.Controls.Add(btnLogout);

            var filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(10, 8, 10, 5)
            };
            Controls.Add(filterPanel);

            bool canFilter = userRole is "Библиотекарь" or "Администратор";

            txtSearch = new TextBox
            {
                Location = new Point(10, 12),
                Size = new Size(220, 26),
                PlaceholderText = "Поиск...",
                Font = new Font("Times New Roman", 10),
                Enabled = canFilter
            };
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(txtSearch);

            cmbGenre = new ComboBox
            {
                Location = new Point(240, 12),
                Size = new Size(160, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Times New Roman", 10),
                Enabled = canFilter
            };
            cmbGenre.SelectedIndexChanged += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(cmbGenre);

            cmbSort = new ComboBox
            {
                Location = new Point(410, 12),
                Size = new Size(180, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Times New Roman", 10),
                Enabled = canFilter
            };
            cmbSort.Items.AddRange(new object[] { "Без сортировки", "Название А-Я", "Название Я-А", "Год (новые)", "Год (старые)" });
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(cmbSort);

            lblCount = new Label
            {
                Location = new Point(600, 14),
                AutoSize = true,
                Font = new Font("Times New Roman", 10),
                ForeColor = Color.Gray
            };
            filterPanel.Controls.Add(lblCount);

            if (userRole == "Администратор")
            {
                var btnAdd = CreateStyledButton("Добавить", new Point(780, 10), new Size(90, 28),
                    Color.FromArgb(74, 111, 165), Color.White, FontStyle.Bold);
                btnAdd.Click += BtnAddBook_Click;
                filterPanel.Controls.Add(btnAdd);

                var btnEdit = CreateStyledButton("Изменить", new Point(875, 10), new Size(90, 28),
                    Color.FromArgb(100, 140, 190), Color.White);
                btnEdit.Click += BtnEditBook_Click;
                filterPanel.Controls.Add(btnEdit);

                var btnDel = CreateStyledButton("Удалить", new Point(970, 10), new Size(80, 28),
                    Color.FromArgb(200, 80, 80), Color.White);
                btnDel.Click += BtnDeleteBook_Click;
                filterPanel.Controls.Add(btnDel);
            }

            if (userRole is "Библиотекарь" or "Администратор")
            {
                var btnLoans = CreateStyledButton("Выдачи", new Point(700, 10), new Size(80, 28),
                    Color.FromArgb(74, 111, 165), Color.White);
                btnLoans.Click += BtnLoans_Click;
                filterPanel.Controls.Add(btnLoans);
            }

            if (userRole == "Читатель")
            {
                var btnMyLoans = CreateStyledButton("Мои книги", new Point(240, 10), new Size(100, 28),
                    Color.FromArgb(74, 111, 165), Color.White);
                btnMyLoans.Click += BtnMyLoans_Click;
                filterPanel.Controls.Add(btnMyLoans);
            }

            dgvBooks = new DataGridView
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
                Font = new Font("Times New Roman", 9),
                RowTemplate = { Height = 60 }
            };
            dgvBooks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvBooks.CellFormatting += DgvBooks_CellFormatting;
            dgvBooks.CellPainting += DgvBooks_CellPainting;
            Controls.Add(dgvBooks);

            dgvBooks.BringToFront();
        }

        private void LoadBooks()
        {
            using var db = new LibraryContext();

            allBooks = db.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.BookLoans)
                .ToList();

            cmbGenre.Items.Clear();
            cmbGenre.Items.Add("Все жанры");
            foreach (var g in db.Genres.OrderBy(g => g.Name).Select(g => g.Name))
                cmbGenre.Items.Add(g);
            cmbGenre.SelectedIndex = 0;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = allBooks.AsEnumerable();

            if (txtSearch.Enabled && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var q = txtSearch.Text.Trim().ToLower();
                filtered = filtered.Where(b =>
                    b.Title.ToLower().Contains(q) ||
                    b.Author.ToLower().Contains(q) ||
                    b.Isbn.ToLower().Contains(q) ||
                    (b.Annotation ?? "").ToLower().Contains(q));
            }

            if (cmbGenre.Enabled && cmbGenre.SelectedIndex > 0)
            {
                var genre = cmbGenre.SelectedItem?.ToString();
                filtered = filtered.Where(b => b.Genre?.Name == genre);
            }

            if (cmbSort.Enabled)
            {
                filtered = cmbSort.SelectedIndex switch
                {
                    1 => filtered.OrderBy(b => b.Title),
                    2 => filtered.OrderByDescending(b => b.Title),
                    3 => filtered.OrderByDescending(b => b.YearPublished),
                    4 => filtered.OrderBy(b => b.YearPublished),
                    _ => filtered
                };
            }

            var list = filtered.ToList();
            lblCount.Text = $"Найдено: {list.Count}";

            var data = list.Select(b => new
            {
                Обложка = "",
                ISBN = b.Isbn,
                Название = b.Title,
                Автор = b.Author,
                Жанр = b.Genre?.Name ?? "",
                Издательство = b.Publisher?.Name ?? "",
                Год = b.YearPublished,
                Страниц = b.Pages,
                Всего = b.TotalCopies,
                Доступно = b.AvailableCopies,
                Аннотация = b.Annotation ?? "",
                _BookId = b.Id,
                _CoverImage = b.CoverImage ?? "",
                _HasOldLoan = b.BookLoans.Any(l =>
                    l.ReturnDateActual == null && (DateTime.Now - l.LoanDate).TotalDays > 30)
            }).ToList();

            dgvBooks.DataSource = data;

            if (dgvBooks.Columns.Contains("_BookId"))
                dgvBooks.Columns["_BookId"]!.Visible = false;
            if (dgvBooks.Columns.Contains("_CoverImage"))
                dgvBooks.Columns["_CoverImage"]!.Visible = false;
            if (dgvBooks.Columns.Contains("_HasOldLoan"))
                dgvBooks.Columns["_HasOldLoan"]!.Visible = false;

            if (dgvBooks.Columns.Contains("Обложка"))
            {
                dgvBooks.Columns["Обложка"]!.Width = 60;
                dgvBooks.Columns["Обложка"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            if (dgvBooks.Columns.Contains("Аннотация"))
                dgvBooks.Columns["Аннотация"]!.FillWeight = 200;
        }

        private void DgvBooks_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvBooks.Rows[e.RowIndex];
            int available = 0;
            bool hasOldLoan = false;

            if (row.Cells["Доступно"].Value is int av) available = av;
            if (row.Cells["_HasOldLoan"].Value is bool ol) hasOldLoan = ol;

            if (available == 0)
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
            else if (available <= 2)
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
            else if (hasOldLoan)
                row.DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218);
        }

        private void DgvBooks_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvBooks.Columns[e.ColumnIndex].Name != "Обложка") return;

            e.Handled = true;
            e.PaintBackground(e.ClipBounds, true);

            var row = dgvBooks.Rows[e.RowIndex];
            var coverVal = row.Cells["_CoverImage"].Value?.ToString();
            string imgPath;

            if (!string.IsNullOrEmpty(coverVal) && File.Exists(Path.Combine("Resources", coverVal)))
                imgPath = Path.Combine("Resources", coverVal);
            else
                imgPath = Path.Combine("Resources", "book_placeholder.png");

            if (!File.Exists(imgPath)) return;

            using var img = Image.FromFile(imgPath);
            var rect = e.CellBounds;
            int pad = 4;
            var destRect = new Rectangle(rect.X + pad, rect.Y + pad,
                rect.Width - pad * 2, rect.Height - pad * 2);
            e.Graphics!.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(img, destRect);
        }

        private void BtnLoans_Click(object? sender, EventArgs e)
        {
            using var form = new FormLoans(currentUser);
            form.ShowDialog();
            LoadBooks();
        }

        private void BtnMyLoans_Click(object? sender, EventArgs e)
        {
            if (currentUser == null) return;
            using var form = new FormMyLoans(currentUser);
            form.ShowDialog();
        }

        private void BtnAddBook_Click(object? sender, EventArgs e)
        {
            using var form = new FormEditBook(null);
            if (form.ShowDialog() == DialogResult.OK)
                LoadBooks();
        }

        private void BtnEditBook_Click(object? sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0) return;
            if (dgvBooks.SelectedRows[0].Cells["_BookId"].Value is not int bookId) return;
            using var db = new LibraryContext();
            var book = db.Books.Include(b => b.Genre).Include(b => b.Publisher).FirstOrDefault(b => b.Id == bookId);
            if (book == null) return;
            using var form = new FormEditBook(book);
            if (form.ShowDialog() == DialogResult.OK)
                LoadBooks();
        }

        private void BtnDeleteBook_Click(object? sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0) return;
            if (dgvBooks.SelectedRows[0].Cells["_BookId"].Value is not int bookId) return;
            var title = dgvBooks.SelectedRows[0].Cells["Название"].Value?.ToString();

            if (MessageBox.Show($"Удалить книгу \"{title}\"?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var db = new LibraryContext();
            var book = db.Books.Find(bookId);
            if (book == null) return;

            db.Books.Remove(book);
            db.SaveChanges();
            LoadBooks();
        }
    }
}
