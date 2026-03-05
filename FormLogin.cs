using Microsoft.EntityFrameworkCore;
using ObutvShop.Models;

namespace ObutvShop;

public partial class FormLogin : Form
{
    public User? AuthenticatedUser { get; private set; }

    public FormLogin()
    {
        InitializeComponent();
        AcceptButton = buttonLogin;
        CenterPanel();
        Resize += (_, _) => CenterPanel();
    }

    private void CenterPanel()
    {
        panelMain.Left = (ClientSize.Width - panelMain.Width) / 2;
        panelMain.Top = (ClientSize.Height - panelMain.Height) / 2;
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
            using var db = new ObutvShopContext();
            var user = db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                labelError.Text = "Неверный логин или пароль";
                return;
            }

            if (!user.IsActive)
            {
                labelError.Text = "Учётная запись заблокирована";
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
