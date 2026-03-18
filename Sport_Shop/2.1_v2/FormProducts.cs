using Microsoft.EntityFrameworkCore;
using SportShopV2.Models;

namespace SportShopV2;

public partial class FormProducts : Form
{
    private readonly User? _currentUser;
    private List<Product> _allProducts = new();
    private int _totalCount;
    private bool _isLoaded;
    private readonly string _imgDir;

    public FormProducts(User? currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();

        _imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        if (_currentUser != null)
            labelUserInfo.Text = $"{_currentUser.FullName} ({_currentUser.Role.Name})";
        else
            labelUserInfo.Text = "Гость";

        comboBoxDiscount.Items.AddRange(new object[]
        {
            "Все скидки", "0 - 5%", "5 - 15%", "15 - 30%", "30 - 70%", "70 - 100%"
        });
        comboBoxDiscount.SelectedIndex = 0;

        dataGridViewProducts.CellFormatting += DataGridView_CellFormatting;
        dataGridViewProducts.CellPainting += DataGridView_CellPainting;
        Load += (_, _) =>
        {
            LoadCategories();
            LoadManufacturers();
            LoadProducts();
            _isLoaded = true;
        };
    }

    private void LoadCategories()
    {
        try
        {
            using var db = new SportShopContext();
            comboBoxCategory.Items.Add("Все категории");
            foreach (var c in db.Categories.OrderBy(c => c.Name))
                comboBoxCategory.Items.Add(c.Name);
            comboBoxCategory.SelectedIndex = 0;
        }
        catch { }
    }

    private void LoadManufacturers()
    {
        try
        {
            using var db = new SportShopContext();
            comboBoxManufacturer.Items.Add("Все производители");
            foreach (var m in db.Manufacturers.OrderBy(m => m.Name))
                comboBoxManufacturer.Items.Add(m.Name);
            comboBoxManufacturer.SelectedIndex = 0;
        }
        catch { }
    }

    private void LoadProducts()
    {
        try
        {
            using var db = new SportShopContext();
            _allProducts = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Manufacturer)
                .Include(p => p.Measure)
                .OrderBy(p => p.Name)
                .ToList();
            _totalCount = _allProducts.Count;
            ApplyFilters();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allProducts.AsEnumerable();

        string search = textBoxSearch.Text.Trim();
        if (search.Length > 0)
        {
            filtered = filtered.Where(p =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Article.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Manufacturer.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        int di = comboBoxDiscount.SelectedIndex;
        filtered = di switch
        {
            1 => filtered.Where(p => p.Discount >= 0 && p.Discount < 5),
            2 => filtered.Where(p => p.Discount >= 5 && p.Discount < 15),
            3 => filtered.Where(p => p.Discount >= 15 && p.Discount < 30),
            4 => filtered.Where(p => p.Discount >= 30 && p.Discount < 70),
            5 => filtered.Where(p => p.Discount >= 70 && p.Discount <= 100),
            _ => filtered
        };

        int ci = comboBoxCategory.SelectedIndex;
        if (ci > 0)
        {
            string? cat = comboBoxCategory.Items[ci]?.ToString();
            if (cat != null) filtered = filtered.Where(p => p.Category.Name == cat);
        }

        int mi = comboBoxManufacturer.SelectedIndex;
        if (mi > 0)
        {
            string? mfr = comboBoxManufacturer.Items[mi]?.ToString();
            if (mfr != null) filtered = filtered.Where(p => p.Manufacturer.Name == mfr);
        }

        var list = filtered.ToList();

        dataGridViewProducts.DataSource = list.Select(p => new
        {
            Фото = p.Image ?? "",
            Артикул = p.Article,
            Наименование = p.Name,
            Категория = p.Category.Name,
            Описание = p.Description ?? "",
            Производитель = p.Manufacturer.Name,
            Поставщик = p.Supplier.Name,
            Цена = p.Price,
            Ед = p.Measure.Name,
            Скидка = p.Discount,
            СоСкидкой = p.PriceDiscounted,
            Остаток = p.Stock
        }).ToList();

        if (dataGridViewProducts.Columns.Contains("Фото"))
        {
            dataGridViewProducts.Columns["Фото"]!.Width = 60;
            dataGridViewProducts.Columns["Фото"]!.HeaderText = "";
        }

        labelCount.Text = $"Показано {list.Count} из {_totalCount} товаров";
    }

    private void DataGridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dataGridViewProducts.Rows[e.RowIndex];

        // Скидка > 15% — фон #2EC4B6 (бирюзовый)
        if (dataGridViewProducts.Columns.Contains("Скидка"))
        {
            var val = row.Cells["Скидка"].Value;
            if (val is decimal d && d > 15)
            {
                e.CellStyle.BackColor = Color.FromArgb(46, 196, 182);
                e.CellStyle.SelectionBackColor = Color.FromArgb(38, 170, 158);
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }

        // Остаток == 0 — фон #E9F5FF
        if (dataGridViewProducts.Columns.Contains("Остаток"))
        {
            var stockVal = row.Cells["Остаток"].Value;
            if (stockVal is int stock && stock == 0)
            {
                e.CellStyle.BackColor = Color.FromArgb(233, 245, 255);
                e.CellStyle.SelectionBackColor = Color.FromArgb(200, 225, 245);
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }

        // Цена перечёркнута если есть скидка
        if (dataGridViewProducts.Columns[e.ColumnIndex].Name == "Цена")
        {
            var discVal = row.Cells["Скидка"].Value;
            if (discVal is decimal disc && disc > 0)
            {
                e.CellStyle.Font = new Font("Times New Roman", 10F, FontStyle.Strikeout);
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        if (dataGridViewProducts.Columns[e.ColumnIndex].Name == "Фото")
        {
            e.FormattingApplied = true;
            e.Value = "";
        }
    }

    private void DataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (!dataGridViewProducts.Columns.Contains("Фото")) return;
        if (e.ColumnIndex != dataGridViewProducts.Columns["Фото"]!.Index) return;

        e.Handled = true;
        e.PaintBackground(e.ClipBounds, true);

        var photoFile = dataGridViewProducts.Rows[e.RowIndex].Cells["Фото"].Value?.ToString();
        string path = "";
        if (!string.IsNullOrEmpty(photoFile))
            path = Path.Combine(_imgDir, photoFile);

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            path = Path.Combine(_imgDir, "picture.png");

        if (File.Exists(path))
        {
            try
            {
                using var img = Image.FromFile(path);
                int pad = 4;
                var dest = new Rectangle(
                    e.CellBounds.X + pad,
                    e.CellBounds.Y + pad,
                    e.CellBounds.Width - pad * 2,
                    e.CellBounds.Height - pad * 2);
                e.Graphics!.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(img, dest);
            }
            catch { }
        }
    }

    private void TextBoxSearch_TextChanged(object? sender, EventArgs e)
    {
        if (_isLoaded) ApplyFilters();
    }

    private void ComboBoxFilter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isLoaded) ApplyFilters();
    }

    private void ButtonOrders_Click(object? sender, EventArgs e)
    {
        using var form = new FormOrders(_currentUser);
        form.ShowDialog();
    }
}
