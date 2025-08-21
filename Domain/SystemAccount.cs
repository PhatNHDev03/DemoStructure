using System;
using System.Collections.Generic;

namespace Domain;

public partial class SystemAccount
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Inventoryproduct> Inventoryproducts { get; set; } = new List<Inventoryproduct>();

    public virtual ICollection<TransactionTable> TransactionTables { get; set; } = new List<TransactionTable>();
}
