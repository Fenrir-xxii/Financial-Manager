using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class GivingLoanViewModel : NotifyPropertyChangedBase
{
    public GivingLoanViewModel()
    {
        Model = new GivingLoan();
    }
    public GivingLoanViewModel(GivingLoan loan)
    {
        Model = loan;
    }
    public GivingLoan Model { get; set; }
    public int Id { get => Model.Id; }
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
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
    public int PaymentMethodId
    {
        get => Model.PaymentMethodId;
        set
        {
            Model.PaymentMethodId = value;
            OnPropertyChanged(nameof(PaymentMethodId));
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
    public int ProviderId
    {
        get => Model.ProviderId;
        set
        {
            Model.ProviderId = value;
            OnPropertyChanged(nameof(ProviderId));
        }
    }
    public int? ReceivingLoanId
    {
        get => Model.ReceivingLoanId;   
        set
        {
            Model.ReceivingLoanId = value;
            OnPropertyChanged(nameof(ReceivingLoanId));
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
    public PaymentMethodViewModel PaymentMethod
    {
        get => new PaymentMethodViewModel { Model = Model.PaymentMethod };
        set
        {
            Model.PaymentMethod = value.Model;
            Model.PaymentMethodId = value.Model.Id;
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(PaymentMethodId));
        }
    }
    public ProviderViewModel Provider
    {
        get => new ProviderViewModel { Model = Model.Provider };
        set
        {
            Model.Provider = value.Model;
            Model.ProviderId = value.Model.Id;
            OnPropertyChanged(nameof(Provider));
            OnPropertyChanged(nameof(ProviderId));
        }
    }
    public ReceivingLoanViewModel? ReceivingLoan
    {
        get => new ReceivingLoanViewModel { Model = Model.ReceivingLoan };   // check for null
        set
        {
            Model.ReceivingLoan = (value == null) ? null : value.Model;
            Model.ReceivingLoanId = (value == null) ? null : value.Model.Id;
            OnPropertyChanged(nameof(ReceivingLoan));
            OnPropertyChanged(nameof(ReceivingLoanId));
        }
    }
}
