using System;
using System.Collections.Generic;

namespace TEST_PFE.Models;

public partial class ConfigurationTable
{
    public Guid ConfigId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Product? Product { get; set; }
}
