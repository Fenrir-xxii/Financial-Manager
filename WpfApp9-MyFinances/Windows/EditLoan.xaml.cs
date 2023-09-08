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
using WpfApp9_MyFinances.ModelsForWpfOnly;
using WpfApp9_MyFinances.ViewModels;

namespace WpfApp9_MyFinances.Windows
{
    /// <summary>
    /// Interaction logic for EditLoan.xaml
    /// </summary>
    public partial class EditLoan : Window
    {
        public EditLoan(Loan loan)
        {
            InitializeComponent();
            DataContext = new EditLoanViewModel(loan);
            if (loan.LoanGiver.Equals("Me"))
            {
                GivingTab.IsSelected = true;
                ReceivingTab.IsEnabled = false;
            }
            else
            {
                GivingTab.IsEnabled = false;
                ReceivingTab.IsSelected = true;
            }
        }
        public EditLoan(LoanPayback payback, Loan loan)
        {
            InitializeComponent();
            DataContext = new EditLoanViewModel(payback, loan);
            if (loan.LoanGiver.Equals("Me"))
            {
                ReceivingTab.IsSelected = true;
                GivingTab.IsEnabled = false;
            }
            else
            {
                ReceivingTab.IsEnabled = false;
                GivingTab.IsSelected = true;
            }
        }
    }
}
