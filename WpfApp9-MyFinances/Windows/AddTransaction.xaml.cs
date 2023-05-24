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
using WpfApp9_MyFinances.ViewModels;

namespace WpfApp9_MyFinances.Windows
{
    /// <summary>
    /// Interaction logic for AddTransaction.xaml
    /// </summary>
    public partial class AddTransaction : Window
    {
        public AddTransaction()
        {
            InitializeComponent();
            DataContext = new AddTransactionViewModel();
        }
        //private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = IsValid(sender as DependencyObject);
        //}

        //private bool IsValid(DependencyObject obj)
        //{
        //    return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        //}
        //void Save_Executed(object target, ExecutedRoutedEventArgs e)
        //{
        //    var a = (AddTransactionViewModel)target;
        //    _ = (DataContext as AddTransactionViewModel).SaveTransactionExp;
        //}

       
    }
}
