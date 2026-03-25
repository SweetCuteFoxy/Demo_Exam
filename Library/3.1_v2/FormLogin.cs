using LibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2
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
            Text = "Библиотека Читай-Город";
            Size = new Size(460, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var panel = new Panel
            {
                Size = new Size(370, 300),
                Location = new Point(35, 30),
                BackColor = Color.FromArgb(240, 248, 255)
            };
            Controls.Add(panel);

            var lblTitle = new Label
            {
                Text = "Авторизация",
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 111, 165),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(330, 35),
                Location = new Point(20, 15)
            };
            panel.Controls.Add(lblTitle);

            var lblLogin = new Label
            {
                Text = "Логин:",
                Location = new Point(30, 65),
                AutoSize = true,
                Font = new Font("Times New Roman", 10)
            };
            panel.Controls.Add(lblLogin);

            txtLogin = new TextBox
            {
                Location = new Point(30, 88),
                Size = new Size(310, 28),
                Font = new Font("Times New Roman", 11)
            };
            panel.Controls.Add(txtLogin);

            var lblPass = new Label
            {
                Text = "Пароль:",
                Location = new Point(30, 124),
                AutoSize = true,
                Font = new Font("Times New Roman", 10)
            };
            panel.Controls.Add(lblPass);

            txtPassword = new TextBox
            {
                Location = new Point(30, 147),
                Size = new Size(310, 28),
                UseSystemPasswordChar = true,
                Font = new Font("Times New Roman", 11)
            };
            panel.Controls.Add(txtPassword);

            lblError = new Label
            {
                Text = "",
                ForeColor = Color.Red,
                Location = new Point(30, 182),
                Size = new Size(310, 22),
                Font = new Font("Times New Roman", 9)
            };
            panel.Controls.Add(lblError);

            var btnLogin = CreateStyledButton("Войти", new Point(30, 212), new Size(145, 40), true);
            btnLogin.Click += BtnLogin_Click;
            panel.Controls.Add(btnLogin);

            var btnGuest = CreateStyledButton("Гость", new Point(195, 212), new Size(145, 40), false);
            btnGuest.Click += BtnGuest_Click;
            panel.Controls.Add(btnGuest);

            AcceptButton = btnLogin;
        }

        private Button CreateStyledButton(string text, Point loc, Size size, bool primary)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Times New Roman", 11, primary ? FontStyle.Bold : FontStyle.Regular),
                Cursor = Cursors.Hand,
                BackColor = primary ? Color.FromArgb(74, 111, 165) : Color.White,
                ForeColor = primary ? Color.White : Color.FromArgb(74, 111, 165)
            };
            btn.FlatAppearance.BorderSize = primary ? 0 : 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(74, 111, 165);

            var normalBg = btn.BackColor;
            var hoverBg = primary ? Color.FromArgb(58, 90, 140) : Color.FromArgb(235, 243, 255);
            btn.MouseEnter += (s, e) => btn.BackColor = hoverBg;
            btn.MouseLeave += (s, e) => btn.BackColor = normalBg;

            return btn;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Заполните все поля";
                return;
            }

            using var db = new LibraryContext();
            var user = db.Users
                .Include(u => u.Role)
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

        private void BtnGuest_Click(object? sender, EventArgs e)
        {
            AuthenticatedUser = null;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
