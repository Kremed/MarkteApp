using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarkteApp.Backend.Models;

public partial class CurrencyPrice
{
    public int Id { get; set; }

    public int CurrencyId { get; set; }
    
    public double Price { get; set; }

    public double Ratio { get; set; }

    public DateTime RecordTime { get; set; }
}
