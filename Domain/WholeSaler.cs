using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class Wholesaler : BaseEntity
{
    public required string Name { get; set; }
    public required ICollection<WholesalerStock> Stocks { get; set; } = new List<WholesalerStock>();
}