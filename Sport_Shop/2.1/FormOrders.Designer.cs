namespace SportShopV1
{
    partial class FormOrders
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
            panelFilters = new Panel();
            textBoxSearch = new TextBox();
            comboBoxStatus = new ComboBox();
            splitContainer = new SplitContainer();
            dataGridViewOrders = new DataGridView();
            panelItemsHeader = new Panel();
            labelItems = new Label();
            dataGridViewItems = new DataGridView();
            panelBottom = new Panel();
            labelCount = new Label();

            panelTop.SuspendLayout();
            panelHeader.SuspendLayout();
            panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItems).BeginInit();
            panelItemsHeader.SuspendLayout();
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
            panelHeader.Controls.Add(labelUserInfo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Size = new Size(1216, 55);

            // labelTitle
            labelTitle.Dock = DockStyle.Left;
            labelTitle.Font = new Font("Times New Roman", 18F, FontStyle.Bold);
            labelTitle.ForeColor = Color.FromArgb(0, 250, 154);
            labelTitle.Size = new Size(200, 55);
            labelTitle.Text = "Заказы";
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;

            // labelUserInfo
            labelUserInfo.Dock = DockStyle.Right;
            labelUserInfo.Font = new Font("Times New Roman", 10F);
            labelUserInfo.ForeColor = Color.Gray;
            labelUserInfo.Size = new Size(300, 55);
            labelUserInfo.TextAlign = ContentAlignment.MiddleRight;

            // panelFilters
            panelFilters.Dock = DockStyle.Bottom;
            panelFilters.Controls.Add(comboBoxStatus);
            panelFilters.Controls.Add(textBoxSearch);
            panelFilters.Size = new Size(1216, 45);

            // textBoxSearch
            textBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            textBoxSearch.Font = new Font("Times New Roman", 11F);
            textBoxSearch.Location = new Point(0, 8);
            textBoxSearch.Size = new Size(300, 28);
            textBoxSearch.PlaceholderText = "Поиск по номеру или клиенту...";
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;

            // comboBoxStatus
            comboBoxStatus.Font = new Font("Times New Roman", 10F);
            comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStatus.Location = new Point(315, 8);
            comboBoxStatus.Size = new Size(200, 28);
            comboBoxStatus.SelectedIndexChanged += ComboBoxStatus_SelectedIndexChanged;

            // splitContainer
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.SplitterDistance = 350;
            splitContainer.SplitterWidth = 6;
            splitContainer.BackColor = Color.FromArgb(230, 230, 230);

            splitContainer.Panel1.Controls.Add(dataGridViewOrders);
            splitContainer.Panel2.Controls.Add(dataGridViewItems);
            splitContainer.Panel2.Controls.Add(panelItemsHeader);

            // dataGridViewOrders
            dataGridViewOrders.AllowUserToAddRows = false;
            dataGridViewOrders.AllowUserToDeleteRows = false;
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewOrders.BackgroundColor = Color.White;
            dataGridViewOrders.BorderStyle = BorderStyle.None;
            dataGridViewOrders.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewOrders.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewOrders.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(127, 255, 0),
                Font = new Font("Times New Roman", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                SelectionBackColor = Color.FromArgb(127, 255, 0),
                SelectionForeColor = Color.FromArgb(60, 60, 60),
                Padding = new Padding(8, 4, 8, 4)
            };
            dataGridViewOrders.ColumnHeadersHeight = 40;
            dataGridViewOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewOrders.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Times New Roman", 10F),
                ForeColor = Color.FromArgb(50, 50, 50),
                SelectionBackColor = Color.FromArgb(210, 230, 255),
                SelectionForeColor = Color.Black,
                Padding = new Padding(8, 2, 8, 2)
            };
            dataGridViewOrders.Dock = DockStyle.Fill;
            dataGridViewOrders.EnableHeadersVisualStyles = false;
            dataGridViewOrders.GridColor = Color.FromArgb(230, 230, 230);
            dataGridViewOrders.ReadOnly = true;
            dataGridViewOrders.RowHeadersVisible = false;
            dataGridViewOrders.RowTemplate.Height = 35;
            dataGridViewOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOrders.SelectionChanged += DataGridViewOrders_SelectionChanged;

            // panelItemsHeader
            panelItemsHeader.BackColor = Color.FromArgb(127, 255, 0);
            panelItemsHeader.Dock = DockStyle.Top;
            panelItemsHeader.Size = new Size(1264, 36);
            panelItemsHeader.Controls.Add(labelItems);

            // labelItems
            labelItems.Dock = DockStyle.Fill;
            labelItems.Font = new Font("Times New Roman", 11F, FontStyle.Bold);
            labelItems.ForeColor = Color.FromArgb(0, 250, 154);
            labelItems.Text = "  Состав заказа";
            labelItems.TextAlign = ContentAlignment.MiddleLeft;

            // dataGridViewItems
            dataGridViewItems.AllowUserToAddRows = false;
            dataGridViewItems.AllowUserToDeleteRows = false;
            dataGridViewItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewItems.BackgroundColor = Color.White;
            dataGridViewItems.BorderStyle = BorderStyle.None;
            dataGridViewItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewItems.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(127, 255, 0),
                Font = new Font("Times New Roman", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                SelectionBackColor = Color.FromArgb(127, 255, 0),
                SelectionForeColor = Color.FromArgb(60, 60, 60)
            };
            dataGridViewItems.ColumnHeadersHeight = 36;
            dataGridViewItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewItems.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Times New Roman", 10F),
                ForeColor = Color.FromArgb(50, 50, 50),
                SelectionBackColor = Color.FromArgb(220, 235, 255),
                SelectionForeColor = Color.Black
            };
            dataGridViewItems.Dock = DockStyle.Fill;
            dataGridViewItems.EnableHeadersVisualStyles = false;
            dataGridViewItems.GridColor = Color.FromArgb(230, 230, 230);
            dataGridViewItems.ReadOnly = true;
            dataGridViewItems.RowHeadersVisible = false;
            dataGridViewItems.RowTemplate.Height = 30;
            dataGridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
            labelCount.Text = "Показано 0 из 0 заказов";
            labelCount.TextAlign = ContentAlignment.MiddleLeft;

            // FormOrders
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1264, 681);
            Controls.Add(splitContainer);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "FormOrders";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Заказы — СпортЭкип";
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
            ((System.ComponentModel.ISupportInitialize)dataGridViewOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItems).EndInit();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelItemsHeader.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Panel panelHeader;
        private Label labelTitle;
        private Label labelUserInfo;
        private Panel panelFilters;
        private TextBox textBoxSearch;
        private ComboBox comboBoxStatus;
        private SplitContainer splitContainer;
        private DataGridView dataGridViewOrders;
        private DataGridView dataGridViewItems;
        private Panel panelItemsHeader;
        private Label labelItems;
        private Panel panelBottom;
        private Label labelCount;
    }
}
