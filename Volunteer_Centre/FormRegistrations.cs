using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormRegistrations : Form
    {
        private int eventId;
        private User? currentUser;
        private DataGridView dgv = null!;
        private DataGridView dgvAttendance = null!;
        private Label lblTitle = null!;
        private List<VolunteerRegistration> allRegs = new();

        public FormRegistrations(int eventId, User? user)
        {
            this.eventId = eventId;
            currentUser = user;
            InitUI();
            LoadRegs();
        }

        private void InitUI()
        {
            Text = "Заявки волонтеров";
            Size = new Size(850, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            string icoPath = Path.Combine(Application.StartupPath, "app.ico");
            if (File.Exists(icoPath))
                try { Icon = new Icon(icoPath); } catch { }

            var top = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.FromArgb(240, 255, 240) };
            Controls.Add(top);

            lblTitle = new Label
            {
                Text = "",
                Location = new Point(10, 5),
                AutoSize = true,
                Font = new Font("Times New Roman", 11, FontStyle.Bold)
            };
            top.Controls.Add(lblTitle);

            string role = currentUser?.Role?.Name ?? "";
            bool canManage = role == "Координатор" || role == "Администратор";

            if (canManage)
            {
                var btnApprove = new Button
                {
                    Text = "Подтвердить",
                    Size = new Size(110, 26),
                    Location = new Point(10, 40),
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnApprove.FlatAppearance.BorderSize = 0;
                btnApprove.Click += (s, e) => ChangeStatus("Подтверждено");
                top.Controls.Add(btnApprove);

                var btnReject = new Button
                {
                    Text = "Отклонить",
                    Size = new Size(100, 26),
                    Location = new Point(128, 40),
                    BackColor = Color.IndianRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Click += (s, e) => ChangeStatus("Отклонено");
                top.Controls.Add(btnReject);

                var btnComplete = new Button
                {
                    Text = "Завершено",
                    Size = new Size(100, 26),
                    Location = new Point(236, 40),
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnComplete.FlatAppearance.BorderSize = 0;
                btnComplete.Click += (s, e) => ChangeStatus("Завершено");
                top.Controls.Add(btnComplete);
            }

            var splitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 230,
                BackColor = Color.White
            };
            Controls.Add(splitter);
            splitter.BringToFront();

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
            splitter.Panel1.Controls.Add(dgv);

            var lblAtt = new Label
            {
                Text = "Посещаемость",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                BackColor = Color.FromArgb(240, 255, 240),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            splitter.Panel2.Controls.Add(lblAtt);

            dgvAttendance = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            splitter.Panel2.Controls.Add(dgvAttendance);
            dgvAttendance.BringToFront();

            if (canManage)
            {
                var btnSaveAtt = new Button
                {
                    Text = "Сохранить посещаемость",
                    Size = new Size(180, 28),
                    Dock = DockStyle.Bottom,
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnSaveAtt.FlatAppearance.BorderSize = 0;
                btnSaveAtt.Click += BtnSaveAttendance_Click;
                splitter.Panel2.Controls.Add(btnSaveAtt);
            }
        }

        private void LoadRegs()
        {
            using var db = new VolunteerContext();
            var ev = db.Events.FirstOrDefault(e => e.Id == eventId);
            lblTitle.Text = ev != null ? $"Мероприятие: {ev.Title}" : "";

            allRegs = db.Registrations
                .Include(r => r.Volunteer)
                .Include(r => r.Status)
                .Include(r => r.Attendance)
                .Where(r => r.EventId == eventId)
                .OrderBy(r => r.RegistrationDate)
                .ToList();

            dgv.DataSource = allRegs.Select(r => new
            {
                ID = r.Id,
                Волонтер = r.Volunteer?.FullName ?? "",
                Email = r.Volunteer?.Email ?? "",
                Дата_заявки = r.RegistrationDate.ToShortDateString(),
                Статус = r.Status?.Name ?? ""
            }).ToList();

            LoadAttendance();
        }

        private void LoadAttendance()
        {
            var confirmed = allRegs
                .Where(r => r.Status?.Name == "Подтверждено" || r.Status?.Name == "Завершено")
                .ToList();

            var attData = confirmed.Select(r => new AttendanceRow
            {
                RegId = r.Id,
                Волонтер = r.Volunteer?.FullName ?? "",
                Присутствовал = r.Attendance?.Attended ?? false,
                Заметки = r.Attendance?.Notes ?? ""
            }).ToList();

            dgvAttendance.DataSource = attData;
            dgvAttendance.Columns["RegId"].Visible = false;
            dgvAttendance.Columns["Волонтер"].ReadOnly = true;
        }

        private void BtnSaveAttendance_Click(object? sender, EventArgs e)
        {
            if (dgvAttendance.DataSource is not List<AttendanceRow> rows) return;

            using var db = new VolunteerContext();
            foreach (var row in rows)
            {
                var existing = db.Attendances.FirstOrDefault(a => a.RegistrationId == row.RegId);
                if (existing != null)
                {
                    existing.Attended = row.Присутствовал;
                    existing.Notes = string.IsNullOrWhiteSpace(row.Заметки) ? null : row.Заметки;
                }
                else
                {
                    db.Attendances.Add(new Attendance
                    {
                        RegistrationId = row.RegId,
                        Attended = row.Присутствовал,
                        Notes = string.IsNullOrWhiteSpace(row.Заметки) ? null : row.Заметки
                    });
                }
            }
            db.SaveChanges();
            MessageBox.Show("Посещаемость сохранена", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChangeStatus(string statusName)
        {
            if (dgv.SelectedRows.Count == 0) return;
            int id = (int)dgv.SelectedRows[0].Cells["ID"].Value;

            using var db = new VolunteerContext();
            var reg = db.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) return;

            var status = db.RegistrationStatuses.FirstOrDefault(s => s.Name == statusName);
            if (status == null) return;

            reg.StatusId = status.Id;
            db.SaveChanges();
            LoadRegs();
        }

        private class AttendanceRow
        {
            public int RegId { get; set; }
            public string Волонтер { get; set; } = "";
            public bool Присутствовал { get; set; }
            public string Заметки { get; set; } = "";
        }
    }
}
