using System.ComponentModel;
using System.Windows;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen
{
    public partial class StatusIconControl
    {
        #region Status

        public static readonly Status StatusDefault = Status.Success;

        protected virtual void OnStatusChanged(Status oldValue, Status newValue)
        {
        }

        protected virtual object OnCoerceStatus(Status candidateValue)
        {
            return candidateValue;
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(Status), typeof(StatusIconControl),
                                        new FrameworkPropertyMetadata(StatusDefault,
                                                                      FrameworkPropertyMetadataOptions.None,
                                                                      (obj, args) => ((StatusIconControl)obj).OnStatusChanged((Status)args.OldValue, (Status)args.NewValue),
                                                                      (obj, candidateValue) => ((StatusIconControl)obj).OnCoerceStatus((Status)candidateValue)));

        [Category("StatusIconControl")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Status Status
        {
            get { return (Status)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        #endregion

        public StatusIconControl()
        {
            InitializeComponent();
        }
    }
}
