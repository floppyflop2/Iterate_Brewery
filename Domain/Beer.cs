using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Domain;

public class Beer : BaseEntity
{
    public required string Name { get; set; }
    public double AlcoholContent { get; set; }
    public int BreweryId { get; set; }
    public required Brewery Brewery { get; set; }
}