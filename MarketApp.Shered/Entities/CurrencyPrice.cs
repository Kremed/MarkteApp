using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MarketApp.Shered;

[Table("CurrencyPrices", Schema = "dbo")]
public partial class CurrencyPrice
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CurrencyID")]
    public int CurrencyId { get; set; }

    public double Price { get; set; }

    public double Ratio { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecordTime { get; set; }
}
