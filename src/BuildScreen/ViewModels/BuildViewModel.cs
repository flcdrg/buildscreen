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
            => string.IsNullOrEmpty(_build.ProjectName) ? _build.TypeName : $"{_build.ProjectName}, {_build.TypeName}";

        public string Subtitle
            => _build.StatusText;
    }
}