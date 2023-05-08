using System.ComponentModel.DataAnnotations;

namespace UnicApi.Auth.Models;

public class RegisterLecturerModel : RegisterModel
{
    [Required]
    [DataType(DataType.Text)]
    public string SocialInsuranceNumber { get; set; }
}