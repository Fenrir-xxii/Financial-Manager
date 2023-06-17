using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;
using WpfApp9_MyFinances.ViewModels;

namespace WpfApp9_MyFinances.Models;

public partial class Currency
{
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is Currency))
            return false;

        return Id.Equals((obj as Currency).Id);
    }
    public override int GetHashCode() =>
      new { Id, CodeNumber, CodeLetter, Title }.GetHashCode();

}
