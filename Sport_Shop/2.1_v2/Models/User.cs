namespace SportShopV2.Models;

public class User
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string FullName { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
