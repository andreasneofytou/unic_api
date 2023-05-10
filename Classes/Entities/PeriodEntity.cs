using System.ComponentModel.DataAnnotations;

namespace UnicApi.Classes.Entities;

public class PeriodEntity
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    public int Year { get; set; }
}