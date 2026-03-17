namespace SportShopV1
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
            labelTitle = new Label();
            labelUserInfo = new Label();
            buttonOrders = new Button();
            panelFilters = new Panel();
            textBoxSearch = new TextBox();
            comboBoxCategory = new ComboBox();
            comboBoxManufacturer = new ComboBox();
            comboBoxDiscount = new ComboBox();
            dataGridViewProducts = new DataGridView();
            panelBottom = new Panel();
            labelCount = new Label();

            panelTop.SuspendLayout();
            panelHeader.SuspendLayout();
            panelFilters.SuspendLayout();
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
            panelHeader.Controls.Add(buttonOrders);
            panelHeader.Controls.Add(labelUserInfo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Size = new Size(1216, 55);

            // labelTitle
            labelTitle.Dock = DockStyle.Left;
            labelTitle.Font = new Font("Times New Roman", 18F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(0, 250, 154);
            labelTitle.Size = new Size(200, 55);
            labelTitle.Text = "Товары";
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;

            // labelUserInfo
            labelUserInfo.Dock = DockStyle.Right;
            labelUserInfo.Font = new Font("Times New Roman", 10F);
            labelUserInfo.ForeColor = Color.Gray;
            labelUserInfo.Size = new Size(300, 55);
            labelUserInfo.TextAlign = ContentAlignment.MiddleRight;

            // buttonOrders
            buttonOrders.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonOrders.BackColor = Color.FromArgb(0, 250, 154);
            buttonOrders.FlatAppearance.BorderSize = 0;
            buttonOrders.FlatStyle = FlatStyle.Flat;
            buttonOrders.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            buttonOrders.ForeColor = Color.White;
            buttonOrders.Location = new Point(700, 12);
            buttonOrders.Size = new Size(130, 32);
            buttonOrders.Text = "Заказы";
            buttonOrders.Cursor = Cursors.Hand;
            buttonOrders.Click += ButtonOrders_Click;

            // panelFilters
            panelFilters.Dock = DockStyle.Bottom;
            panelFilters.Controls.Add(comboBoxDiscount);
            panelFilters.Controls.Add(comboBoxManufacturer);
            panelFilters.Controls.Add(comboBoxCategory);
            panelFilters.Controls.Add(textBoxSearch);
            panelFilters.Size = new Size(1216, 45);

            // textBoxSearch
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Times New Roman", 11F);
            textBoxSearch.Location = new Point(0, 8);
            textBoxSearch.Size = new Size(280, 28);
            textBoxSearch.PlaceholderText = "Поиск по названию, артикулу...";
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;

            // comboBoxCategory
            comboBoxCategory.Font = new Font("Times New Roman", 10F);
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCategory.Location = new Point(295, 8);
            comboBoxCategory.Size = new Size(180, 28);
            comboBoxCategory.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // comboBoxManufacturer
            comboBoxManufacturer.Font = new Font("Times New Roman", 10F);
            comboBoxManufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxManufacturer.Location = new Point(490, 8);
            comboBoxManufacturer.Size = new Size(200, 28);
            comboBoxManufacturer.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // comboBoxDiscount
            comboBoxDiscount.Font = new Font("Times New Roman", 10F);
            comboBoxDiscount.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDiscount.Location = new Point(705, 8);
            comboBoxDiscount.Size = new Size(160, 28);
            comboBoxDiscount.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // dataGridViewProducts
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.BackgroundColor = Color.White;
            dataGridViewProducts.BorderStyle = BorderStyle.None;
            dataGridViewProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(127, 255, 0),
                Font = new Font("Times New Roman", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                SelectionBackColor = Color.FromArgb(127, 255, 0),
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
            panelBottom.BackColor = Color.FromArgb(127, 255, 0);
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
            BackColor = Color.White;
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
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Panel panelHeader;
        private Label labelTitle;
        private Label labelUserInfo;
        private Button buttonOrders;
        private Panel panelFilters;
        private TextBox textBoxSearch;
        private ComboBox comboBoxCategory;
        private ComboBox comboBoxManufacturer;
        private ComboBox comboBoxDiscount;
        private DataGridView dataGridViewProducts;
        private Panel panelBottom;
        private Label labelCount;
    }
}
