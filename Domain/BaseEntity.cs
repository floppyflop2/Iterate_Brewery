using System.ComponentModel.DataAnnotations;

namespace Domain;

public class BaseEntity
{
    [Key] public int Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; }
}