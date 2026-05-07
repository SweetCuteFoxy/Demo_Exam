using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormEditEvent : Form
    {
        private VolunteerEvent? editing;
        private User? currentUser;
        private TextBox txtTitle = null!;
        private ComboBox cmbCategory = null!;
        private DateTimePicker dtpDate = null!;
        private ComboBox cmbLocation = null!;
        private TextBox txtNeeded = null!;
        private ComboBox cmbCoordinator = null!;
        private ComboBox cmbStatus = null!;
        private Label lblErr = null!;

        private List<EventCategory> categories = new();
        private List<Location> locations = new();
        private List<User> coordinators = new();
        private List<EventStatus> statuses = new();

        public FormEditEvent(VolunteerEvent? ev, User? user)
        {
            editing = ev;
            currentUser = user;
            InitUI();
            LoadData();
            if (ev != null) FillFields();
        }

        private void InitUI()
        {
            Text = editing == null ? "Новое мероприятие" : "Редактирование";
            Size = new Size(420, 420);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            int y = 10;

            Controls.Add(new Label { Text = "Название:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            txtTitle = new TextBox { Location = new Point(15, y), Size = new Size(370, 24) };
            Controls.Add(txtTitle); y += 28;

            Controls.Add(new Label { Text = "Категория:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbCategory = new ComboBox { Location = new Point(15, y), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbCategory); y += 28;

            Controls.Add(new Label { Text = "Дата:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            dtpDate = new DateTimePicker { Location = new Point(15, y), Size = new Size(180, 24) };
            Controls.Add(dtpDate); y += 28;

            Controls.Add(new Label { Text = "Место:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbLocation = new ComboBox { Location = new Point(15, y), Size = new Size(370, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbLocation); y += 28;

            Controls.Add(new Label { Text = "Нужно волонтеров:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            txtNeeded = new TextBox { Location = new Point(15, y), Size = new Size(100, 24) };
            Controls.Add(txtNeeded); y += 28;

            Controls.Add(new Label { Text = "Координатор:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbCoordinator = new ComboBox { Location = new Point(15, y), Size = new Size(250, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbCoordinator); y += 28;

            Controls.Add(new Label { Text = "Статус:", Location = new Point(15, y), AutoSize = true });
            y += 18;
            cmbStatus = new ComboBox { Location = new Point(15, y), Size = new Size(180, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cmbStatus); y += 28;

            lblErr = new Label { Text = "", ForeColor = Color.Red, Location = new Point(15, y), AutoSize = true };
            Controls.Add(lblErr); y += 20;

            var btnSave = new Button
            {
                Text = "Сохранить",
                Size = new Size(100, 30),
                Location = new Point(15, y),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            var btnCancel = new Button { Text = "Отмена", Size = new Size(80, 30), Location = new Point(125, y) };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            using var db = new VolunteerContext();
            categories = db.EventCategories.OrderBy(c => c.Name).ToList();
            locations = db.Locations.OrderBy(l => l.Name).ToList();
            coordinators = db.Users.Include(u => u.Role)
                .Where(u => u.Role!.Name == "Координатор" || u.Role!.Name == "Администратор")
                .OrderBy(u => u.FullName).ToList();
            statuses = db.EventStatuses.OrderBy(s => s.Id).ToList();

            foreach (var c in categories) cmbCategory.Items.Add(c.Name);
            if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;

            foreach (var l in locations) cmbLocation.Items.Add(l.Name);
            if (cmbLocation.Items.Count > 0) cmbLocation.SelectedIndex = 0;

            foreach (var u in coordinators) cmbCoordinator.Items.Add(u.FullName);
            if (cmbCoordinator.Items.Count > 0) cmbCoordinator.SelectedIndex = 0;

            foreach (var s in statuses) cmbStatus.Items.Add(s.Name);
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
        }

        private void FillFields()
        {
            if (editing == null) return;
            txtTitle.Text = editing.Title;
            dtpDate.Value = editing.EventDate;
            txtNeeded.Text = editing.VolunteersNeeded.ToString();

            for (int i = 0; i < categories.Count; i++)
                if (categories[i].Id == editing.CategoryId) { cmbCategory.SelectedIndex = i; break; }
            for (int i = 0; i < locations.Count; i++)
                if (locations[i].Id == editing.LocationId) { cmbLocation.SelectedIndex = i; break; }
            for (int i = 0; i < coordinators.Count; i++)
                if (coordinators[i].Id == editing.CoordinatorId) { cmbCoordinator.SelectedIndex = i; break; }
            for (int i = 0; i < statuses.Count; i++)
                if (statuses[i].Id == editing.StatusId) { cmbStatus.SelectedIndex = i; break; }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            lblErr.Text = "";
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblErr.Text = "Заполните обязательные поля";
                return;
            }
            if (!int.TryParse(txtNeeded.Text, out int needed) || needed <= 0)
            {
                lblErr.Text = "Количество волонтеров должно быть больше 0";
                return;
            }
            if (cmbCategory.SelectedIndex < 0 || cmbLocation.SelectedIndex < 0 || cmbCoordinator.SelectedIndex < 0 || cmbStatus.SelectedIndex < 0)
            {
                lblErr.Text = "Выберите все поля";
                return;
            }

            using var db = new VolunteerContext();
            VolunteerEvent ev;
            if (editing != null)
                ev = db.Events.Find(editing.Id)!;
            else
            {
                ev = new VolunteerEvent();
                db.Events.Add(ev);
            }

            ev.Title = txtTitle.Text.Trim();
            ev.CategoryId = categories[cmbCategory.SelectedIndex].Id;
            ev.EventDate = dtpDate.Value;
            ev.LocationId = locations[cmbLocation.SelectedIndex].Id;
            ev.VolunteersNeeded = needed;
            ev.CoordinatorId = coordinators[cmbCoordinator.SelectedIndex].Id;
            ev.StatusId = statuses[cmbStatus.SelectedIndex].Id;

            db.SaveChanges();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
