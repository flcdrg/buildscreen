using System.ComponentModel;
using BuildScreen.Annotations;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen.ViewModels
{
    public class BuildViewModel : INotifyPropertyChanged
    {
        private Build _build;

        public Build Build
        {
            get { return _build; }
            set
            {
                _build = value;
                OnPropertyChanged(nameof(Build));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(BuildNumber));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Subtitle));
                OnPropertyChanged(nameof(RatioComplete));
            }
        }

        public BuildViewModel(Build build)
        {
            Build = build;
        }

        #region Status

        public Status Status => Build.Status;

        #endregion

        #region BuildNumber

        public string BuildNumber => Build.Number;

        #endregion

        #region Title

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(Build.ProjectName))
                    return Build.TypeName;

                if (Build.TypeName == Build.ProjectName)
                    return Build.ProjectName;

                return $"{Build.ProjectName} ({Build.TypeName})";
            }
        }

        #endregion

        #region Subtitle

        public string Subtitle => Build.StatusText;

        #endregion

        #region RatioComplete

        public double RatioComplete => Build.PercentageComplete / 100.0;

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}