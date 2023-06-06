using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class ReceivingLoanViewModel : NotifyPropertyChangedBase
{
    public ReceivingLoanViewModel()
    {
        Model = new ReceivingLoan();
    }
    public ReceivingLoanViewModel(ReceivingLoan loan)
    {
        Model = loan;
    }
    public ReceivingLoan Model { get; set; }
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
    public int? GivingLoanId
    {
        get => Model.GivingLoanId;
        set
        {
            Model.GivingLoanId = value;
            OnPropertyChanged(nameof(GivingLoanId));
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
    public GivingLoanViewModel? GivingLoan
    {
        get => new GivingLoanViewModel { Model = Model.GivingLoan };   // check for null
        set
        {
            Model.GivingLoan = (value == null) ? null : value.Model;
            Model.GivingLoanId = (value == null) ? null : value.Model.Id;
            OnPropertyChanged(nameof(GivingLoan));
            OnPropertyChanged(nameof(GivingLoanId));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is ReceivingLoanViewModel))
            return false;

        return Model.Id.Equals((obj as ReceivingLoanViewModel).Model.Id);
    }
}
