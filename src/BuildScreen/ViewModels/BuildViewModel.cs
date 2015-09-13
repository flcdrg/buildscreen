using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen.ViewModels
{
    public class BuildViewModel
    {
        private readonly Build _build;

        public BuildViewModel(Build build)
        {
            _build = build;
        }

        public Status Status
            => _build.Status;

        public string BuildNumber
            => _build.Number;

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_build.ProjectName))
                    return _build.TypeName;

                if (_build.TypeName == _build.ProjectName)
                    return _build.ProjectName;

                return $"{_build.ProjectName} ({_build.TypeName})";
            }
        }

        public string Subtitle
            => _build.StatusText;
    }
}