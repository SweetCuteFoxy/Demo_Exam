using LibraryV2.Models;

namespace LibraryV2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            ApplicationConfiguration.Initialize();

            while (true)
            {
                using var loginForm = new FormLogin();
                if (loginForm.ShowDialog() != DialogResult.OK)
                    break;

                using var booksForm = new FormBooks(loginForm.AuthenticatedUser);
                if (booksForm.ShowDialog() != DialogResult.Retry)
                    break;
            }
        }
    }
}