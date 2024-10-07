using Brewery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Brewer
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Beer> Beers { get; set; }
    }
}
