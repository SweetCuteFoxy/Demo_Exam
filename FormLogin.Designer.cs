namespace ObutvShop
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
            labelLogin = new Label();
            textBoxLogin = new TextBox();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            buttonLogin = new Button();
            buttonGuest = new Button();
            labelError = new Label();

            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();

            // panelMain
            panelMain.Anchor = AnchorStyles.None;
            panelMain.BackColor = Color.White;
            panelMain.BorderStyle = BorderStyle.FixedSingle;
            panelMain.Name = "panelMain";
            panelMain.Controls.Add(pictureBoxLogo);
            panelMain.Controls.Add(labelTitle);
            panelMain.Controls.Add(labelLogin);
            panelMain.Controls.Add(textBoxLogin);
            panelMain.Controls.Add(labelPassword);
            panelMain.Controls.Add(textBoxPassword);
            panelMain.Controls.Add(buttonLogin);
            panelMain.Controls.Add(buttonGuest);
            panelMain.Controls.Add(labelError);
            panelMain.Location = new Point(420, 100);
            panelMain.Size = new Size(400, 500);

            // pictureBoxLogo
            pictureBoxLogo.Anchor = AnchorStyles.Top;
            pictureBoxLogo.Location = new Point(125, 20);
            pictureBoxLogo.Size = new Size(150, 100);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.BackColor = Color.FromArgb(127, 255, 0);

            // labelTitle
            labelTitle.Anchor = AnchorStyles.Top;
            labelTitle.Font = new Font("Times New Roman", 18F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(46, 139, 87);
            labelTitle.Location = new Point(50, 130);
            labelTitle.Size = new Size(300, 40);
            labelTitle.Text = "ООО «Обувь»";
            labelTitle.TextAlign = ContentAlignment.MiddleCenter;

            // labelLogin
            labelLogin.Font = new Font("Times New Roman", 12F);
            labelLogin.Location = new Point(50, 190);
            labelLogin.Size = new Size(300, 25);
            labelLogin.Text = "Логин";

            // textBoxLogin
            textBoxLogin.Font = new Font("Times New Roman", 12F);
            textBoxLogin.Location = new Point(50, 218);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(300, 30);

            // labelPassword
            labelPassword.Font = new Font("Times New Roman", 12F);
            labelPassword.Location = new Point(50, 260);
            labelPassword.Size = new Size(300, 25);
            labelPassword.Text = "Пароль";

            // textBoxPassword
            textBoxPassword.Font = new Font("Times New Roman", 12F);
            textBoxPassword.Location = new Point(50, 288);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(300, 30);
            textBoxPassword.UseSystemPasswordChar = true;

            // buttonLogin
            buttonLogin.BackColor = Color.FromArgb(127, 255, 0);
            buttonLogin.FlatStyle = FlatStyle.Flat;
            buttonLogin.Font = new Font("Times New Roman", 14F, FontStyle.Bold);
            buttonLogin.ForeColor = Color.Black;
            buttonLogin.Location = new Point(50, 340);
            buttonLogin.Size = new Size(300, 45);
            buttonLogin.Text = "Войти";
            buttonLogin.UseVisualStyleBackColor = false;
            buttonLogin.Cursor = Cursors.Hand;
            buttonLogin.Click += ButtonLogin_Click;

            // buttonGuest
            buttonGuest.BackColor = Color.FromArgb(0, 250, 154);
            buttonGuest.FlatStyle = FlatStyle.Flat;
            buttonGuest.Font = new Font("Times New Roman", 12F);
            buttonGuest.ForeColor = Color.Black;
            buttonGuest.Location = new Point(50, 400);
            buttonGuest.Size = new Size(300, 40);
            buttonGuest.Text = "Войти как гость";
            buttonGuest.UseVisualStyleBackColor = false;
            buttonGuest.Cursor = Cursors.Hand;
            buttonGuest.Click += ButtonGuest_Click;

            // labelError
            labelError.Font = new Font("Times New Roman", 10F);
            labelError.ForeColor = Color.Red;
            labelError.Location = new Point(50, 450);
            labelError.Size = new Size(300, 40);
            labelError.Text = "";
            labelError.TextAlign = ContentAlignment.MiddleCenter;

            // FormLogin
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1264, 681);
            Controls.Add(panelMain);
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация — ООО «Обувь»";
            WindowState = FormWindowState.Maximized;

            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelLogin;
        private TextBox textBoxLogin;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Button buttonLogin;
        private Button buttonGuest;
        private Label labelError;
    }
}
