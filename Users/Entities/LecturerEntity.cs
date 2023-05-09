using System.ComponentModel.DataAnnotations.Schema;

namespace UnicApi.Users.Entities;

public class LecturerEntity
{
    [ForeignKey("User")]
    public string Id { get; set; }
    public virtual UserEntity User { get; set; }

    public string SocialInsuranceNumber { get; set; }
}