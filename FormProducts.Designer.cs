namespace ObutvShop
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
            labelTitle = new Label();
            labelUserInfo = new Label();
            textBoxSearch = new TextBox();
            labelSearch = new Label();
            comboBoxFilter = new ComboBox();
            labelFilter = new Label();
            dataGridViewProducts = new DataGridView();
            panelBottom = new Panel();
            labelCount = new Label();

            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();

            // panelTop
            panelTop.BackColor = Color.FromArgb(127, 255, 0);
            panelTop.Controls.Add(labelTitle);
            panelTop.Controls.Add(labelUserInfo);
            panelTop.Controls.Add(labelSearch);
            panelTop.Controls.Add(textBoxSearch);
            panelTop.Controls.Add(labelFilter);
            panelTop.Controls.Add(comboBoxFilter);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1264, 100);

            // labelTitle
            labelTitle.Font = new Font("Times New Roman", 18F, FontStyle.Bold);
            labelTitle.ForeColor = Color.Black;
            labelTitle.Location = new Point(20, 10);
            labelTitle.Size = new Size(300, 35);
            labelTitle.Text = "ООО «Обувь»";

            // labelUserInfo
            labelUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelUserInfo.Font = new Font("Times New Roman", 11F);
            labelUserInfo.ForeColor = Color.Black;
            labelUserInfo.Location = new Point(900, 10);
            labelUserInfo.Size = new Size(350, 30);
            labelUserInfo.Text = "Гость";
            labelUserInfo.TextAlign = ContentAlignment.MiddleRight;

            // labelSearch
            labelSearch.Font = new Font("Times New Roman", 11F);
            labelSearch.Location = new Point(20, 58);
            labelSearch.Size = new Size(60, 25);
            labelSearch.Text = "Поиск:";

            // textBoxSearch
            textBoxSearch.Font = new Font("Times New Roman", 11F);
            textBoxSearch.Location = new Point(85, 55);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(350, 28);
            textBoxSearch.PlaceholderText = "Введите название или артикул...";
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;

            // labelFilter
            labelFilter.Font = new Font("Times New Roman", 11F);
            labelFilter.Location = new Point(470, 58);
            labelFilter.Size = new Size(110, 25);
            labelFilter.Text = "Скидка:";

            // comboBoxFilter
            comboBoxFilter.Font = new Font("Times New Roman", 11F);
            comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFilter.Location = new Point(580, 55);
            comboBoxFilter.Name = "comboBoxFilter";
            comboBoxFilter.Size = new Size(200, 28);
            comboBoxFilter.SelectedIndexChanged += ComboBoxFilter_SelectedIndexChanged;

            // dataGridViewProducts
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.BackgroundColor = Color.White;
            dataGridViewProducts.BorderStyle = BorderStyle.None;
            dataGridViewProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 250, 154);
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 11F, FontStyle.Bold);
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 250, 154);
            dataGridViewProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProducts.DefaultCellStyle.Font = new Font("Times New Roman", 10F);
            dataGridViewProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 255, 200);
            dataGridViewProducts.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridViewProducts.Dock = DockStyle.Fill;
            dataGridViewProducts.EnableHeadersVisualStyles = false;
            dataGridViewProducts.GridColor = Color.FromArgb(220, 220, 220);
            dataGridViewProducts.Location = new Point(0, 100);
            dataGridViewProducts.Name = "dataGridViewProducts";
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.RowHeadersVisible = false;
            dataGridViewProducts.RowTemplate.Height = 30;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // panelBottom
            panelBottom.BackColor = Color.FromArgb(127, 255, 0);
            panelBottom.Controls.Add(labelCount);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 641);
            panelBottom.Size = new Size(1264, 40);

            // labelCount
            labelCount.Dock = DockStyle.Fill;
            labelCount.Font = new Font("Times New Roman", 11F);
            labelCount.ForeColor = Color.Black;
            labelCount.Padding = new Padding(20, 0, 20, 0);
            labelCount.Text = "Записей: 0 из 0";

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
            Text = "Товары — ООО «Обувь»";
            WindowState = FormWindowState.Maximized;

            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label labelTitle;
        private Label labelUserInfo;
        private TextBox textBoxSearch;
        private Label labelSearch;
        private ComboBox comboBoxFilter;
        private Label labelFilter;
        private DataGridView dataGridViewProducts;
        private Panel panelBottom;
        private Label labelCount;
    }
}
