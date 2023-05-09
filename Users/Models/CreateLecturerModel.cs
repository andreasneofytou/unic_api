using System.ComponentModel.DataAnnotations;

namespace UnicApi.Users.Models;

public class CreateLecturerModel : CreateUserModel
{
    [Required]
    [DataType(DataType.Text)]
    public string SocialInsuranceNumber { get; set; }
}