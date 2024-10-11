using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Quote
    {
        public decimal Price { get; private set; }
        public Wholesaler Saler { get; private set; }
        public string Summary { get; private set; }
        public List<QuoteItem> OrderItems { get; set; }

    }
}
