using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.ModelsForWpfOnly;

namespace WpfApp9_MyFinances.ViewModels;

public class LoanViewModel : NotifyPropertyChangedBase
{
    public LoanViewModel() { }
    public LoanViewModel(Loan loan)
    {
        Model = loan;
        _allLoansPaybacks = loan.Paybacks;
    }
    public Loan Model { get; set; }
    public decimal LoanAmount
    {
        get => Model.LoanAmount;
        set
        {
            Model.LoanAmount = value;
            OnPropertyChanged(nameof(LoanAmount));
        }
    }
    public string? LoanDescription
    {
        get => Model.LoanDescription;
        set
        {
            Model.LoanDescription = value;
            OnPropertyChanged(nameof(LoanDescription));
        }
    }
    public DateTime DateOfLoan
    {
        get => Model.DateOfLoan;
        set
        {
            Model.DateOfLoan = value;
            OnPropertyChanged(nameof(DateOfLoan));
        }
    }
    public bool IsLoanClosed
    {
        get => Model.IsLoanClosed;
        set
        {
            Model.IsLoanClosed = value;
            OnPropertyChanged(nameof(IsLoanClosed));
        }
    }
    public PaymentMethodViewModel LoanPaymentMethod
    {
        get => new PaymentMethodViewModel { Model = Model.LoanPaymentMethod };
        set
        {
            Model.LoanPaymentMethod = value.Model;
            //Model.PaymentMethodId = value.Model.Id;
            OnPropertyChanged(nameof(LoanPaymentMethod));
            //OnPropertyChanged(nameof(PaymentMethodId));
        }
    }
    public ProviderViewModel LoanProvider
    {
        get => new ProviderViewModel { Model = Model.LoanProvider };
        set
        {
            Model.LoanProvider = value.Model;
            //Model.ProviderId = value.Model.Id;
            OnPropertyChanged(nameof(LoanProvider));
            //OnPropertyChanged(nameof(ProviderId));
        }
    }
    public string LoanGiver
    {
        get => Model.LoanGiver;
        set
        {
            Model.LoanGiver = value;
            OnPropertyChanged(nameof(LoanGiver));
        }
    }
    public string LoanReceiver
    {
        get => Model.LoanReceiver;
        set
        {
            Model.LoanReceiver = value;
            OnPropertyChanged(nameof(LoanReceiver));
        }
    }
    private List<LoanPayback> _allLoansPaybacks {  get; set; }
    public ObservableCollection<LoanPayback> LoanPaybacks
    {
        get
        {
            var collection = new ObservableCollection<LoanPayback>();
            _allLoansPaybacks.ForEach(loan =>
            {
                collection.Add(loan);
            });

            return collection;
        }
        set
        {
            LoanPaybacks = value;
            OnPropertyChanged(nameof(LoanPaybacks));
            OnPropertyChanged(nameof(TotalAmountPayback));
            OnPropertyChanged(nameof(LoanBalance));
        }
    }
    public decimal TotalAmountPayback
    {
        get
        {
            return _allLoansPaybacks.Sum(loan => loan.Amount);
        }
    }
    public decimal LoanBalance
    {
        get => LoanAmount - TotalAmountPayback;
    }
}
