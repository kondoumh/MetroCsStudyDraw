using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace MetroCsStudyDraw.Figure
{
    /// <summary>
    /// 矩形コントロールの基底クラス
    /// </summary>
    public class FigureRectangleBase : FigureBase
    {
        #region Fields

        // リサイズ開始時に矩形のオリジナル情報を対比するためのメンバー
        private double leftOrg, topOrg, rightOrg, bottomOrg, widthOrg, heightOrg;
        // ドラッグ開始フラグ
        private bool beganDrag = false;

        #endregion Fields

        #region Ctors

        public FigureRectangleBase()
        {
            this.PointerPressed += FigureRectangleBase_PointerPressed;
            this.PointerMoved += FigureRectangleBase_PointerMoved;
            this.PointerReleased += FigureRectangleBase_PointerReleased;
            this.PointerEntered += FigureRectangleBase_PointerEntered;
            this.PointerExited += FigureRectangleBase_PointerExited;
        }

        #endregion Ctors

        #region Event Handlers

        void FigureRectangleBase_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Point point = e.GetCurrentPoint((UIElement)this.Parent).Position;
            BackupTapPoint(point);
            Point ptInBound = e.GetCurrentPoint(this).Position;

            Point pointInner = e.GetCurrentPoint(this).Position;
            handleNumber = HitTestHandle(pointInner);

            topOrg = Canvas.GetTop(this);
            leftOrg = Canvas.GetLeft(this);
            widthOrg = this.Width;
            heightOrg = this.Height;
            rightOrg = leftOrg + widthOrg;
            bottomOrg = topOrg + heightOrg;

            CapturePointer(e.Pointer);
            beganDrag = true;
            if (!TrackerVisible)
            {
                Document.Instance.UnSelectAllElements();
            }

            e.Handled = true;
            TrackerVisible = true;
            //Document.Instance.UpdateSelection();
        }

        void FigureRectangleBase_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Point pointInner = e.GetCurrentPoint(this).Position;

            if (!beganDrag) return;

            Point point = e.GetCurrentPoint((UIElement)this.Parent).Position;
            if (handleNumber != 0)
            {
                ResizeControl(e, handleNumber);
            }
            else
            {
                double left = point.X - offSetX;
                double top = point.Y - offSetY;
                Canvas.SetLeft(this, left >= 0 ? left : 0);
                Canvas.SetTop(this, top >= 0 ? top : 0);
                double dx = point.X - prevPoint.X;
                double dy = point.Y - prevPoint.Y;
                Document.Instance.MoveElementsSelected(dx, dy);
                prevPoint = point;
            }
            //Document.Instance.ResetEdges();
        }

        void FigureRectangleBase_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            handleNumber = 0;
            if (beganDrag)
            {
                beganDrag = false;
                e.Handled = true;
                ReleasePointerCaptures();
                Document.Instance.CalcMinSize();
            }
        }

        void FigureRectangleBase_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // NOP
        }

        void FigureRectangleBase_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            handleNumber = 0;
            beganDrag = false;
        }

        #endregion Event Handlers


        #region Overrides

        /// <summary>
        /// ハンドルの数を取得するプロパティ
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 1を起点するハンドル番号のハンドルの座標を取得する
        /// 矩形の左上を基点とする座標が取得される
        /// </summary>
        public override Point GetHandle(int handleNumber)
        {
            double left = 0;
            double top = 0;
            double right = left + this.Width;
            double bottom = top + this.Height;
            double x = left;
            double y = top;
            double xCenter = (right + left) / 2;
            double yCenter = (bottom + top) / 2;

            switch (handleNumber)
            {
                case 1:
                    x = left;
                    y = top;
                    break;
                case 2:
                    x = xCenter;
                    y = top;
                    break;
                case 3:
                    x = right;
                    y = top;
                    break;
                case 4:
                    x = right;
                    y = yCenter;
                    break;
                case 5:
                    x = right;
                    y = bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = bottom;
                    break;
                case 7:
                    x = left;
                    y = bottom;
                    break;
                case 8:
                    x = left;
                    y = yCenter;
                    break;
            }
            return new Point(x, y);
        }

        #endregion Overrides

        #region Resize Method

        /// <summary>
        /// ドラッグ中のトラッカハンドルの位置によりコントロールをリサイズする
        /// </summary>
        protected void ResizeControl(Windows.UI.Xaml.Input.PointerRoutedEventArgs e, int HandleNumber)
        {
            if (handleNumber == 0) return;

            Point point = e.GetCurrentPoint((UIElement)this.Parent).Position;
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);
            double width = this.Width;
            double height = this.Height;

            switch (handleNumber)
            {
                case 1:
                    left = Math.Min(point.X, rightOrg);
                    top = Math.Min(point.Y, bottomOrg);
                    width = Math.Abs(point.X - rightOrg);
                    height = Math.Abs(point.Y - bottomOrg);
                    break;
                case 2:
                    top = Math.Min(point.Y, bottomOrg);
                    height = Math.Abs(point.Y - bottomOrg);
                    break;
                case 3:
                    left = Math.Min(point.X, leftOrg);
                    top = Math.Min(point.Y, bottomOrg);
                    width = Math.Abs(point.X - leftOrg);
                    height = Math.Abs(point.Y - bottomOrg);
                    break;
                case 4:
                    left = Math.Min(leftOrg, point.X);
                    width = Math.Abs(leftOrg - point.X);
                    break;
                case 5:
                    left = Math.Min(point.X, leftOrg);
                    top = Math.Min(point.Y, topOrg);
                    width = Math.Abs(point.X - leftOrg);
                    height = Math.Abs(point.Y - topOrg);
                    break;
                case 6:
                    top = Math.Min(point.Y, topOrg);
                    height = Math.Abs(point.Y - topOrg);
                    break;
                case 7:
                    left = Math.Min(point.X, rightOrg);
                    top = Math.Min(point.Y, topOrg);
                    width = Math.Abs(point.X - rightOrg);
                    height = Math.Abs(point.Y - topOrg);
                    break;
                case 8:
                    left = Math.Min(point.X, rightOrg);
                    width = Math.Abs(point.X - rightOrg);
                    break;
            }
            Canvas.SetLeft(this, left >= 0 ? left : 0);
            Canvas.SetTop(this, top >= 0 ? top : 0);
            this.Width = width;
            this.Height = height;
        }

        #endregion Resize Method

        public override void ChangeTrackerControlVisiblity(bool visible) { }

    }
}
