using System;
using System.Collections.Generic;

namespace CartManagmentSystem.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string CreationBy { get; set; } = null!;

    public DateTime? LastUpdatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
