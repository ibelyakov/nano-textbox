using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NanoTextBox
{
    /// <summary>
    /// Internal nano TextBox
    /// </summary>
    internal class InternalNanoTextBox : TextBox
    {
        private Point lastRenderPoint = new Point(0, 0);
        private double lastRenderWidth;

        /// <summary>
        /// Identifies the SelectionForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionForegroundProperty =
            DependencyProperty.Register(
                "SelectionForeground",
                typeof(Brush),
                typeof(InternalNanoTextBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static DependencyProperty BaseBackgroundProperty =
            DependencyProperty.Register(
                "BaseBackground",
                typeof(Brush),
                typeof(InternalNanoTextBox),
                new FrameworkPropertyMetadata(Panel.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

        public static DependencyProperty BaseForegroundProperty =
            DependencyProperty.Register(
                "BaseForeground",
                typeof(Brush),
                typeof(InternalNanoTextBox),
                new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the BaseSelectionBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseSelectionBrushProperty =
            DependencyProperty.Register(
                "BaseSelectionBrush",
                typeof(Brush),
                typeof(InternalNanoTextBox),
                new FrameworkPropertyMetadata(new SolidColorBrush(SystemColors.HighlightColor), FrameworkPropertyMetadataOptions.AffectsRender));

        public InternalNanoTextBox()
        {
            Background = new SolidColorBrush(Colors.Transparent);
            Foreground = new SolidColorBrush(Colors.Transparent);
            SelectionBrush = new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        /// Gets or sets the brush that highlights selected text.
        /// </summary>
        /// 
        /// <returns>
        /// The brush that highlights selected text.
        /// </returns>
        public Brush SelectionForeground
        {
            get { return (Brush)GetValue(SelectionForegroundProperty); }
            set { SetValue(SelectionForegroundProperty, value); }
        }


        /// <summary>
        /// Gets or sets a brush that describes the background of a control.
        /// </summary>
        /// 
        /// <returns>
        /// The brush that is used to fill the background of the control. The default is <see cref="P:System.Windows.Media.Brushes.Transparent"/>.
        /// </returns>
        [Bindable(true)]
        public Brush BaseBackground
        {
            get { return (Brush)GetValue(BaseBackgroundProperty); }
            set { SetValue(BaseBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color.
        /// </summary>
        /// 
        /// <returns>
        /// The brush that paints the foreground of the control. The default value is the system dialog font color.
        /// </returns>
        [Bindable(true)]
        public Brush BaseForeground
        {
            get { return (Brush)GetValue(BaseForegroundProperty); }
            set { SetValue(BaseForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush that highlights selected text.
        /// </summary>
        /// 
        /// <returns>
        /// The brush that highlights selected text.
        /// </returns>
        public Brush BaseSelectionBrush
        {
            get { return (Brush)GetValue(BaseSelectionBrushProperty); }
            set { SetValue(BaseSelectionBrushProperty, value); }
        }

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            InvalidateVisual();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
            drawingContext.DrawRectangle(BaseBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (string.IsNullOrEmpty(Text))
                return;

            var firstLine = GetFirstVisibleLineIndex();
            var firstChar = (firstLine == 0) ? 0 : GetCharacterIndexFromLineIndex(firstLine);

            var formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily.Source),
                FontSize,
                BaseForeground);

            var cRect = GetRectFromCharacterIndex(firstChar);
            var renderPoint = double.IsInfinity(cRect.Top)
                ? new Point(lastRenderPoint.X + lastRenderWidth - formattedText.Width, lastRenderPoint.Y)
                : new Point(cRect.Left, cRect.Top);

            if (IsFocused)
            {
                var selectionForeground = SelectionForeground;
                if (selectionForeground != null)
                {
                    formattedText.SetForegroundBrush(selectionForeground, SelectionStart, SelectionLength);
                }

                var highlightGeometry = formattedText.BuildHighlightGeometry(new Point(0, 0), SelectionStart,
                    SelectionLength);
                if (highlightGeometry != null)
                {
                    highlightGeometry.Transform = new TranslateTransform(renderPoint.X, renderPoint.Y);
                    drawingContext.DrawGeometry(BaseSelectionBrush, null, highlightGeometry);
                }
            }

            drawingContext.DrawText(formattedText, renderPoint);
            lastRenderPoint = renderPoint;
            lastRenderWidth = formattedText.Width;
        }
    }
}
