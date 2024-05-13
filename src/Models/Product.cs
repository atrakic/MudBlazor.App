using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
