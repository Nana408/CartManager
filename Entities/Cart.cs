using System;
using System.Collections.Generic;

namespace CartManagmentSystem.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public string CreationBy { get; set; } = null!;

    public DateTime? LastUpdatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
