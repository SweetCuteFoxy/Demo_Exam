using Microsoft.EntityFrameworkCore;
using SportShopV22.Models;

namespace SportShopV22;

public partial class FormProducts : Form
{
    private readonly User? _currentUser;
    private List<Product> _allProducts = new();
    private int _totalCount;
    private bool _isLoaded;
    private readonly string _imgDir;
    private FormProductEdit? _editForm;

    public FormProducts(User? currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();

        _imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        if (_currentUser != null)
            labelUserInfo.Text = $"{_currentUser.FullName} ({_currentUser.Role.Name})";
        else
            labelUserInfo.Text = "Гость";

        // Фильтры и сортировка видны только менеджеру и администратору
        bool isManagerOrAdmin = _currentUser?.Role.Name == "Менеджер"
                             || _currentUser?.Role.Name == "Администратор";
        bool isAdmin = _currentUser?.Role.Name == "Администратор";

        panelFilters.Visible = isManagerOrAdmin;
        if (!isManagerOrAdmin) panelTop.Size = new Size(panelTop.Width, 60);

        buttonOrders.Visible = isManagerOrAdmin;
        buttonAdd.Visible = isAdmin;
        buttonDelete.Visible = isAdmin;

        // Фильтр по поставщику
        comboBoxSupplier.Items.Add("Все поставщики");
        comboBoxSupplier.SelectedIndex = 0;

        // Сортировка по количеству на складе
        comboBoxSort.Items.AddRange(new object[]
        {
            "Без сортировки", "Остаток ↑", "Остаток ↓"
        });
        comboBoxSort.SelectedIndex = 0;

        dataGridViewProducts.CellFormatting += DataGridView_CellFormatting;
        dataGridViewProducts.CellPainting += DataGridView_CellPainting;

        // Двойной клик для редактирования (только админ)
        if (isAdmin)
            dataGridViewProducts.CellDoubleClick += DataGridView_CellDoubleClick;

        Load += (_, _) =>
        {
            LoadSuppliers();
            LoadProducts();
            _isLoaded = true;
        };
    }

    private void LoadSuppliers()
    {
        try
        {
            using var db = new SportShopContext();
            foreach (var s in db.Suppliers.OrderBy(s => s.Name))
                comboBoxSupplier.Items.Add(s.Name);
        }
        catch { }
    }

    public void LoadProducts()
    {
        try
        {
            using var db = new SportShopContext();
            _allProducts = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Manufacturer)
                .Include(p => p.OrderItems)
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

        // Поиск по всем текстовым полям
        string search = textBoxSearch.Text.Trim();
        if (search.Length > 0)
        {
            filtered = filtered.Where(p =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Article.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (p.Description ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Manufacturer.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Supplier.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Unit.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        // Фильтр по поставщику
        int si = comboBoxSupplier.SelectedIndex;
        if (si > 0)
        {
            string? sup = comboBoxSupplier.Items[si]?.ToString();
            if (sup != null) filtered = filtered.Where(p => p.Supplier.Name == sup);
        }

        // Сортировка по количеству на складе
        int sortIdx = comboBoxSort.SelectedIndex;
        filtered = sortIdx switch
        {
            1 => filtered.OrderBy(p => p.StockQty),
            2 => filtered.OrderByDescending(p => p.StockQty),
            _ => filtered
        };

        var list = filtered.ToList();

        dataGridViewProducts.DataSource = list.Select(p => new
        {
            Фото = p.Photo ?? "",
            Артикул = p.Article,
            Наименование = p.Name,
            Категория = p.Category.Name,
            Описание = p.Description ?? "",
            Производитель = p.Manufacturer.Name,
            Поставщик = p.Supplier.Name,
            Цена = p.Price,
            Ед = p.Unit,
            Скидка = p.DiscountPct,
            СоСкидкой = p.PriceDiscounted,
            Остаток = p.StockQty
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
            if (val is int d && d > 15)
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
            if (discVal is int disc && disc > 0)
            {
                e.CellStyle.Font = new Font("Times New Roman", 10F, FontStyle.Strikeout);
                e.CellStyle.ForeColor = Color.Red;
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

    private void ButtonLogout_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Abort;
        Close();
    }

    private void ButtonAdd_Click(object? sender, EventArgs e)
    {
        if (_editForm != null && !_editForm.IsDisposed)
        {
            _editForm.BringToFront();
            return;
        }

        _editForm = new FormProductEdit(null);
        if (_editForm.ShowDialog() == DialogResult.OK)
            LoadProducts();
    }

    private void ButtonDelete_Click(object? sender, EventArgs e)
    {
        if (dataGridViewProducts.CurrentRow == null) return;

        var article = dataGridViewProducts.CurrentRow.Cells["Артикул"].Value?.ToString();
        if (article == null) return;

        var product = _allProducts.FirstOrDefault(p => p.Article == article);
        if (product == null) return;

        if (product.OrderItems.Any())
        {
            MessageBox.Show("Невозможно удалить товар, так как он присутствует в заказах.",
                "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show($"Удалить товар \"{product.Name}\"?",
            "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result != DialogResult.Yes) return;

        try
        {
            using var db = new SportShopContext();
            var entity = db.Products.Find(product.Id);
            if (entity != null)
            {
                // Удалить фото
                if (!string.IsNullOrEmpty(entity.Photo) && entity.Photo != "picture.png")
                {
                    var path = Path.Combine(_imgDir, entity.Photo);
                    if (File.Exists(path))
                        try { File.Delete(path); } catch { }
                }

                db.Products.Remove(entity);
                db.SaveChanges();
                LoadProducts();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DataGridView_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var article = dataGridViewProducts.Rows[e.RowIndex].Cells["Артикул"].Value?.ToString();
        if (article == null) return;

        var product = _allProducts.FirstOrDefault(p => p.Article == article);
        if (product == null) return;

        if (_editForm != null && !_editForm.IsDisposed)
        {
            _editForm.BringToFront();
            return;
        }

        _editForm = new FormProductEdit(product.Id);
        if (_editForm.ShowDialog() == DialogResult.OK)
            LoadProducts();
    }
}
