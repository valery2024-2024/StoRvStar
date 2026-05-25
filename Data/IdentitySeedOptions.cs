namespace StoRvStar.Data;

public class IdentitySeedOptions
{
    public const string SectionName = "IdentitySeed";

    public bool Enabled { get; set; }

    public string AdminUsername { get; set; } = "admin";
    public string AdminEmail { get; set; } = "admin@storvstar.local";
    public string AdminRole { get; set; } = "Admin";

    public string? AdminPassword { get; set; }
}
