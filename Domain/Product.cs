using System;
using System.Collections.Generic;

namespace Domain;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Unit { get; set; }

    public string? Category { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Inventoryproduct> Inventoryproducts { get; set; } = new List<Inventoryproduct>();
}
