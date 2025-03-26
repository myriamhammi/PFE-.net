using System;
using System.Collections.Generic;

namespace ConfigAPI.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public Guid? OriginatingLead { get; set; }

    public Guid? PreferredEquipement { get; set; }

    public string? PreferredService { get; set; }

    public string? Territory { get; set; }

    public int? Opendeals { get; set; }

    public decimal? OpenRevenue { get; set; }

    public decimal? OpenRevenueBase { get; set; }

    public virtual ICollection<ConfigurationTable> ConfigurationTables { get; set; } = new List<ConfigurationTable>();
}
