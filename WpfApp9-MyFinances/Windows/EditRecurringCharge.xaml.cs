using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ViewModels;

namespace WpfApp9_MyFinances.Windows
{
    /// <summary>
    /// Interaction logic for EditRecuringCharge.xaml
    /// </summary>
    public partial class EditRecurringCharge : Window
    {
        public EditRecurringCharge(RecurringCharge rc, Database3MyFinancesContext db)
        {
            InitializeComponent();
            DataContext = new EditRecurringChargeViewModel(rc, db);
        }
    }
}
