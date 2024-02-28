using System;
using System.Collections.Generic;

namespace CartManagmentSystem.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public DateTime CreationDate { get; set; }

    public string CreationBy { get; set; } = null!;

    public DateTime? LastUpdatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
