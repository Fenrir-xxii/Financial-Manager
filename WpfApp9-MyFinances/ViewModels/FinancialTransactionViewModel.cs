using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ModelsForWpfOnly;
using WpfApp9_MyFinances.Windows;

namespace WpfApp9_MyFinances.ViewModels;

public class FinancialTransactionViewModel : NotifyPropertyChangedBase
{
    public FinancialTransactionViewModel() { }
    public FinancialTransactionViewModel(FinancialTransaction financialTransaction)
    {
        Model = financialTransaction;
    }
    public FinancialTransaction Model { get; set; }
    public string Title
    {
        get => Model.Title;
        set
        {
            Model.Title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    public decimal Amount
    {
        get => Model.Amount;
        set
        {
            Model.Amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }
    public DateTime DateOfTransaction
    {
        get => Model.DateOfTransaction;
        set
        {
            Model.DateOfTransaction = value;
            OnPropertyChanged(nameof(DateOfTransaction));
        }
    }
    public decimal BalanceBefore
    {
        get => Model.BalanceBefore;
        set
        {
            Model.BalanceBefore = value;
            OnPropertyChanged(nameof(BalanceBefore));
        }
    }
    public decimal BalanceAfter
    {
        get => Model.BalanceAfter;
        set
        {
            Model.BalanceAfter = value;
            OnPropertyChanged(nameof(BalanceAfter));
        }
    }
    public int TransactionId
    {
        get => Model.TransactionId;
    }
    public TransactionType TransactionType
    {
        get => Model.TransactionType;
        set
        {
            Model.TransactionType = value;
            OnPropertyChanged(nameof(TransactionType));
        }
    }
    public Brush TransactionColor
    {
        get
        {
            if (Amount > 0)
            {
                return Brushes.Green;
            }
            return Brushes.Red;
        }
    }
}
