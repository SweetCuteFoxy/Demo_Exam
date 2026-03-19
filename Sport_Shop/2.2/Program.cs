namespace SportShopV22;

static class Program
{
    [STAThread]
    static void Main()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        ApplicationConfiguration.Initialize();

        while (true)
        {
            using var formLogin = new FormLogin();
            if (formLogin.ShowDialog() != DialogResult.OK)
                break;

            var formProducts = new FormProducts(formLogin.AuthenticatedUser);
            Application.Run(formProducts);

            // Если пользователь нажал "Выход" — показать логин снова
            if (formProducts.DialogResult != DialogResult.Abort)
                break;
        }
    }
}
