using System.ComponentModel.DataAnnotations;

namespace UnicApi.Auth.Models;

public class RegisterModel
{
    [Required]
    [DataType(DataType.Text)]
    public string FirstName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}