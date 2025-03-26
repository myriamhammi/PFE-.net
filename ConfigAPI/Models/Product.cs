using System;
using System.Collections.Generic;

namespace ConfigAPI.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public decimal? CurrentCost { get; set; }

    public string? DefaultUoMidName { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? ProductNumber { get; set; }

    public string? ProductStructureName { get; set; }

    public int? ProductTypeCode { get; set; }

    public decimal? QuantityOnHand { get; set; }

    public decimal? StantardCost { get; set; }

    public string? StateCodeName { get; set; }

    public string? StatusCodeName { get; set; }

    public decimal? StockVolume { get; set; }

    public decimal? StockWeight { get; set; }

    public DateOnly? ValidFromDate { get; set; }

    public virtual ICollection<ConfigurationTable> ConfigurationTables { get; set; } = new List<ConfigurationTable>();
}
