using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormMyEvents : Form
    {
        private User currentUser;
        private DataGridView dgv = null!;
        private Label lblStats = null!;

        public FormMyEvents(User user)
        {
            currentUser = user;
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            Text = "Мои мероприятия";
            Size = new Size(800, 450);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            var top = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(240, 255, 240) };
            Controls.Add(top);

            lblStats = new Label
            {
                Text = "",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80)
            };
            top.Controls.Add(lblStats);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            Controls.Add(dgv);
            dgv.BringToFront();
        }

        private void LoadData()
        {
            using var db = new VolunteerContext();
            var regs = db.Registrations
                .Include(r => r.Event).ThenInclude(e => e!.Category)
                .Include(r => r.Event).ThenInclude(e => e!.Location)
                .Include(r => r.Event).ThenInclude(e => e!.Status)
                .Include(r => r.Status)
                .Where(r => r.VolunteerId == currentUser.Id)
                .OrderByDescending(r => r.Event!.EventDate)
                .ToList();

            int completedCount = regs.Count(r => r.Status?.Name == "Завершено");
            lblStats.Text = $"{currentUser.FullName} | Завершено мероприятий: {completedCount}";

            dgv.DataSource = regs.Select(r => new
            {
                Мероприятие = r.Event?.Title ?? "",
                Категория = r.Event?.Category?.Name ?? "",
                Дата = r.Event?.EventDate.ToShortDateString() ?? "",
                Место = r.Event?.Location?.Name ?? "",
                Статус_мероприятия = r.Event?.Status?.Name ?? "",
                Статус_заявки = r.Status?.Name ?? ""
            }).ToList();
        }
    }
}
