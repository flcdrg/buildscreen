using System.Collections.ObjectModel;
using System.ComponentModel;
using BuildScreen.Annotations;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen
{
    public class MainWindowViewModel
    {
        public virtual ObservableCollection<BuildViewModel> Builds { get; } = new ObservableCollection<BuildViewModel>();
    }
}