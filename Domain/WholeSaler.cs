using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class WholeSaler
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required List<BeerStock> BeerStocks { get; set; }
    }
}
