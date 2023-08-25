using System;
using System.Collections.Generic;

namespace WpfApp9_MyFinances.Models;

public partial class ProviderType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
