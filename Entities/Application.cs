using System;
using System.Collections.Generic;

namespace CartManagmentSystem.Entities;

public partial class Application
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? SourceKey { get; set; }

    public string? SourceToken { get; set; }

    public bool? Status { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? DateModified { get; set; }

    public string? ModifiedBy { get; set; }
}
