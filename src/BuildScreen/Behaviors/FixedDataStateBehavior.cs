using System.Windows.Data;
using Microsoft.Expression.Interactivity.Core;

namespace BuildScreen.Behaviors
{
    public class FixedDataStateBehavior : DataStateBehavior
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += (sender, routedEventArgs) =>
            {
                var bindingExpression = BindingOperations.GetBindingExpression(this, BindingProperty);
                SetCurrentValue(BindingProperty, new object());
                bindingExpression?.UpdateTarget();
            };
        }
    }
}