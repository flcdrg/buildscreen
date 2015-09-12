using System.Collections.ObjectModel;
using System.ComponentModel;
using BuildScreen.Annotations;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen
{
    public class MainWindowViewModel
    {
        public virtual ObservableCollection<Build> Builds { get; } = new ObservableCollection<Build>();
    }
}