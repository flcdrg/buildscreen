﻿using System;
using System.Collections.ObjectModel;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen.Design
{
    public class DesignMainWindowViewModel : MainWindowViewModel
    {
        public override ObservableCollection<Build> Builds =>
            new ObservableCollection<Build>(new[] { CreateDummyBuild1(), CreateDummyBuild2(), CreateDummyBuild1(), CreateDummyBuild2() });

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
                    UniqueIdentifier = "myprojectuniqueid1"
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
                UniqueIdentifier = "myprojectuniqueid2"
            };
        }
    }
}