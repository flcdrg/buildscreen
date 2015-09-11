using System.Windows.Input;

namespace BuildScreen.Core.Extensions
{
    public static class MouseEventArgsExtensions
    {
        public static bool NoButtonIsPressed(this MouseEventArgs e)
        {
            return e.LeftButton == MouseButtonState.Released &&
                   e.RightButton == MouseButtonState.Released &&
                   e.MiddleButton == MouseButtonState.Released &&
                   e.XButton1 == MouseButtonState.Released &&
                   e.XButton2 == MouseButtonState.Released;
        }
    }
}
