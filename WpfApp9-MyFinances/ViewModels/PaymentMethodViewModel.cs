using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class PaymentMethodViewModel : NotifyPropertyChangedBase
{
    public PaymentMethodViewModel() { }
    public PaymentMethodViewModel(PaymentMethod payment)
    {
        Model = payment;
    }
    public PaymentMethod Model { get; set; }
    public int Id { get => Model.Id; }
    public string Title
    {
        get => Model.Title;
        set
        {
            Model.Title = value;
            OnPropertyChanged(nameof(Title));
        }
    }
    public string? Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }
    public decimal CurrentBalance
    {
        get => Model.CurrentBalance;
        set
        {
            Model.CurrentBalance = value;
            OnPropertyChanged(nameof(CurrentBalance));
        }
    }
    public bool IsCash
    {
        get => Model.IsCash; 
        set
        {
            Model.IsCash = value;
            OnPropertyChanged(nameof(IsCash));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is PaymentMethodViewModel))
            return false;

        return Model.Id.Equals((obj as PaymentMethodViewModel).Model.Id);
    }
}

