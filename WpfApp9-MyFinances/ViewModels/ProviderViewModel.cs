using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels;

public class ProviderViewModel : NotifyPropertyChangedBase
{
    public ProviderViewModel() { }
    public ProviderViewModel(Provider provider)
    {
        Model= provider;
    }
    public Provider Model { get; set; }
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
    public int ProviderTypesId
    {
        get => Model.ProviderTypesId;
        set
        {
            Model.ProviderTypesId = value;
            OnPropertyChanged(nameof(ProviderTypesId));
        }
    }
    public ProviderTypeViewModel ProviderTypes
    {
        get => new ProviderTypeViewModel { Model = Model.ProviderTypes };
        set
        {
            Model.ProviderTypes = value.Model;
            Model.ProviderTypesId = value.Model.Id;
            OnPropertyChanged(nameof(ProviderTypes));
            OnPropertyChanged(nameof(ProviderTypesId));
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (!(obj is ProviderViewModel))
            return false;

        return Model.Id.Equals((obj as ProviderViewModel).Model.Id);
    }
}
