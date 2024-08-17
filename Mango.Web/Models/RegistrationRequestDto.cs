using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models;

public class RegistrationRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Phonenumber { get; set; }
    [Required]
    public string Password { get; set; }
    public string? Role { get; set; }
}