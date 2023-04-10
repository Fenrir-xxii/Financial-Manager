using Microsoft.EntityFrameworkCore;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp9_MyFinances.Models;
using WpfApp9_MyFinances.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfApp9_MyFinances.ViewModels;

public class MainWindowViewModel : NotifyPropertyChangedBase
{
    public MainWindowViewModel() 
    { 
        
        _db = new Database3MyFinancesContext();
        _allPaymentMethods = new List<PaymentMethod>();
        _allCategoriesExp = new List<CategoriesExp>();
        _categoriesExpItems = new ObservableCollection<TreeViewItem>();

        Task.Run(async () =>
        {
            _allPaymentMethods = await LoadPaymentMethodsAsync();
            _allPaymentMethods.ForEach(p => PaymentMethods.Add(new PaymentMethodViewModel(p)));
            _allCategoriesExp = await LoadCategoriesExpAsync();
            _allCategoriesExp.ForEach(c => CategoriesExp.Add(new CategoryExpViewModel(c)));
            //_allCategoriesExp.ForEach(c => CategoriesExpItems.Add(new TreeViewItem { Header = c.Title }));

            OnPropertyChanged(nameof(PaymentMethods));
            OnPropertyChanged(nameof(CategoriesExp));
            OnPropertyChanged(nameof(CategoriesExpItems));
        });
    }
    private Database3MyFinancesContext _db;

    public async Task<List<PaymentMethod>> LoadPaymentMethodsAsync()
    {
        return await _db.PaymentMethods.ToListAsync();
    }
    public async Task<List<CategoriesExp>> LoadCategoriesExpAsync()
    {
        return await _db.CategoriesExps.Include(x=> x.SubcategoriesExps).ToListAsync();
    }
    private List<PaymentMethod> _allPaymentMethods;

    public ObservableCollection<PaymentMethodViewModel> PaymentMethods
    {
        get 
        { 
            var collection = new ObservableCollection<PaymentMethodViewModel>();
            foreach(var pay in _allPaymentMethods)
            {
                collection.Add(new PaymentMethodViewModel(pay));
            }
            return collection;
        }
        set 
        {
            PaymentMethods = value;
            OnPropertyChanged(nameof(PaymentMethods));
        }
    }
    private List<CategoriesExp> _allCategoriesExp;
    public ObservableCollection<CategoryExpViewModel> CategoriesExp
    {
        get
        {
            var collection = new ObservableCollection<CategoryExpViewModel>();
            foreach (var category in _allCategoriesExp)
            {
                collection.Add(new CategoryExpViewModel(category));
            }
            return collection;
        }
        set
        {
            CategoriesExp = value;
            OnPropertyChanged(nameof(CategoriesExp));
            //OnPropertyChanged(nameof(CategoriesExpItems));
        }
    }
    private ObservableCollection<TreeViewItem> _categoriesExpItems = new ObservableCollection<TreeViewItem>();
    public ObservableCollection<TreeViewItem> CategoriesExpItems
    {
        get
        {
            // FIX no subcategory in category
            var collection = new ObservableCollection<TreeViewItem>();
            int i = 0;
            _allCategoriesExp.ForEach(c =>
            {
                collection.Add(new TreeViewItem { Header = c.Title });
               
                if(c.SubcategoriesExps.Count>0)//  c.SubcategoriesExps!=null)
                {
                    c.SubcategoriesExps.ToList().ForEach(s => collection[i].Items.Add(new TreeViewItem { Header = s.Title }));
                }
                i++;
            });
            return collection;
            //return _categoriesExpItems;
        }
        set
        {
            _categoriesExpItems = value;
            OnPropertyChanged(nameof(CategoriesExpItems));
            //OnPropertyChanged(nameof(CategoriesExp));
        }
    }
    public ICommand AddTransaction => new RelayCommand(x =>
    {
        var window = new AddTransaction();
        window.ShowDialog();

    }, x => true);
}
