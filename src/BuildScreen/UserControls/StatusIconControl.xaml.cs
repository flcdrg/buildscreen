using System.ComponentModel;
using System.Windows;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen
{
    public partial class StatusIconControl
    {
        #region BuildStatus

        public static readonly BuildStatus BuildStatusDefault = BuildStatus.Success;

        protected virtual void OnStatusChanged(BuildStatus oldValue, BuildStatus newValue)
        {
        }

        protected virtual object OnCoerceStatus(BuildStatus candidateValue)
        {
            return candidateValue;
        }

        public static readonly DependencyProperty BuildStatusProperty =
            DependencyProperty.Register("BuildBuildStatus", typeof(BuildStatus), typeof(StatusIconControl),
                                        new FrameworkPropertyMetadata(BuildStatusDefault,
                                                                      FrameworkPropertyMetadataOptions.None,
                                                                      (obj, args) => ((StatusIconControl)obj).OnStatusChanged((BuildStatus)args.OldValue, (BuildStatus)args.NewValue),
                                                                      (obj, candidateValue) => ((StatusIconControl)obj).OnCoerceStatus((BuildStatus)candidateValue)));

        [Category("StatusIconControl")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public BuildStatus BuildStatus
        {
            get { return (BuildStatus)GetValue(BuildStatusProperty); }
            set { SetValue(BuildStatusProperty, value); }
        }

        #endregion

        public StatusIconControl()
        {
            InitializeComponent();
        }
    }
}
