using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp
{
    public class FormLogin : Form
    {
        public User? AuthenticatedUser { get; private set; }

        private TextBox txtLogin = null!;
        private TextBox txtPassword = null!;
        private Label lblError = null!;

        public FormLogin()
        {
            InitUI();
        }

        private void InitUI()
        {
            Text = "Читай-Город — Вход";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var lblTitle = new Label
            {
                Text = "Авторизация",
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 111, 165),
                Location = new Point(120, 15),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            Controls.Add(new Label { Text = "Логин:", Location = new Point(40, 60), AutoSize = true });
            txtLogin = new TextBox { Location = new Point(40, 80), Size = new Size(300, 24) };
            Controls.Add(txtLogin);

            Controls.Add(new Label { Text = "Пароль:", Location = new Point(40, 110), AutoSize = true });
            txtPassword = new TextBox { Location = new Point(40, 130), Size = new Size(300, 24), UseSystemPasswordChar = true };
            Controls.Add(txtPassword);

            lblError = new Label
            {
                Text = "",
                ForeColor = Color.Red,
                Location = new Point(40, 162),
                Size = new Size(300, 18),
                Font = new Font("Times New Roman", 9)
            };
            Controls.Add(lblError);

            var btnLogin = new Button
            {
                Text = "Войти",
                Size = new Size(140, 35),
                Location = new Point(40, 190),
                BackColor = Color.FromArgb(74, 111, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            Controls.Add(btnLogin);

            var btnGuest = new Button
            {
                Text = "Войти как гость",
                Size = new Size(140, 35),
                Location = new Point(200, 190),
                Cursor = Cursors.Hand
            };
            btnGuest.Click += (s, e) =>
            {
                AuthenticatedUser = null;
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnGuest);

            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Введите логин и пароль";
                return;
            }

            using var db = new LibraryContext();
            var user = db.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Login == txtLogin.Text.Trim() && u.PasswordText == txtPassword.Text);
            if (user == null)
            {
                lblError.Text = "Неверный логин или пароль";
                return;
            }

            AuthenticatedUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
