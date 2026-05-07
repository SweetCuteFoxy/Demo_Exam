using VolunteerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace VolunteerApp
{
    public class FormEvents : Form
    {
        private User? currentUser;
        private FlowLayoutPanel flowPanel = null!;
        private TextBox txtSearch = null!;
        private ComboBox cmbCategory = null!;
        private ComboBox cmbStatus = null!;
        private Label lblUserName = null!;
        private Label lblCount = null!;
        private List<VolunteerEvent> allEvents = new();
        private Panel? selectedCard = null;
        private int selectedEventId = -1;
        private Image? logoImage;

        public FormEvents(User? user)
        {
            currentUser = user;
            LoadLogo();
            InitUI();
            LoadEvents();
        }

        private void LoadLogo()
        {
            string path = Path.Combine(Application.StartupPath, "picture.png");
            if (File.Exists(path))
            {
                try { logoImage = Image.FromFile(path); } catch { }
            }
        }

        private void InitUI()
        {
            Text = "Мероприятия — Доброе сердце";
            Size = new Size(1050, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            Font = new Font("Times New Roman", 10);
            MinimumSize = new Size(850, 450);

            string icoPath = Path.Combine(Application.StartupPath, "app.ico");
            if (File.Exists(icoPath))
                try { Icon = new Icon(icoPath); } catch { }

            string role = currentUser?.Role?.Name ?? "Гость";
            bool isGuest = currentUser == null;
            bool isVolunteer = role == "Волонтер";
            bool isCoord = role == "Координатор";
            bool isAdmin = role == "Администратор";

            var top = new Panel { Dock = DockStyle.Top, Height = isGuest ? 45 : 80, BackColor = Color.FromArgb(240, 255, 240) };
            Controls.Add(top);

            if (!isGuest)
            {
                txtSearch = new TextBox
                {
                    Location = new Point(10, 10),
                    Size = new Size(170, 24),
                    PlaceholderText = "Поиск..."
                };
                txtSearch.TextChanged += (s, e) => Filter();
                top.Controls.Add(txtSearch);

                cmbCategory = new ComboBox
                {
                    Location = new Point(190, 10),
                    Size = new Size(120, 24),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbCategory.Items.Add("Все категории");
                using (var db = new VolunteerContext())
                    foreach (var c in db.EventCategories.OrderBy(c => c.Name))
                        cmbCategory.Items.Add(c.Name);
                cmbCategory.SelectedIndex = 0;
                cmbCategory.SelectedIndexChanged += (s, e) => Filter();
                top.Controls.Add(cmbCategory);

                cmbStatus = new ComboBox
                {
                    Location = new Point(320, 10),
                    Size = new Size(120, 24),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbStatus.Items.Add("Все статусы");
                cmbStatus.Items.AddRange(new object[] { "Запланировано", "В процессе", "Завершено", "Отменено" });
                cmbStatus.SelectedIndex = 0;
                cmbStatus.SelectedIndexChanged += (s, e) => Filter();
                top.Controls.Add(cmbStatus);
            }

            int bx = 10;

            if (isCoord || isAdmin)
            {
                MakeBtn(top, "Добавить", bx, 88, Color.FromArgb(76, 175, 80), 46).Click +=
                    (s, e) => { if (new FormEditEvent(null, currentUser).ShowDialog() == DialogResult.OK) LoadEvents(); };
                MakeBtn(top, "Изменить", bx + 93, 88, Color.FromArgb(76, 175, 80), 46).Click += BtnEdit_Click;
                MakeBtn(top, "Удалить", bx + 186, 78, Color.IndianRed, 46).Click += BtnDelete_Click;
                MakeBtn(top, "Заявки", bx + 269, 75, Color.FromArgb(76, 175, 80), 46).Click += BtnRegs_Click;
                MakeBtn(top, "Навыки", bx + 349, 75, Color.FromArgb(76, 175, 80), 46).Click +=
                    (s, e) => new FormSkills().ShowDialog();
            }

            if (isVolunteer)
            {
                MakeBtn(top, "Записаться", bx, 100, Color.FromArgb(76, 175, 80), 46).Click += BtnSignUp_Click;
                MakeBtn(top, "Мои мероприятия", bx + 108, 135, Color.FromArgb(76, 175, 80), 46).Click +=
                    (s, e) => new FormMyEvents(currentUser!).ShowDialog();
            }

            if (!isGuest)
            {
                var btnLogout = new Button
                {
                    Text = "Выход",
                    Size = new Size(65, 26),
                    Location = new Point(ClientSize.Width - 75, 9),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnLogout.Click += (s, e) => { DialogResult = DialogResult.Retry; Close(); };
                top.Controls.Add(btnLogout);
            }

            lblUserName = new Label
            {
                Text = currentUser != null ? currentUser.FullName : "Гость",
                AutoSize = true,
                ForeColor = Color.FromArgb(76, 175, 80),
                Font = new Font("Times New Roman", 9)
            };
            lblUserName.Location = new Point(ClientSize.Width - (isGuest ? 75 : 150) - lblUserName.PreferredWidth, 14);
            lblUserName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            top.Controls.Add(lblUserName);

            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 25, BackColor = Color.FromArgb(240, 255, 240) };
            Controls.Add(bottom);
            lblCount = new Label { Text = "", Location = new Point(10, 4), AutoSize = true, Font = new Font("Times New Roman", 9) };
            bottom.Controls.Add(lblCount);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(5)
            };
            Controls.Add(flowPanel);
            flowPanel.BringToFront();

            Resize += (s, e) => UpdateCardWidths();
        }

        private Button MakeBtn(Panel parent, string text, int x, int w, Color bg, int y = 9)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, 26),
                Location = new Point(x, y),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            parent.Controls.Add(btn);
            return btn;
        }

        private void LoadEvents()
        {
            using var db = new VolunteerContext();
            allEvents = db.Events
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Coordinator)
                .Include(e => e.Status)
                .Include(e => e.Registrations)
                .OrderBy(e => e.EventDate)
                .ToList();
            Filter();
        }

        private void Filter()
        {
            var list = allEvents.AsEnumerable();
            bool isGuest = currentUser == null;

            if (!isGuest && txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var q = txtSearch.Text.Trim().ToLower();
                list = list.Where(e =>
                    e.Title.ToLower().Contains(q)
                    || (e.Location?.Name ?? "").ToLower().Contains(q)
                    || (e.Coordinator?.FullName ?? "").ToLower().Contains(q));
            }

            if (!isGuest && cmbCategory != null && cmbCategory.SelectedIndex > 0)
            {
                string cat = cmbCategory.SelectedItem?.ToString() ?? "";
                list = list.Where(e => e.Category?.Name == cat);
            }

            if (!isGuest && cmbStatus != null && cmbStatus.SelectedIndex > 0)
            {
                string st = cmbStatus.SelectedItem?.ToString() ?? "";
                list = list.Where(e => e.Status?.Name == st);
            }

            var filtered = list.ToList();
            RebuildCards(filtered);
            lblCount.Text = $"Показано {filtered.Count} из {allEvents.Count}";
        }

        private void RebuildCards(List<VolunteerEvent> events)
        {
            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();
            selectedCard = null;
            selectedEventId = -1;

            int cardW = flowPanel.ClientSize.Width - 30;
            if (cardW < 500) cardW = 500;

            foreach (var ev in events)
                flowPanel.Controls.Add(CreateCard(ev, cardW));

            flowPanel.ResumeLayout();
        }

        private Panel CreateCard(VolunteerEvent ev, int cardW)
        {
            int confirmed = ev.Registrations.Count(r => r.StatusId == 2 || r.StatusId == 5);
            int free = Math.Max(0, ev.VolunteersNeeded - confirmed);
            double pct = ev.VolunteersNeeded > 0 ? (double)confirmed / ev.VolunteersNeeded * 100 : 0;

            var card = new Panel
            {
                Width = cardW,
                Height = 140,
                Margin = new Padding(3, 2, 3, 4),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = ev.Id,
                Cursor = Cursors.Hand
            };

            Color textColor = Color.Black;
            Color cardBg = Color.White;

            if (ev.Status?.Name == "Отменено")
                cardBg = Color.FromArgb(255, 182, 193); // #FFB6C1
            else if (ev.Status?.Name == "Завершено")
                cardBg = Color.FromArgb(224, 224, 224); // #E0E0E0
            else if (free > 0 && free < 3)
                cardBg = Color.FromArgb(255, 229, 180); // #FFE5B4

            card.BackColor = cardBg;
            card.AccessibleDescription = cardBg.ToArgb().ToString();

            var pic = new PictureBox
            {
                Size = new Size(100, 110),
                Location = new Point(10, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            if (logoImage != null)
                pic.Image = logoImage;
            else
            {
                pic.BackColor = Color.FromArgb(240, 240, 240);
            }
            card.Controls.Add(pic);

            var lblTitle = new Label
            {
                Text = $"{ev.Category?.Name ?? "—"} | {ev.Title}",
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(120, 8),
                AutoSize = true,
                MaximumSize = new Size(cardW - 290, 0)
            };
            card.Controls.Add(lblTitle);

            int y = 34;
            AddLabel(card, $"Координатор: {ev.Coordinator?.FullName ?? "—"}", 120, y, textColor);
            y += 20;
            AddLabel(card, $"Дата: {ev.EventDate:dd.MM.yyyy}", 120, y, textColor);
            y += 20;
            AddLabel(card, $"Место: {ev.Location?.Name ?? "—"}", 120, y, textColor);
            y += 20;

            if (ev.Status?.Name == "Завершено" && confirmed < ev.VolunteersNeeded)
            {
                var lbl1 = AddLabel(card, "Волонтёров: ", 120, y, textColor);
                int offX = 120 + lbl1.PreferredWidth;

                var lblOld = new Label
                {
                    Text = ev.VolunteersNeeded.ToString(),
                    Font = new Font("Times New Roman", 10, FontStyle.Strikeout),
                    ForeColor = Color.Red,
                    Location = new Point(offX, y),
                    AutoSize = true
                };
                card.Controls.Add(lblOld);

                var lblNew = new Label
                {
                    Text = $" {confirmed}",
                    Font = new Font("Times New Roman", 10, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(offX + lblOld.PreferredWidth, y),
                    AutoSize = true
                };
                card.Controls.Add(lblNew);
            }
            else
            {
                AddLabel(card, $"Волонтёров нужно: {ev.VolunteersNeeded}   Записано: {confirmed}", 120, y, textColor);
            }

            var lblStatus = new Label
            {
                Text = ev.Status?.Name ?? "",
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                BackColor = Color.FromArgb(240, 255, 240),
                Location = new Point(cardW - 160, 10),
                Size = new Size(135, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "right"
            };
            card.Controls.Add(lblStatus);

            var lblPct = new Label
            {
                Text = $"Набор: {pct:F0}%",
                Font = new Font("Times New Roman", 10),
                ForeColor = textColor,
                Location = new Point(cardW - 160, 40),
                AutoSize = true,
                Tag = "right"
            };
            card.Controls.Add(lblPct);

            var lblFree = new Label
            {
                Text = $"Свободно мест: {free}",
                Font = new Font("Times New Roman", 10),
                ForeColor = textColor,
                Location = new Point(cardW - 160, 60),
                AutoSize = true,
                Tag = "right"
            };
            card.Controls.Add(lblFree);

            Color hoverBg = ControlPaint.Dark(cardBg, -0.05f);
            void OnEnter(object? s, EventArgs ea) { card.BackColor = hoverBg; }
            void OnLeave(object? s, EventArgs ea)
            {
                card.BackColor = card == selectedCard
                    ? Color.FromArgb(240, 255, 240) : cardBg;
            }
            card.MouseEnter += OnEnter;
            card.MouseLeave += OnLeave;
            foreach (Control c in card.Controls)
            {
                c.MouseEnter += OnEnter;
                c.MouseLeave += OnLeave;
            }

            card.Click += (s, e) => SelectCard(card);
            foreach (Control c in card.Controls)
                c.Click += (s, e) => SelectCard(card);

            return card;
        }

        private Label AddLabel(Panel card, string text, int x, int y, Color color)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Times New Roman", 10),
                ForeColor = color,
                Location = new Point(x, y),
                AutoSize = true
            };
            card.Controls.Add(lbl);
            return lbl;
        }

        private void SelectCard(Panel card)
        {
            if (selectedCard != null)
            {
                selectedCard.BorderStyle = BorderStyle.FixedSingle;
                if (selectedCard.AccessibleDescription != null)
                {
                    int argb = int.Parse(selectedCard.AccessibleDescription);
                    selectedCard.BackColor = Color.FromArgb(argb);
                }
            }

            selectedCard = card;
            selectedEventId = (int)card.Tag!;
            card.BorderStyle = BorderStyle.Fixed3D;
            card.BackColor = Color.FromArgb(240, 255, 240);
        }

        private void UpdateCardWidths()
        {
            int cardW = flowPanel.ClientSize.Width - 30;
            if (cardW < 500) cardW = 500;

            foreach (Control c in flowPanel.Controls)
            {
                if (c is Panel card)
                {
                    card.Width = cardW;
                    foreach (Control child in card.Controls)
                    {
                        if (child.Tag?.ToString() == "right")
                            child.Location = new Point(cardW - 160, child.Location.Y);
                    }
                }
            }
        }

        private void BtnEdit_Click(object? s, EventArgs e)
        {
            if (selectedEventId < 0)
            {
                MessageBox.Show("Выберите мероприятие");
                return;
            }
            using var db = new VolunteerContext();
            var ev = db.Events.Include(x => x.Category).Include(x => x.Location)
                .Include(x => x.Coordinator).Include(x => x.Status)
                .FirstOrDefault(x => x.Id == selectedEventId);
            if (ev == null) return;

            if (new FormEditEvent(ev, currentUser).ShowDialog() == DialogResult.OK)
                LoadEvents();
        }

        private void BtnDelete_Click(object? s, EventArgs e)
        {
            if (selectedEventId < 0)
            {
                MessageBox.Show("Выберите мероприятие");
                return;
            }

            if (MessageBox.Show("Удалить мероприятие?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var db = new VolunteerContext();
            var ev = db.Events
                .Include(x => x.Registrations).ThenInclude(r => r.Attendance)
                .FirstOrDefault(x => x.Id == selectedEventId);
            if (ev == null) return;

            foreach (var reg in ev.Registrations)
                if (reg.Attendance != null)
                    db.Attendances.Remove(reg.Attendance);
            db.Registrations.RemoveRange(ev.Registrations);
            db.Events.Remove(ev);
            db.SaveChanges();
            LoadEvents();
        }

        private void BtnRegs_Click(object? s, EventArgs e)
        {
            if (selectedEventId < 0)
            {
                MessageBox.Show("Выберите мероприятие");
                return;
            }
            new FormRegistrations(selectedEventId, currentUser).ShowDialog();
            LoadEvents();
        }

        private void BtnSignUp_Click(object? s, EventArgs e)
        {
            if (selectedEventId < 0)
            {
                MessageBox.Show("Выберите мероприятие");
                return;
            }

            using var db = new VolunteerContext();
            var ev = db.Events.Include(x => x.Status).FirstOrDefault(x => x.Id == selectedEventId);
            if (ev == null) return;

            if (ev.Status?.Name != "Запланировано" && ev.Status?.Name != "В процессе")
            {
                MessageBox.Show("На это мероприятие нельзя записаться");
                return;
            }

            bool already = db.Registrations
                .Any(r => r.EventId == selectedEventId && r.VolunteerId == currentUser!.Id
                    && r.StatusId != 3 && r.StatusId != 4);
            if (already)
            {
                MessageBox.Show("Вы уже записаны на это мероприятие");
                return;
            }

            var pending = db.RegistrationStatuses.FirstOrDefault(s2 => s2.Name == "На рассмотрении");
            if (pending == null) return;

            db.Registrations.Add(new VolunteerRegistration
            {
                EventId = selectedEventId,
                VolunteerId = currentUser!.Id,
                RegistrationDate = DateTime.Now,
                StatusId = pending.Id
            });
            db.SaveChanges();
            MessageBox.Show("Заявка отправлена");
            LoadEvents();
        }
    }
}
