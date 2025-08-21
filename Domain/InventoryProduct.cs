using System;
using System.Collections.Generic;

namespace Domain;

public partial class Inventoryproduct
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int? SystemAccountId { get; set; }

    public string? WarehouseLocation { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? Notes { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual SystemAccount? SystemAccount { get; set; }

    public virtual ICollection<TransactionTable> TransactionTables { get; set; } = new List<TransactionTable>();
}
