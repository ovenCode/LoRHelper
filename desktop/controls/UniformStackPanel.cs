using System.Windows;
using System.Windows.Controls;

namespace desktop.Controls
{
    public class UniformStackPanel : StackPanel
    {
        public double ItemSpacing
        {
            get => (double)GetValue(ItemSpacingProperty);
            set => SetValue(ItemSpacingProperty, value);
        }

        public static DependencyProperty ItemSpacingProperty =
            DependencyProperty.Register(nameof(ItemSpacing), typeof(double), typeof(UniformStackPanel),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
                
        // public Orientation Orientation
        // {
        //     get => (Orientation)GetValue(OrientationProperty);
        //     set => SetValue(OrientationProperty, value);
        // }
        // public static readonly DependencyProperty OrientationProperty =
        //     StackPanel.OrientationProperty.AddOwner(typeof(UniformStackPanel));


        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            bool isFirst = true;
            foreach (UIElement child in InternalChildren)
            {
                if (child == null || child.Visibility == Visibility.Collapsed) continue;
                child.Measure(constraint);
                Size desiredSize = child.DesiredSize;

                if (Orientation == Orientation.Vertical)
                {
                    size.Height += desiredSize.Height + (isFirst ? 0 : ItemSpacing);
                    size.Width = Math.Max(size.Width, desiredSize.Width);
                }
                else
                {
                    size.Width += desiredSize.Width + (isFirst ? 0 : ItemSpacing);
                    size.Height = Math.Max(size.Height, desiredSize.Height);
                }

                isFirst = false;
            }

            return size;
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point offset = new Point(0, 0);
            bool isFirst = true;

            foreach (UIElement child in InternalChildren)
            {
                if (child == null || child.Visibility == Visibility.Collapsed) continue;

                Size desiredSize = child.DesiredSize;

                if (!isFirst)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        offset.Y += ItemSpacing;
                    }
                    else
                    {
                        offset.X += ItemSpacing;
                    }
                }

                if (Orientation == Orientation.Vertical)
                {
                    child.Arrange(new Rect(0, offset.Y, arrangeSize.Width, desiredSize.Height));
                    offset.Y += desiredSize.Height;
                }
                else
                {
                    child.Arrange(new Rect(offset.X, 0, desiredSize.Width, arrangeSize.Height));
                    offset.X += desiredSize.Width;
                }

                isFirst = false;
            }

            return arrangeSize;
        }

    }
}