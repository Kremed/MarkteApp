using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


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
    public DateTime FromDate { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime")]
    public DateTime ToDate { get; set; } = DateTime.Now.AddDays(30);

    public bool IsActive { get; set; }

    public string? Url { get; set; }

    [JsonIgnore]
    [NotMapped]
    public string? StringActiveSatus { get; set; }

    [JsonIgnore]
    [NotMapped]
    public string? colorCode { get; set; }
}
