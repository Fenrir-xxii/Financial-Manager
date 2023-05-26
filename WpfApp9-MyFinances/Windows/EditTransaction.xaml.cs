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
        public EditTransaction(Expense expense, Database3MyFinancesContext db)
        {
            InitializeComponent();
            //DataContext= new ExpenseViewModel(expense);
            DataContext = new EditTransactionViewModel(expense, db);
            ExpensesTab.IsSelected = true;
            IncomesTab.IsEnabled = false;
            TransfersTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Income income, Database3MyFinancesContext db)
        {
            InitializeComponent();
            //DataContext= new IncomeViewModel(income);
            DataContext = new EditTransactionViewModel(income, db);
            IncomesTab.IsSelected = true;
            ExpensesTab.IsEnabled = false;
            TransfersTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Transfer transfer, Database3MyFinancesContext db)
        {
            InitializeComponent();
            //DataContext= new TransferViewModel(transfer); 
            DataContext = new EditTransactionViewModel(transfer, db);
            TransfersTab.IsSelected = true;
            ExpensesTab.IsEnabled = false;
            IncomesTab.IsEnabled = false;
            ExchangeTab.IsEnabled = false;
        }
        public EditTransaction(Exchange exchange, Database3MyFinancesContext db)
        {
            InitializeComponent();
            //DataContext= new TransferViewModel(transfer); 
            DataContext = new EditTransactionViewModel(exchange, db);
            ExchangeTab.IsSelected = true;
            TransfersTab.IsEnabled = false;
            ExpensesTab.IsEnabled = false;
            IncomesTab.IsEnabled = false;
        }
    }
}
