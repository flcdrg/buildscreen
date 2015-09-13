using System.Collections.ObjectModel;
using System.ComponentModel;
using BuildScreen.Annotations;
using BuildScreen.ContinousIntegration.Entities;
using BuildScreen.ViewModels;

namespace BuildScreen
{
    public class MainWindowViewModel
    {
        public virtual ObservableCollection<BuildViewModel> Builds { get; } = new ObservableCollection<BuildViewModel>();
    }
}