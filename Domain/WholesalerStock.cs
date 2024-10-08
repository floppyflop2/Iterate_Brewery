using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class WholesalerStock : BaseEntity
{
    public int WholesalerId { get; set; }
    public required Wholesaler Wholesaler { get; set; }
    public int BeerId { get; set; }
    public required Beer Beer { get; set; }
    public int Quantity { get; set; }
}
