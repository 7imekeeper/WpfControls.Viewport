using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls
{
	/// <summary>
	/// Defines a content decorator that scales and stretches a single child without distortion to fill the available space.
	/// </summary>
	public class Viewport : Decorator
	{
		#region StretchDirection
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(
			typeof(Viewport), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		public StretchDirection StretchDirection { get => (StretchDirection)GetValue(StretchDirectionProperty); set => SetValue(StretchDirectionProperty, value); }
		#endregion StretchDirection

		protected double Scale { get; set; } = 1;

		protected override Size MeasureOverride(Size availableSize)
		{
			if (Child == null)
				return availableSize;

			Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

			Scale = Math.Min(availableSize.Height / Child.DesiredSize.Height, availableSize.Width / Child.DesiredSize.Width);

			if ((StretchDirection == StretchDirection.UpOnly && Scale < 1) ||
				(StretchDirection == StretchDirection.DownOnly && Scale > 1))
			{
				Scale = 1;
				Child.Measure(availableSize);
			}

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (Child == null || Scale == 0)
				return finalSize;

			Child.RenderTransform = new ScaleTransform(Scale, Scale);
			Child.Arrange(new Rect(new Point(0, 0), new Size(finalSize.Width / Scale, finalSize.Height / Scale)));
			return finalSize;
		}
	}
}