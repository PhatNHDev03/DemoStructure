using System;
using System.Collections.Generic;

namespace Domain;

public partial class TransactionTable
{
    public int TransactionId { get; set; }

    public int InventoryId { get; set; }

    public int Quantity { get; set; }

    public int? SystemAccountId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Inventoryproduct Inventory { get; set; } = null!;

    public virtual SystemAccount? SystemAccount { get; set; }
}
