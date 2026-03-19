namespace SportShopV22
{
    partial class FormProducts
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
            panelTop = new Panel();
            panelHeader = new Panel();
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            labelUserInfo = new Label();
            buttonLogout = new Button();
            buttonOrders = new Button();
            panelFilters = new Panel();
            textBoxSearch = new TextBox();
            comboBoxSupplier = new ComboBox();
            comboBoxSort = new ComboBox();
            buttonAdd = new Button();
            buttonDelete = new Button();
            dataGridViewProducts = new DataGridView();
            panelBottom = new Panel();
            labelCount = new Label();

            panelTop.SuspendLayout();
            panelHeader.SuspendLayout();
            panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();

            // panelTop
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(panelFilters);
            panelTop.Controls.Add(panelHeader);
            panelTop.Dock = DockStyle.Top;
            panelTop.Padding = new Padding(24, 0, 24, 0);
            panelTop.Size = new Size(1264, 115);

            // panelHeader
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Controls.Add(buttonLogout);
            panelHeader.Controls.Add(buttonOrders);
            panelHeader.Controls.Add(labelUserInfo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Size = new Size(1216, 55);

            // pictureBoxLogo
            pictureBoxLogo.Dock = DockStyle.Left;
            pictureBoxLogo.Size = new Size(50, 55);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.Padding = new Padding(0, 8, 0, 8);
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icon.png");
            if (File.Exists(logoPath))
                try { pictureBoxLogo.Image = Image.FromFile(logoPath); } catch { }

            // labelTitle
            labelTitle.Dock = DockStyle.Left;
            labelTitle.Font = new Font("Times New Roman", 18F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(67, 97, 238);
            labelTitle.Size = new Size(220, 55);
            labelTitle.Text = "  СпортЭкип";
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;

            // labelUserInfo
            labelUserInfo.Dock = DockStyle.Right;
            labelUserInfo.Font = new Font("Times New Roman", 10F);
            labelUserInfo.ForeColor = Color.FromArgb(80, 80, 80);
            labelUserInfo.Size = new Size(300, 55);
            labelUserInfo.TextAlign = ContentAlignment.MiddleRight;

            // buttonLogout
            buttonLogout.Dock = DockStyle.Right;
            buttonLogout.BackColor = Color.White;
            buttonLogout.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            buttonLogout.FlatAppearance.BorderSize = 1;
            buttonLogout.FlatStyle = FlatStyle.Flat;
            buttonLogout.Font = new Font("Times New Roman", 9F);
            buttonLogout.ForeColor = Color.FromArgb(100, 100, 100);
            buttonLogout.Size = new Size(80, 55);
            buttonLogout.Text = "Выход";
            buttonLogout.Cursor = Cursors.Hand;
            buttonLogout.Click += ButtonLogout_Click;

            // buttonOrders
            buttonOrders.Dock = DockStyle.Right;
            buttonOrders.BackColor = Color.FromArgb(67, 97, 238);
            buttonOrders.FlatAppearance.BorderSize = 0;
            buttonOrders.FlatStyle = FlatStyle.Flat;
            buttonOrders.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            buttonOrders.ForeColor = Color.White;
            buttonOrders.Size = new Size(120, 55);
            buttonOrders.Text = "Заказы";
            buttonOrders.Cursor = Cursors.Hand;
            buttonOrders.Click += ButtonOrders_Click;

            // panelFilters
            panelFilters.Dock = DockStyle.Bottom;
            panelFilters.Controls.Add(buttonDelete);
            panelFilters.Controls.Add(buttonAdd);
            panelFilters.Controls.Add(comboBoxSort);
            panelFilters.Controls.Add(comboBoxSupplier);
            panelFilters.Controls.Add(textBoxSearch);
            panelFilters.Size = new Size(1216, 45);

            // textBoxSearch
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Times New Roman", 11F);
            textBoxSearch.Location = new Point(0, 8);
            textBoxSearch.Size = new Size(350, 28);
            textBoxSearch.PlaceholderText = "Поиск по всем текстовым полям...";
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;

            // comboBoxSupplier
            comboBoxSupplier.Font = new Font("Times New Roman", 10F);
            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSupplier.Location = new Point(365, 8);
            comboBoxSupplier.Size = new Size(220, 28);
            comboBoxSupplier.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // comboBoxSort
            comboBoxSort.Font = new Font("Times New Roman", 10F);
            comboBoxSort.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSort.Location = new Point(600, 8);
            comboBoxSort.Size = new Size(180, 28);
            comboBoxSort.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // buttonAdd
            buttonAdd.BackColor = Color.FromArgb(67, 97, 238);
            buttonAdd.FlatAppearance.BorderSize = 0;
            buttonAdd.FlatStyle = FlatStyle.Flat;
            buttonAdd.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            buttonAdd.ForeColor = Color.White;
            buttonAdd.Location = new Point(800, 5);
            buttonAdd.Size = new Size(150, 34);
            buttonAdd.Text = "Добавить товар";
            buttonAdd.Cursor = Cursors.Hand;
            buttonAdd.Click += ButtonAdd_Click;

            // buttonDelete
            buttonDelete.BackColor = Color.FromArgb(220, 53, 69);
            buttonDelete.FlatAppearance.BorderSize = 0;
            buttonDelete.FlatStyle = FlatStyle.Flat;
            buttonDelete.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            buttonDelete.ForeColor = Color.White;
            buttonDelete.Location = new Point(965, 5);
            buttonDelete.Size = new Size(130, 34);
            buttonDelete.Text = "Удалить";
            buttonDelete.Cursor = Cursors.Hand;
            buttonDelete.Click += ButtonDelete_Click;

            // dataGridViewProducts
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.BackgroundColor = Color.FromArgb(248, 249, 250);
            dataGridViewProducts.BorderStyle = BorderStyle.None;
            dataGridViewProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(233, 245, 255),
                Font = new Font("Times New Roman", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                SelectionBackColor = Color.FromArgb(233, 245, 255),
                SelectionForeColor = Color.FromArgb(60, 60, 60),
                Padding = new Padding(8, 4, 8, 4)
            };
            dataGridViewProducts.ColumnHeadersHeight = 40;
            dataGridViewProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewProducts.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Times New Roman", 10F),
                ForeColor = Color.FromArgb(50, 50, 50),
                SelectionBackColor = Color.FromArgb(210, 230, 255),
                SelectionForeColor = Color.Black,
                Padding = new Padding(8, 2, 8, 2)
            };
            dataGridViewProducts.Dock = DockStyle.Fill;
            dataGridViewProducts.EnableHeadersVisualStyles = false;
            dataGridViewProducts.GridColor = Color.FromArgb(230, 230, 230);
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.RowHeadersVisible = false;
            dataGridViewProducts.RowTemplate.Height = 55;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // panelBottom
            panelBottom.BackColor = Color.FromArgb(233, 245, 255);
            panelBottom.Controls.Add(labelCount);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Size = new Size(1264, 38);

            // labelCount
            labelCount.Dock = DockStyle.Fill;
            labelCount.Font = new Font("Times New Roman", 10F);
            labelCount.ForeColor = Color.FromArgb(60, 60, 60);
            labelCount.Padding = new Padding(24, 0, 24, 0);
            labelCount.Text = "Показано 0 из 0 товаров";
            labelCount.TextAlign = ContentAlignment.MiddleLeft;

            // FormProducts
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1264, 681);
            Controls.Add(dataGridViewProducts);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "FormProducts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Товары — СпортЭкип";
            WindowState = FormWindowState.Maximized;

            var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icon.ico");
            if (File.Exists(iconPath))
            {
                using var bmp = new Bitmap(iconPath);
                Icon = System.Drawing.Icon.FromHandle(bmp.GetHicon());
            }

            panelTop.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Panel panelHeader;
        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelUserInfo;
        private Button buttonLogout;
        private Button buttonOrders;
        private Panel panelFilters;
        private TextBox textBoxSearch;
        private ComboBox comboBoxSupplier;
        private ComboBox comboBoxSort;
        private Button buttonAdd;
        private Button buttonDelete;
        private DataGridView dataGridViewProducts;
        private Panel panelBottom;
        private Label labelCount;
    }
}
