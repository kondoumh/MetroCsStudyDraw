using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using MetroCsStudyDraw.Figure;
using MetroCsStudyDraw.Entity;
using MetroCsStudyDraw.Utility;
using Windows.UI.Xaml.Data;

namespace MetroCsStudyDraw
{
    class DiagramCanvas : Canvas, IView
    {
        private Shape _rubberBand;
        private Point _lastPoint;

        public DiagramCanvas()
        {
            this.PointerPressed += DiagramCanvas_PointerPressed;
            this.PointerMoved += DiagramCanvas_PointerMoved;
            this.PointerReleased += DiagramCanvas_PointerReleased;
            this.DoubleTapped += DiagramCanvas_DoubleTapped;
        }

        void DiagramCanvas_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            addFigureRectangle(e.GetPosition(this));
        }

        void DiagramCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Document.Instance.UnSelectAllElements();
            var point = e.GetCurrentPoint(this).Position;
            _lastPoint = point;
            if (_rubberBand == null)
            {
                _rubberBand = new Rectangle();
            }
            _rubberBand.StrokeThickness = 2;
            _rubberBand.Stroke = new SolidColorBrush(Colors.DarkGray);
            var dc = new DoubleCollection();
            dc.Add(4); dc.Add(1);
            _rubberBand.StrokeDashArray = dc;
            _rubberBand.Visibility = Visibility.Visible;
            _rubberBand.Width = 1;
            _rubberBand.Height = 1;
            Canvas.SetLeft(_rubberBand, point.X);
            Canvas.SetTop(_rubberBand, point.Y);
            this.Children.Add(_rubberBand);
            e.Handled = true;
        }

        void DiagramCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this).Position;
            if (_rubberBand == null) return;
            double dx = point.X - _lastPoint.X;
            double dy = point.Y - _lastPoint.Y;
            _rubberBand.Width = Math.Abs(dx);
            _rubberBand.Height = Math.Abs(dy);
            double left = Math.Min(_lastPoint.X, point.X);
            double top = Math.Min(_lastPoint.Y, point.Y);
            Canvas.SetLeft(_rubberBand, left);
            Canvas.SetTop(_rubberBand, top);
            e.Handled = true;
        }

        void DiagramCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Point point = e.GetCurrentPoint(this).Position;
            if (_rubberBand != null)
            {
                this.Children.Remove(_rubberBand);
                _rubberBand = null;
            }
            e.Handled = true;
            this.ReleasePointerCaptures();
            double left = Math.Min(point.X, _lastPoint.X);
            double top = Math.Min(point.Y, _lastPoint.Y);
            double width = Math.Abs(point.X - _lastPoint.X);
            double height = Math.Abs(point.Y - _lastPoint.Y);
            Document.Instance.SelectElementsInBound(left, top, width, height);
        }


        private void addFigureRectangle(Point point)
        {
            var node = new Node();
            //node.Left = Math.Min(point.X, _lastPoint.X);
            //node.Top = Math.Min(point.Y, _lastPoint.Y);
            //node.Width = Math.Abs(point.X - _lastPoint.X);
            //node.Height = Math.Abs(point.Y - _lastPoint.Y);
            node.Left = point.X; node.Top = point.Y; node.Width = 150; node.Height = 45;
            node.Name = "新しいノード" + Document.Instance.Sequence.ToString();
            node.FigureType = FigureTypes.Rectangle;
            Document.Instance.Add(node);
            node.IsSelected = true;
        }

        #region IView メンバー

        public void Update(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var node = e.NewItems[0] as Node;
            var rnd = new Random();
            if (node.Width <= 1)
            {
                node.Left = rnd.Next(300);
                node.Top = rnd.Next(300);
                node.Width = 80;
                node.Height = 40;
            }
            bindNodeFigure(node);
            Document.Instance.UnSelectAllElements();
            node.IsSelected = true;
        }

        #endregion

        private void bindNodeFigure(Node node)
        {
            var figure = new FigureRectangle();
            figure.DataContext = node;
            BindingHelper.BindProperty(figure, node, "FigureType",
                FigureRectangle.FigureTypeProperty, BindingMode.TwoWay);
            BindingHelper.BindProperty(figure, node, "IsSelected",
                FigureBase.TrackerVisibleProperty, BindingMode.TwoWay);
            this.Children.Add(figure);
        }

    }
}
