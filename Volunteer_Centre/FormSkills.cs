using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormSkills : Form
    {
        private ComboBox cmbVolunteer = null!;
        private CheckedListBox clbSkills = null!;
        private DataGridView dgvOverview = null!;
        private List<User> volunteers = new();
        private List<Skill> skills = new();

        public FormSkills()
        {
            InitUI();
            LoadData();
        }

        private void InitUI()
        {
            Text = "Навыки волонтёров";
            Size = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);

            string icoPath = Path.Combine(Application.StartupPath, "app.ico");
            if (File.Exists(icoPath))
                try { Icon = new Icon(icoPath); } catch { }

            var splitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 280,
                BackColor = Color.White
            };
            Controls.Add(splitter);

            var leftPanel = splitter.Panel1;
            var lblVol = new Label
            {
                Text = "Волонтёр:",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Times New Roman", 10, FontStyle.Bold)
            };
            leftPanel.Controls.Add(lblVol);

            cmbVolunteer = new ComboBox
            {
                Location = new Point(10, 32),
                Size = new Size(250, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbVolunteer.SelectedIndexChanged += (s, e) => LoadVolunteerSkills();
            leftPanel.Controls.Add(cmbVolunteer);

            var lblSkills = new Label
            {
                Text = "Навыки:",
                Location = new Point(10, 65),
                AutoSize = true,
                Font = new Font("Times New Roman", 10, FontStyle.Bold)
            };
            leftPanel.Controls.Add(lblSkills);

            clbSkills = new CheckedListBox
            {
                Location = new Point(10, 87),
                Size = new Size(250, 300),
                CheckOnClick = true
            };
            leftPanel.Controls.Add(clbSkills);

            var btnSave = new Button
            {
                Text = "Сохранить",
                Size = new Size(120, 30),
                Location = new Point(10, 395),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            leftPanel.Controls.Add(btnSave);

            var rightPanel = splitter.Panel2;
            var lblOverview = new Label
            {
                Text = "Обзор навыков",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                BackColor = Color.FromArgb(240, 255, 240),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
            rightPanel.Controls.Add(lblOverview);

            dgvOverview = new DataGridView
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
            rightPanel.Controls.Add(dgvOverview);
            dgvOverview.BringToFront();
        }

        private void LoadData()
        {
            using var db = new VolunteerContext();
            volunteers = db.Users.Include(u => u.Role)
                .Where(u => u.Role!.Name == "Волонтер")
                .OrderBy(u => u.FullName)
                .ToList();
            skills = db.Skills.OrderBy(s => s.Name).ToList();

            foreach (var v in volunteers)
                cmbVolunteer.Items.Add(v.FullName);
            foreach (var s in skills)
                clbSkills.Items.Add(s.Name);

            if (cmbVolunteer.Items.Count > 0)
                cmbVolunteer.SelectedIndex = 0;

            LoadOverview();
        }

        private void LoadVolunteerSkills()
        {
            if (cmbVolunteer.SelectedIndex < 0) return;
            var vol = volunteers[cmbVolunteer.SelectedIndex];

            using var db = new VolunteerContext();
            var volSkillIds = db.VolunteerSkills
                .Where(vs => vs.VolunteerId == vol.Id)
                .Select(vs => vs.SkillId)
                .ToHashSet();

            for (int i = 0; i < skills.Count; i++)
                clbSkills.SetItemChecked(i, volSkillIds.Contains(skills[i].Id));
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cmbVolunteer.SelectedIndex < 0) return;
            var vol = volunteers[cmbVolunteer.SelectedIndex];

            using var db = new VolunteerContext();
            var existing = db.VolunteerSkills.Where(vs => vs.VolunteerId == vol.Id).ToList();
            db.VolunteerSkills.RemoveRange(existing);

            for (int i = 0; i < skills.Count; i++)
            {
                if (clbSkills.GetItemChecked(i))
                {
                    db.VolunteerSkills.Add(new VolunteerSkill
                    {
                        VolunteerId = vol.Id,
                        SkillId = skills[i].Id
                    });
                }
            }

            db.SaveChanges();
            LoadOverview();
            MessageBox.Show("Навыки сохранены", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadOverview()
        {
            using var db = new VolunteerContext();
            var data = db.VolunteerSkills
                .Include(vs => vs.Volunteer)
                .Include(vs => vs.Skill)
                .OrderBy(vs => vs.Volunteer!.FullName)
                .ThenBy(vs => vs.Skill!.Name)
                .Select(vs => new
                {
                    Волонтёр = vs.Volunteer!.FullName,
                    Навык = vs.Skill!.Name
                })
                .ToList();

            dgvOverview.DataSource = data;
        }
    }
}
