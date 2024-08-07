using System;
using System.Collections.Generic;

namespace MarkteApp.Backend.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public void Tost()
    {

    }
}
