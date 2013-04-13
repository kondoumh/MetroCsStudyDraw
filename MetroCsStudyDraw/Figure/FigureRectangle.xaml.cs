using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MetroCsStudyDraw.Entity;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace MetroCsStudyDraw.Figure
{
    public sealed partial class FigureRectangle : FigureRectangleBase
    {
        public FigureRectangle()
        {
            this.InitializeComponent();
            this.SizeChanged += FigureRectangle_SizeChanged;
        }

        public static DependencyProperty FigureTypeProperty = DependencyProperty.Register(
            "FigureType",
            typeof(FigureTypes),
            typeof(FigureRectangle),
            new PropertyMetadata(null, OnFigureTypeChanged));

        public FigureTypes FigureType
        {
            get { return (FigureTypes)GetValue(FigureTypeProperty); }
            set { SetValue(FigureTypeProperty, value); }
        }

        private static void OnFigureTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as FigureRectangle).changeFigureType((FigureTypes)e.NewValue);
        }

        private void changeFigureType(FigureTypes type)
        {
            switch (type)
            {
                case FigureTypes.Rectangle:
                    ShapeRectangle.Visibility = Visibility.Visible;
                    ShapeRoundedRect.Visibility = Visibility.Collapsed;
                    ShapeEllipse.Visibility = Visibility.Collapsed;
                    break;
                case FigureTypes.RoundedRect:
                    ShapeRectangle.Visibility = Visibility.Collapsed;
                    ShapeRoundedRect.Visibility = Visibility.Visible;
                    ShapeEllipse.Visibility = Visibility.Collapsed;
                    break;
                case FigureTypes.Ellipse:
                    ShapeRectangle.Visibility = Visibility.Collapsed;
                    ShapeRoundedRect.Visibility = Visibility.Collapsed;
                    ShapeEllipse.Visibility = Visibility.Visible;
                    break;
            }
        }

        public override void ChangeTrackerControlVisiblity(bool visible)
        {
            Tracker.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        void FigureRectangle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(tr1, -2.5);
            Canvas.SetTop(tr1, -2.5);
            Canvas.SetLeft(tr2, this.ActualWidth / 2 - 2.5);
            Canvas.SetTop(tr2, -2.5);
            Canvas.SetLeft(tr3, this.ActualWidth - 2.5);
            Canvas.SetTop(tr3, -2.5);
            Canvas.SetLeft(tr4, this.ActualWidth - 2.5);
            Canvas.SetTop(tr4, this.ActualHeight / 2 - 2.5);
            Canvas.SetLeft(tr5, this.ActualWidth - 2.5);
            Canvas.SetTop(tr5, this.ActualHeight - 2.5);
            Canvas.SetLeft(tr6, this.ActualWidth / 2 - 2.5);
            Canvas.SetTop(tr6, this.ActualHeight - 2.5);
            Canvas.SetLeft(tr7, -2.5);
            Canvas.SetTop(tr7, this.ActualHeight - 2.5);
            Canvas.SetLeft(tr8, -2.5);
            Canvas.SetTop(tr8, this.ActualHeight / 2 - 2.5);
        }
    }
}
