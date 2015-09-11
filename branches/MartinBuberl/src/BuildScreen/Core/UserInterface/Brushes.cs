using System;
using System.Windows;
using System.Windows.Media;

namespace BuildScreen.Core.UserInterface
{
    public static class Brushes
    {
        private static readonly Lazy<LinearGradientBrush> _brushNeutral = new Lazy<LinearGradientBrush>(CreateBrushNeutral);
        private static readonly Lazy<LinearGradientBrush> _brushSuccess = new Lazy<LinearGradientBrush>(CreateBrushSuccess);
        private static readonly Lazy<LinearGradientBrush> _brushFailure = new Lazy<LinearGradientBrush>(CreateBrushFailure);

        private static LinearGradientBrush CreateBrushNeutral()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush {StartPoint = new Point(0.1, 0), EndPoint = new Point(0.1, 1)};
// ReSharper disable PossibleNullReferenceException
            linearGradientBrush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#7F7F7F"), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color) ColorConverter.ConvertFromString("#4B4B4B"), 1));
// ReSharper restore PossibleNullReferenceException

            return linearGradientBrush;
        }

        private static LinearGradientBrush CreateBrushSuccess()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush { StartPoint = new Point(0.1, 0), EndPoint = new Point(0.1, 1) };
// ReSharper disable PossibleNullReferenceException
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#83BE40"), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#4C7422"), 1));
// ReSharper restore PossibleNullReferenceException

            return linearGradientBrush;
        }

        private static LinearGradientBrush CreateBrushFailure()
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush { StartPoint = new Point(0.1, 0), EndPoint = new Point(0.1, 1) };
// ReSharper disable PossibleNullReferenceException
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#BE4040"), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#742222"), 1));
// ReSharper restore PossibleNullReferenceException

            return linearGradientBrush;
        }

        public static LinearGradientBrush BrushNeutral
        {
            get { return _brushNeutral.Value; }
        }

        public static LinearGradientBrush BrushSuccess
        {
            get { return _brushSuccess.Value; }
        }

        public static LinearGradientBrush BrushFailure
        {
            get { return _brushFailure.Value; }
        }
    }
}