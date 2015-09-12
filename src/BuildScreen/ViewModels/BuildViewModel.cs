using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen
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
            => string.IsNullOrEmpty(_build.ProjectName) ? $"Build {_build.Number}" : $"Build {_build.Number}, {_build.StatusText}";
    }
}