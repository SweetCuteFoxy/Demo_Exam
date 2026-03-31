using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormEditBook : Form
    {
        private Book? editing;
        private TextBox txtIsbn = null!;
        private TextBox txtTitle = null!;
        private ComboBox cmbAuthor = null!;
        private ComboBox cmbGenre = null!;
        private ComboBox cmbPublisher = null!;
        private TextBox txtYear = null!;
        private TextBox txtPages = null!;
        private TextBox txtTotal = null!;
        private TextBox txtAvail = null!;
        private TextBox txtAnnotation = null!;
        private Label lblErr = null!;

        private List<Genre> genres = new();
        private List<Publisher> publishers = new();
        private List<Author> authors = new();

        public FormEditBook(Book? book)
        {
            editing = book;
            InitUI();
            LoadData();
            if (book != null) FillFields();
        }

        private void InitUI()
        {
            Text = editing == null ? "Новая книга" : "Редактирование";
            Size = new Size(420, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            AutoScroll = true;

            int y = 10;

            void Add(string label, out TextBox t)
            {
                Controls.Add(new Label { Text = label, Location = new Point(15, y), AutoSize = true });
                y += 18;
                t = new TextBox { Location = new Point(15, y), Size = new Size(370, 24) };
                Controls.Add(t);
                y += 28;
            }

            Add("ISBN:", out txtIsbn);
            Add("Название:", out txtTitle);

            Controls.Add(new Label { Text = "Автор:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbAuthor = new ComboBox { Location = new Point(15, y), Size = new Size(250, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbAuthor);
            y += 28;

            Controls.Add(new Label { Text = "Жанр:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbGenre = new ComboBox { Location = new Point(15, y), Size = new Size(180, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbGenre);
            y += 28;

            Controls.Add(new Label { Text = "Издательство:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbPublisher = new ComboBox { Location = new Point(15, y), Size = new Size(180, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbPublisher);
            y += 28;

            Add("Год:", out txtYear);
            Add("Страниц:", out txtPages);
            Add("Всего экз.:", out txtTotal);
            Add("Доступно:", out txtAvail);

            Controls.Add(new Label { Text = "Аннотация:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            txtAnnotation = new TextBox { Location = new Point(15, y), Size = new Size(370, 40), Multiline = true };
            Controls.Add(txtAnnotation);
            y += 45;

            lblErr = new Label { Text = "", ForeColor = Color.Red, Location = new Point(15, y), AutoSize = true };
            Controls.Add(lblErr);
            y += 20;

            var btnSave = new Button
            {
                Text = "Сохранить",
                Size = new Size(100, 32),
                Location = new Point(15, y),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(80, 32),
                Location = new Point(125, y)
            };
            btnCancel.Click += (s, e) => Close();
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            using var db = new LibraryContext();
            genres = db.Genres.OrderBy(g => g.Name).ToList();
            publishers = db.Publishers.OrderBy(p => p.Name).ToList();
            authors = db.Authors.OrderBy(a => a.Name).ToList();

            foreach (var a in authors) cmbAuthor.Items.Add(a.Name);
            if (cmbAuthor.Items.Count > 0) cmbAuthor.SelectedIndex = 0;

            foreach (var g in genres) cmbGenre.Items.Add(g.Name);
            if (cmbGenre.Items.Count > 0) cmbGenre.SelectedIndex = 0;

            foreach (var p in publishers) cmbPublisher.Items.Add(p.Name);
            if (cmbPublisher.Items.Count > 0) cmbPublisher.SelectedIndex = 0;
        }

        private void FillFields()
        {
            if (editing == null) return;
            txtIsbn.Text = editing.Isbn;
            txtTitle.Text = editing.Title;
            txtYear.Text = editing.YearPublished.ToString();
            txtPages.Text = editing.Pages.ToString();
            txtTotal.Text = editing.TotalCopies.ToString();
            txtAvail.Text = editing.AvailableCopies.ToString();
            txtAnnotation.Text = editing.Annotation ?? "";

            for (int i = 0; i < genres.Count; i++)
                if (genres[i].Id == editing.GenreId) { cmbGenre.SelectedIndex = i; break; }
            for (int i = 0; i < publishers.Count; i++)
                if (publishers[i].Id == editing.PublisherId) { cmbPublisher.SelectedIndex = i; break; }
            for (int i = 0; i < authors.Count; i++)
                if (authors[i].Id == editing.AuthorId) { cmbAuthor.SelectedIndex = i; break; }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblErr.Text = "";
            if (string.IsNullOrWhiteSpace(txtIsbn.Text) || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblErr.Text = "Заполните обязательные поля";
                return;
            }
            if (!int.TryParse(txtYear.Text, out int year) || !int.TryParse(txtPages.Text, out int pages)
                || !int.TryParse(txtTotal.Text, out int total) || !int.TryParse(txtAvail.Text, out int avail))
            {
                lblErr.Text = "Числовые поля заполнены неверно";
                return;
            }
            if (year < 1 || year > DateTime.Now.Year)
            {
                lblErr.Text = $"Год должен быть от 1 до {DateTime.Now.Year}";
                return;
            }
            if (pages <= 0)
            {
                lblErr.Text = "Количество страниц должно быть больше 0";
                return;
            }
            if (total < 0 || avail < 0)
            {
                lblErr.Text = "Количество экземпляров не может быть отрицательным";
                return;
            }
            if (avail > total)
            {
                lblErr.Text = "Доступных экземпляров не может быть больше общего количества";
                return;
            }

            using var db = new LibraryContext();
            Book book;
            if (editing != null)
                book = db.Books.Find(editing.Id)!;
            else
            {
                book = new Book();
                db.Books.Add(book);
            }

            book.Isbn = txtIsbn.Text.Trim();
            book.Title = txtTitle.Text.Trim();
            book.AuthorId = authors[cmbAuthor.SelectedIndex].Id;
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
