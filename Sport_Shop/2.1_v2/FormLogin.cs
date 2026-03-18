using System.Drawing.Drawing2D;
using Microsoft.EntityFrameworkCore;
using SportShopV2.Models;

namespace SportShopV2;

public partial class FormLogin : Form
{
    public User? AuthenticatedUser { get; private set; }

    public FormLogin()
    {
        InitializeComponent();
        AcceptButton = buttonLogin;
        CenterPanel();
        Resize += (_, _) => CenterPanel();

        panelMain.Paint += PanelMain_Paint;

        buttonLogin.MouseEnter += (_, _) => buttonLogin.BackColor = Color.FromArgb(55, 81, 207);
        buttonLogin.MouseLeave += (_, _) => buttonLogin.BackColor = Color.FromArgb(67, 97, 238);

        buttonGuest.MouseEnter += (_, _) => { buttonGuest.BackColor = Color.FromArgb(67, 97, 238); buttonGuest.ForeColor = Color.White; };
        buttonGuest.MouseLeave += (_, _) => { buttonGuest.BackColor = Color.White; buttonGuest.ForeColor = Color.FromArgb(67, 97, 238); };

        textBoxLogin.Enter += (_, _) => panelLoginLine.BackColor = Color.FromArgb(67, 97, 238);
        textBoxLogin.Leave += (_, _) => panelLoginLine.BackColor = Color.FromArgb(200, 200, 200);
        textBoxPassword.Enter += (_, _) => panelPasswordLine.BackColor = Color.FromArgb(67, 97, 238);
        textBoxPassword.Leave += (_, _) => panelPasswordLine.BackColor = Color.FromArgb(200, 200, 200);
    }

    private void CenterPanel()
    {
        panelMain.Left = (ClientSize.Width - panelMain.Width) / 2;
        panelMain.Top = (ClientSize.Height - panelMain.Height) / 2;
    }

    private void PanelMain_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int r = 16;
        var rect = new Rectangle(0, 0, panelMain.Width - 1, panelMain.Height - 1);
        using var path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, r, r, 180, 90);
        path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
        path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
        path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
        path.CloseFigure();

        panelMain.Region = new Region(path);

        using var shadow = new Pen(Color.FromArgb(40, 0, 0, 0), 1f);
        g.DrawPath(shadow, path);
    }

    private void ButtonLogin_Click(object? sender, EventArgs e)
    {
        labelError.Text = "";

        string login = textBoxLogin.Text.Trim();
        string password = textBoxPassword.Text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            labelError.Text = "Введите логин и пароль";
            return;
        }

        try
        {
            using var db = new SportShopContext();
            var user = db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                labelError.Text = "Неверный логин или пароль";
                return;
            }

            AuthenticatedUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ButtonGuest_Click(object? sender, EventArgs e)
    {
        AuthenticatedUser = null;
        DialogResult = DialogResult.OK;
        Close();
    }
}
