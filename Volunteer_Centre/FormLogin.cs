using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormLogin : Form
    {
        public User? AuthenticatedUser { get; private set; }
        private TextBox txtLogin = null!;
        private TextBox txtPassword = null!;
        private Label lblErr = null!;

        public FormLogin()
        {
            InitUI();
        }

        private void InitUI()
        {
            Text = "Вход в систему";
            Size = new Size(380, 310);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            string icoPath = Path.Combine(Application.StartupPath, "app.ico");
            if (File.Exists(icoPath))
                try { Icon = new Icon(icoPath); } catch { }

            var panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(380, 70),
                BackColor = Color.FromArgb(240, 255, 240)
            };

            string logoPath = Path.Combine(Application.StartupPath, "picture.png");
            if (File.Exists(logoPath))
            {
                try
                {
                    var pic = new PictureBox
                    {
                        Size = new Size(50, 50),
                        Location = new Point(50, 10),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = Image.FromFile(logoPath)
                    };
                    panel.Controls.Add(pic);
                }
                catch { }
            }

            var title = new Label
            {
                Text = "Доброе сердце",
                Font = new Font("Times New Roman", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                AutoSize = true,
                Location = new Point(110, 22)
            };
            panel.Controls.Add(title);
            Controls.Add(panel);

            Controls.Add(new Label { Text = "Логин:", Location = new Point(30, 85), AutoSize = true });
            txtLogin = new TextBox { Location = new Point(30, 105), Size = new Size(300, 24) };
            Controls.Add(txtLogin);

            Controls.Add(new Label { Text = "Пароль:", Location = new Point(30, 135), AutoSize = true });
            txtPassword = new TextBox { Location = new Point(30, 155), Size = new Size(300, 24), UseSystemPasswordChar = true };
            Controls.Add(txtPassword);

            lblErr = new Label { Text = "", ForeColor = Color.Red, Location = new Point(30, 185), AutoSize = true };
            Controls.Add(lblErr);

            var btnLogin = new Button
            {
                Text = "Войти",
                Size = new Size(90, 30),
                Location = new Point(30, 215),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            Controls.Add(btnLogin);

            var btnGuest = new Button
            {
                Text = "Гость",
                Size = new Size(90, 30),
                Location = new Point(130, 215)
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
            lblErr.Text = "";
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblErr.Text = "Введите логин и пароль";
                return;
            }

            using var db = new VolunteerContext();
            var user = db.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.PasswordText == password);

            if (user == null)
            {
                lblErr.Text = "Неверный логин или пароль";
                return;
            }

            AuthenticatedUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
