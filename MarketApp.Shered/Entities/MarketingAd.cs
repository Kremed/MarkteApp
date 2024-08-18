using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MarketApp.Shered;

[Table("MarketingAds", Schema = "dbo")]
public partial class MarketingAd
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string CoverImage { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FromDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ToDate { get; set; }

    public bool IsActive { get; set; }

    public string? Url { get; set; }
}
