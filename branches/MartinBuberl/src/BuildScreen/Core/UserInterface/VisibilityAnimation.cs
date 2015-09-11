using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace BuildScreen.Core.UserInterface
{
    /// <summary>
    /// Supplies attached properties that provides visibility of animations.
    /// </summary>
    /// <see href="http://blogs.microsoft.co.il/blogs/arik/archive/2010/02/08/wpf-how-to-animate-visibility-property.aspx"/>
    public class VisibilityAnimation
    {
        public enum AnimationType
        {
            None,
            Fade
        }

        /// <summary>Duration of the animation in milliseconds.</summary>
        private const int AnimationDuration = 600;

        /// <summary>List of hooked objects.</summary>
        private static readonly Dictionary<FrameworkElement, bool> _hookedElements = new Dictionary<FrameworkElement, bool>();

        /// <summary>
        /// Get AnimationType attached property.
        /// </summary>
        public static AnimationType GetAnimationType(DependencyObject dependencyObject)
        {
            return (AnimationType)dependencyObject.GetValue(AnimationTypeProperty);
        }

        /// <summary>
        /// Set AnimationType attached property.
        /// </summary>
        public static void SetAnimationType(DependencyObject dependencyObject, AnimationType animationType)
        {
            dependencyObject.SetValue(AnimationTypeProperty, animationType);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for AnimationType.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.RegisterAttached("AnimationType", typeof(AnimationType), typeof(VisibilityAnimation), new FrameworkPropertyMetadata(AnimationType.None, new PropertyChangedCallback(OnAnimationTypePropertyChanged)));

        /// <summary>
        /// AnimationType property changed
        /// </summary>
        private static void OnAnimationTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement == null)
                return;

            if (GetAnimationType(frameworkElement) != AnimationType.None)
            {
                // add this framework element to hooked list
                HookVisibilityChanges(frameworkElement);
            }
            else
            {
                // otherwise, remove it from the hooked list
                UnHookVisibilityChanges(frameworkElement);
            }
        }

        /// <summary>
        /// Add framework element to list of hooked objects.
        /// </summary>
        private static void HookVisibilityChanges(FrameworkElement frameworkElement)
        {
            _hookedElements.Add(frameworkElement, false);
        }

        /// <summary>
        /// Remove framework element from list of hooked objects.
        /// </summary>
        private static void UnHookVisibilityChanges(FrameworkElement frameworkElement)
        {
            if (_hookedElements.ContainsKey(frameworkElement))
                _hookedElements.Remove(frameworkElement);
        }

        static VisibilityAnimation()
        {
            // here we "register" on Visibility property "before change" event
            UIElement.VisibilityProperty.AddOwner(typeof(FrameworkElement), new FrameworkPropertyMetadata(Visibility.Visible, VisibilityChanged, CoerceVisibility));
        }

        private static void VisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            // ignore
        }

        private static object CoerceVisibility(DependencyObject dependencyObject, object baseValue)
        {
            // make sure object is a framework element
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement == null)
                return baseValue;

            Visibility visibility = (Visibility)baseValue;

            // If Visibility value hasn't change, do nothing. This can happen if the Visibility property is set using data binding and the 
            // binding source has changed but the new visibility value hasn't changed.
            if (visibility == frameworkElement.Visibility)
                return baseValue;

            // If element is not hooked by our attached property, stop here.
            if (!IsHookedElement(frameworkElement))
                return baseValue;

            // Update animation flag. If animation already started, don't restart it (otherwise, infinite loop).
            if (UpdateAnimationStartedFlag(frameworkElement))
                return baseValue;

            // If we get here, it means we have to start fade in or fade out animation. In any case return value of this method will be 
            // Visibility.Visible, to allow the animation.
            DoubleAnimation doubleAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromMilliseconds(AnimationDuration)) };

            // When animation completes, set the visibility value to the requested value (baseValue).
            doubleAnimation.Completed += (sender, eventArgs) =>
            {
                if (visibility == Visibility.Visible)
                {
                    // In case we change into Visibility.Visible, the correct value is already set, so just update the animation started flag.
                    UpdateAnimationStartedFlag(frameworkElement);
                }
                else
                {
                    // This will trigger value coercion again but UpdateAnimationStartedFlag() function will reture true this time, thus 
                    // animation will not be triggered.
                    if (BindingOperations.IsDataBound(frameworkElement, UIElement.VisibilityProperty))
                    {
                        // set visiblity using bounded value
                        Binding bindingValue = BindingOperations.GetBinding(frameworkElement, UIElement.VisibilityProperty);

                        if (bindingValue != null)
                            BindingOperations.SetBinding(frameworkElement, UIElement.VisibilityProperty, bindingValue);
                    }
                    else
                    {
                        // no binding, just assign the value
                        frameworkElement.Visibility = visibility;
                    }
                }
            };

            if (visibility == Visibility.Collapsed || visibility == Visibility.Hidden)
            {
                // fade out by animating opacity
                doubleAnimation.From = 1.0;
                doubleAnimation.To = 0.0;
            }
            else
            {
                // fade in by animating opacity
                doubleAnimation.From = 0.0;
                doubleAnimation.To = 1.0;
            }

            // start animation
            frameworkElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

            // Make sure the element remains visible during the animation. The original requested value will be set in the completed event of 
            // the animation
            return Visibility.Visible;
        }

        /// <summary>
        /// Check if framework element is hooked with AnimationType property.
        /// </summary>
        private static bool IsHookedElement(FrameworkElement frameworkElement)
        {
            return _hookedElements.ContainsKey(frameworkElement);
        }

        /// <summary>
        /// Update animation started flag or a given framework element.
        /// </summary>
        private static bool UpdateAnimationStartedFlag(FrameworkElement frameworkElement)
        {
            bool animationStarted = _hookedElements[frameworkElement];
            _hookedElements[frameworkElement] = !animationStarted;

            return animationStarted;
        }
    }
}
