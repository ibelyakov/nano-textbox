﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NanoTextBox
{
    /// <summary>
    /// NanoTextBox
    /// </summary>
    public class NanoTextBox : TextBox
    {
        /// <summary>
        /// Ctor
        /// </summary>
        static NanoTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NanoTextBox),
                new FrameworkPropertyMetadata(typeof(NanoTextBox)));
        }

        /// <summary>
        /// Identifies the SelectionForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionForegroundProperty =
            DependencyProperty.Register(
                "SelectionForeground",
                typeof(Brush),
                typeof(NanoTextBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

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
    }
}
