using System.ComponentModel.DataAnnotations;

namespace StoRvStar.Models.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Введіть логін")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Введіть пароль")]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
}
