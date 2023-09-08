using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp9_MyFinances.Models;

namespace WpfApp9_MyFinances.ViewModels
{
    public class ProviderTypeViewModel : NotifyPropertyChangedBase
    {
        public ProviderTypeViewModel() { }
        public ProviderTypeViewModel(ProviderType type)
        {
            Model = type;
        }
        public ProviderType Model { get; set; }
        public int Id { get => Model.Id; }
        public string Title
        {
            get => Model.Title.Trim();
            set
            {
                Model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public Brush Color
        {
            get
            {
                var type = Title.ToLower();
                switch (type)
                {
                    case "internet-shop":
                        return Brushes.Blue;
                    case "gas station":
                        return Brushes.Green;
                    case "post":
                        return Brushes.Red;
                    case "web services":
                        return Brushes.BlueViolet;
                    case "shop":
                        return Brushes.DarkBlue;
                    case "stream":
                        return Brushes.DarkOrange;
                    case "cafe":
                        return Brushes.Brown;
                    case "charity":
                        return Brushes.Orchid;
                    case "mobile telecom.":
                        return Brushes.LightSeaGreen;
                    case "cable":
                        return Brushes.DarkSlateBlue;
                    case "other":
                        return Brushes.SlateGray;
                    default:
                        return Brushes.Black;
                }
            }
            set
            {
                Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ProviderTypeViewModel))
                return false;

            return Model.Id.Equals((obj as ProviderTypeViewModel).Model.Id);
        }
    }
}
