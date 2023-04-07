using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class TransferViewModel : NotifyPropertyChangedBase
{
    public TransferViewModel() { }
    public TransferViewModel(Transfer transfer) 
    {
        Model = transfer;
    }
    public Transfer Model { get; set; }
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
    public int FromId
    {
        get => Model.FromId;
        set
        {
            Model.FromId = value;
            OnPropertyChanged(nameof(FromId));
        }
    }
    public int ToId
    {
        get => Model.ToId;
        set
        {
            Model.ToId = value;
            OnPropertyChanged(nameof(ToId));
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
    public PaymentMethodViewModel From
    {
        get => new PaymentMethodViewModel { Model = Model.From };
        set
        {
            Model.From = value.Model;
            Model.FromId = value.Model.Id;
            OnPropertyChanged(nameof(From));
            OnPropertyChanged(nameof(FromId));
        }
    }
    public PaymentMethodViewModel To
    {
        get => new PaymentMethodViewModel { Model = Model.To };
        set
        {
            Model.To = value.Model;
            Model.ToId = value.Model.Id;
            OnPropertyChanged(nameof(To));
            OnPropertyChanged(nameof(ToId));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is TransferViewModel))
            return false;

        return Model.Id.Equals((obj as TransferViewModel).Model.Id);
    }
}
