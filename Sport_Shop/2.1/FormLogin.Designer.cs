namespace SportShopV1
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelMain = new Panel();
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            labelSubtitle = new Label();
            labelLogin = new Label();
            textBoxLogin = new TextBox();
            panelLoginLine = new Panel();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            panelPasswordLine = new Panel();
            buttonLogin = new Button();
            buttonGuest = new Button();
            labelError = new Label();

            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();

            // panelMain
            panelMain.Anchor = AnchorStyles.None;
            panelMain.BackColor = Color.White;
            panelMain.Name = "panelMain";
            panelMain.Controls.Add(pictureBoxLogo);
            panelMain.Controls.Add(labelTitle);
            panelMain.Controls.Add(labelSubtitle);
            panelMain.Controls.Add(labelLogin);
            panelMain.Controls.Add(textBoxLogin);
            panelMain.Controls.Add(panelLoginLine);
            panelMain.Controls.Add(labelPassword);
            panelMain.Controls.Add(textBoxPassword);
            panelMain.Controls.Add(panelPasswordLine);
            panelMain.Controls.Add(buttonLogin);
            panelMain.Controls.Add(buttonGuest);
            panelMain.Controls.Add(labelError);
            panelMain.Size = new Size(420, 530);

            // pictureBoxLogo
            pictureBoxLogo.Location = new Point(160, 20);
            pictureBoxLogo.Size = new Size(100, 70);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.BackColor = Color.White;
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icon.ico");
            if (File.Exists(logoPath))
                pictureBoxLogo.Image = Image.FromFile(logoPath);

            // labelTitle
            labelTitle.Font = new Font("Times New Roman", 20F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(0, 250, 154);
            labelTitle.Location = new Point(10, 95);
            labelTitle.Size = new Size(400, 35);
            labelTitle.Text = "СпортЭкип";
            labelTitle.TextAlign = ContentAlignment.MiddleCenter;

            // labelSubtitle
            labelSubtitle.Font = new Font("Times New Roman", 10F);
            labelSubtitle.ForeColor = Color.Gray;
            labelSubtitle.Location = new Point(10, 130);
            labelSubtitle.Size = new Size(400, 22);
            labelSubtitle.Text = "Войдите в систему для продолжения";
            labelSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // labelLogin
            labelLogin.Font = new Font("Times New Roman", 10F);
            labelLogin.ForeColor = Color.Gray;
            labelLogin.Location = new Point(50, 175);
            labelLogin.Size = new Size(320, 20);
            labelLogin.Text = "Логин";

            // textBoxLogin
            textBoxLogin.BorderStyle = BorderStyle.None;
            textBoxLogin.Font = new Font("Times New Roman", 13F);
            textBoxLogin.Location = new Point(50, 198);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(320, 26);

            // panelLoginLine
            panelLoginLine.BackColor = Color.FromArgb(200, 200, 200);
            panelLoginLine.Location = new Point(50, 228);
            panelLoginLine.Size = new Size(320, 1);

            // labelPassword
            labelPassword.Font = new Font("Times New Roman", 10F);
            labelPassword.ForeColor = Color.Gray;
            labelPassword.Location = new Point(50, 248);
            labelPassword.Size = new Size(320, 20);
            labelPassword.Text = "Пароль";

            // textBoxPassword
            textBoxPassword.BorderStyle = BorderStyle.None;
            textBoxPassword.Font = new Font("Times New Roman", 13F);
            textBoxPassword.Location = new Point(50, 271);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(320, 26);
            textBoxPassword.UseSystemPasswordChar = true;

            // panelPasswordLine
            panelPasswordLine.BackColor = Color.FromArgb(200, 200, 200);
            panelPasswordLine.Location = new Point(50, 301);
            panelPasswordLine.Size = new Size(320, 1);

            // buttonLogin
            buttonLogin.BackColor = Color.FromArgb(0, 250, 154);
            buttonLogin.FlatAppearance.BorderSize = 0;
            buttonLogin.FlatStyle = FlatStyle.Flat;
            buttonLogin.Font = new Font("Times New Roman", 13F, FontStyle.Bold);
            buttonLogin.ForeColor = Color.White;
            buttonLogin.Location = new Point(50, 330);
            buttonLogin.Size = new Size(320, 45);
            buttonLogin.Text = "Войти";
            buttonLogin.Cursor = Cursors.Hand;
            buttonLogin.Click += ButtonLogin_Click;

            // buttonGuest
            buttonGuest.BackColor = Color.White;
            buttonGuest.FlatAppearance.BorderColor = Color.FromArgb(0, 250, 154);
            buttonGuest.FlatAppearance.BorderSize = 1;
            buttonGuest.FlatStyle = FlatStyle.Flat;
            buttonGuest.Font = new Font("Times New Roman", 11F);
            buttonGuest.ForeColor = Color.FromArgb(0, 250, 154);
            buttonGuest.Location = new Point(50, 390);
            buttonGuest.Size = new Size(320, 40);
            buttonGuest.Text = "Войти как гость";
            buttonGuest.Cursor = Cursors.Hand;
            buttonGuest.Click += ButtonGuest_Click;

            // labelError
            labelError.Font = new Font("Times New Roman", 10F);
            labelError.ForeColor = Color.FromArgb(220, 50, 50);
            labelError.Location = new Point(50, 445);
            labelError.Size = new Size(320, 50);
            labelError.TextAlign = ContentAlignment.TopCenter;

            // FormLogin
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1264, 681);
            Controls.Add(panelMain);
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация — СпортЭкип";
            WindowState = FormWindowState.Maximized;

            var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icon.ico");
            if (File.Exists(iconPath))
            {
                using var bmp = new Bitmap(iconPath);
                Icon = System.Drawing.Icon.FromHandle(bmp.GetHicon());
            }

            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelSubtitle;
        private Label labelLogin;
        private TextBox textBoxLogin;
        private Panel panelLoginLine;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Panel panelPasswordLine;
        private Button buttonLogin;
        private Button buttonGuest;
        private Label labelError;
    }
}
