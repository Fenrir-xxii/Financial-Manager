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
    /// Interaction logic for EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        public EditTransaction(Expense expense)
        {
            InitializeComponent();
            DataContext = new EditTransactionViewModel(expense);
            ExpensesTab.IsSelected = true;
            IncomesTab.IsEnabled = false;
            TransfersTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Income income)
        {
            InitializeComponent();
            DataContext = new EditTransactionViewModel(income);
            IncomesTab.IsSelected = true;
            ExpensesTab.IsEnabled = false;
            TransfersTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Transfer transfer)
        {
            InitializeComponent();
            DataContext = new EditTransactionViewModel(transfer);
            TransfersTab.IsSelected = true;
            ExpensesTab.IsEnabled = false;
            IncomesTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Exchange exchange)
        {
            InitializeComponent();
            DataContext = new EditTransactionViewModel(exchange);
            ExchangeTab.IsSelected = true;
            TransfersTab.IsEnabled = false;
            ExpensesTab.IsEnabled = false;
            IncomesTab.IsEnabled = false;
        }
    }
}
