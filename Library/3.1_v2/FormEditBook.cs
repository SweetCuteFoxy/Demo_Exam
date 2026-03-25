using LibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2
{
    public class FormEditBook : Form
    {
        private readonly Book? editingBook;
        private TextBox txtIsbn = null!;
        private TextBox txtTitle = null!;
        private TextBox txtAuthor = null!;
        private ComboBox cmbGenre = null!;
        private ComboBox cmbPublisher = null!;
        private TextBox txtYear = null!;
        private TextBox txtPages = null!;
        private TextBox txtTotal = null!;
        private TextBox txtAvailable = null!;
        private TextBox txtAnnotation = null!;
        private Label lblError = null!;
        private List<Genre> genres = new();
        private List<Publisher> publishers = new();

        public FormEditBook(Book? book)
        {
            editingBook = book;
            InitUI();
            LoadLookups();
            if (book != null) FillFields();
        }

        private void InitUI()
        {
            Text = editingBook == null ? "Добавить книгу" : "Редактировать книгу";
            Size = new Size(450, 520);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            AutoScroll = true;

            int y = 10;
            int lblX = 15, fldX = 15, fldW = 400;

            void AddField(string label, out TextBox txt)
            {
                Controls.Add(new Label { Text = label, Location = new Point(lblX, y), AutoSize = true });
                y += 20;
                txt = new TextBox { Location = new Point(fldX, y), Size = new Size(fldW, 24) };
                Controls.Add(txt);
                y += 30;
            }

            AddField("ISBN:", out txtIsbn);
            AddField("Название:", out txtTitle);
            AddField("Автор:", out txtAuthor);

            Controls.Add(new Label { Text = "Жанр:", Location = new Point(lblX, y), AutoSize = true });
            y += 20;
            cmbGenre = new ComboBox
            {
                Location = new Point(fldX, y),
                Size = new Size(200, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(cmbGenre);
            y += 30;

            Controls.Add(new Label { Text = "Издательство:", Location = new Point(lblX, y), AutoSize = true });
            y += 20;
            cmbPublisher = new ComboBox
            {
                Location = new Point(fldX, y),
                Size = new Size(200, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(cmbPublisher);
            y += 30;

            AddField("Год:", out txtYear);
            AddField("Страниц:", out txtPages);
            AddField("Всего экз.:", out txtTotal);
            AddField("Доступно экз.:", out txtAvailable);

            Controls.Add(new Label { Text = "Аннотация:", Location = new Point(lblX, y), AutoSize = true });
            y += 20;
            txtAnnotation = new TextBox
            {
                Location = new Point(fldX, y),
                Size = new Size(fldW, 50),
                Multiline = true
            };
            Controls.Add(txtAnnotation);
            y += 55;

            lblError = new Label
            {
                Text = "",
                ForeColor = Color.Red,
                Location = new Point(fldX, y),
                AutoSize = true,
                Font = new Font("Times New Roman", 9)
            };
            Controls.Add(lblError);
            y += 20;

            var btnSave = new Button
            {
                Text = "Сохранить",
                Size = new Size(110, 34),
                Location = new Point(fldX, y),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            var normalBg = btnSave.BackColor;
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.FromArgb(58, 90, 140);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = normalBg;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);
        }

        private void LoadLookups()
        {
            using var db = new LibraryContext();
            genres = db.Genres.OrderBy(g => g.Name).ToList();
            publishers = db.Publishers.OrderBy(p => p.Name).ToList();

            cmbGenre.Items.Clear();
            foreach (var g in genres) cmbGenre.Items.Add(g.Name);
            if (cmbGenre.Items.Count > 0) cmbGenre.SelectedIndex = 0;

            cmbPublisher.Items.Clear();
            foreach (var p in publishers) cmbPublisher.Items.Add(p.Name);
            if (cmbPublisher.Items.Count > 0) cmbPublisher.SelectedIndex = 0;
        }

        private void FillFields()
        {
            if (editingBook == null) return;
            txtIsbn.Text = editingBook.Isbn;
            txtTitle.Text = editingBook.Title;
            txtAuthor.Text = editingBook.Author;
            txtYear.Text = editingBook.YearPublished.ToString();
            txtPages.Text = editingBook.Pages.ToString();
            txtTotal.Text = editingBook.TotalCopies.ToString();
            txtAvailable.Text = editingBook.AvailableCopies.ToString();
            txtAnnotation.Text = editingBook.Annotation ?? "";

            for (int i = 0; i < genres.Count; i++)
                if (genres[i].Id == editingBook.GenreId) { cmbGenre.SelectedIndex = i; break; }
            for (int i = 0; i < publishers.Count; i++)
                if (publishers[i].Id == editingBook.PublisherId) { cmbPublisher.SelectedIndex = i; break; }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtIsbn.Text) || string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                lblError.Text = "Заполните обязательные поля";
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || !int.TryParse(txtPages.Text, out int pages) ||
                !int.TryParse(txtTotal.Text, out int total) || !int.TryParse(txtAvailable.Text, out int avail))
            {
                lblError.Text = "Проверьте числовые поля";
                return;
            }

            using var db = new LibraryContext();

            Book book;
            if (editingBook != null)
            {
                book = db.Books.Find(editingBook.Id)!;
            }
            else
            {
                book = new Book();
                db.Books.Add(book);
            }

            book.Isbn = txtIsbn.Text.Trim();
            book.Title = txtTitle.Text.Trim();
            book.Author = txtAuthor.Text.Trim();
            book.GenreId = genres[cmbGenre.SelectedIndex].Id;
            book.PublisherId = publishers[cmbPublisher.SelectedIndex].Id;
            book.YearPublished = year;
            book.Pages = pages;
            book.TotalCopies = total;
            book.AvailableCopies = avail;
            book.Annotation = string.IsNullOrWhiteSpace(txtAnnotation.Text) ? null : txtAnnotation.Text.Trim();

            db.SaveChanges();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
