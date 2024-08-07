using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarkteApp.Backend.Models;

public partial class MarketingAd
{
    public int Id { get; set; }

    public string CoverImage { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool IsActive { get; set; }

    public string? Url { get; set; }
}
