using System;
using System.Collections.ObjectModel;
using BuildScreen.ContinousIntegration.Entities;
using BuildScreen.ViewModels;

namespace BuildScreen.Design
{
    public class DesignMainWindowViewModel : MainWindowViewModel
    {
        public override ObservableCollection<BuildViewModel> Builds =>
            new ObservableCollection<BuildViewModel>(new[]
                {
                    new BuildViewModel(CreateDummyBuild1()),
                    new BuildViewModel(CreateDummyBuild2()),
                    new BuildViewModel(CreateDummyBuild1()),
                    new BuildViewModel(CreateDummyBuild2())
                });

        private static Build CreateDummyBuild1()
        {
            return new Build
                {
                    Number = "42",
                    Status = Status.Success,
                    StatusText = "All is good",
                    StartDate = DateTime.UtcNow,
                    FinishDate = DateTime.UtcNow,
                    ProjectName = "MyProject1",
                    TypeName = "MyType1",
                    UniqueIdentifier = "myprojectuniqueid1",
                    PercentageComplete = 32
                };
        }

        private static Build CreateDummyBuild2()
        {
            return new Build
            {
                Number = "84",
                Status = Status.Fail,
                StatusText = "Something went wrong",
                StartDate = DateTime.UtcNow,
                FinishDate = DateTime.UtcNow,
                ProjectName = "MyProject2",
                TypeName = "MyType2",
                UniqueIdentifier = "myprojectuniqueid2",
                PercentageComplete = 100
            };
        }
    }
}