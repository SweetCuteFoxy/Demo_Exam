namespace ObutvShop;

static class Program
{
    [STAThread]
    static void Main()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        ApplicationConfiguration.Initialize();

        using var formLogin = new FormLogin();
        if (formLogin.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new FormProducts(formLogin.AuthenticatedUser));
        }
    }
}