using System;
using System.Collections.Generic;

namespace CartManagmentSystem.Entities;

public partial class UserLog
{
    public int Id { get; set; }

    public string? UserAgent { get; set; }

    public string? UserFunction { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TransStatus { get; set; }

    public int? SourceId { get; set; }

    public string? SourceName { get; set; }

    public string? ResponseBody { get; set; }

    public string? RequestBody { get; set; }
}
