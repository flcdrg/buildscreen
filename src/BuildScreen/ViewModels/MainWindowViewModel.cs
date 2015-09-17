using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BuildScreen.ContinousIntegration.Entities;
using BuildScreen.ViewModels;

namespace BuildScreen
{
    public class MainWindowViewModel
    {
        public virtual ObservableCollection<BuildViewModel> Builds { get; } = new ObservableCollection<BuildViewModel>();

        public void UpdateBuilds(IEnumerable<Build> newBuilds)
        {
            Builds.Clear();

            foreach (var newBuild in newBuilds)
            {
                var buildViewModel = Builds.FirstOrDefault(b => b.Build.UniqueIdentifier == newBuild.UniqueIdentifier);
                if (buildViewModel == null)
                {
                    buildViewModel = new BuildViewModel(newBuild);
                    Builds.Add(buildViewModel);
                }
                else
                {
                    buildViewModel.Build = newBuild;
                }
            }
        }
    }
}