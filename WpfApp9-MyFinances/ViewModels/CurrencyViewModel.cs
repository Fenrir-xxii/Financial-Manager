using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class CurrencyViewModel : NotifyPropertyChangedBase
{
    public CurrencyViewModel() { Model = new Currency(); }
    public CurrencyViewModel(Currency currency)
    {
        Model = currency;
    }
    public Currency Model { get; set; }
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
    public int CodeNumber
    {
        get => Model.CodeNumber;
        set
        {
            Model.CodeNumber = value;
            OnPropertyChanged(nameof(CodeNumber));
        }
    }
    public string CodeLetter
    {
        get => Model.CodeLetter;
        set
        {
            Model.CodeLetter = value;
            OnPropertyChanged(nameof(CodeLetter));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is CurrencyViewModel))
            return false;

        return Model.Id.Equals((obj as CurrencyViewModel).Model.Id);
    }
}
