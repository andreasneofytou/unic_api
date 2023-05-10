using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnicApi.Users.Entities;

namespace UnicApi.Classes.Entities;

public class SubjectEntity
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    public List<UserEntity> Lecturers { get; set; }

    public List<UserEntity> Students { get; set; }

    [ForeignKey("Period")]
    public string PeriodId { get; set; }
    public PeriodEntity Period { get; set; }
}