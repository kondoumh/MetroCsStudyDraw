using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace MetroCsStudyDraw.Figure
{
    public abstract class FigureBase : UserControl
    {
        #region Fields

        // Canvas内のマウスポイントのオフセット値を格納
        protected double offSetX;
        protected double offSetY;
        // マウスポイント時のキャンバス座標を格納
        protected Point lastPoint;
        // 直前のカーソル位置を格納
        protected Point prevPoint;

        // ハンドルの大きさ
        protected const double HandleSize = 9.0;

        // リサイズ開始時のトラッカハンドル保持用 初期値は0
        protected int handleNumber = 0;

        // トラッカの可視性を表す依存プロパティ
        public static readonly DependencyProperty TrackerVisibleProperty = DependencyProperty.Register(
            "TrackerVisible",
            typeof(bool),
            typeof(FigureBase),
            new PropertyMetadata(null, OnTrackerVisibiltyChanged));

        private static void OnTrackerVisibiltyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as FigureBase).ChangeTrackerControlVisiblity((bool)e.NewValue);
        }

        #endregion Fields


        #region MouseEventHelper

        /// <summary>
        /// コントロールがタップされた時の位置情報を保持する
        /// </summary>
        /// <param name="point"></param>
        protected void BackupTapPoint(Point point)
        {
            lastPoint = point;
            prevPoint = point;
            offSetX = Math.Abs(Canvas.GetLeft(this) - point.X);
            offSetY = Math.Abs(Canvas.GetTop(this) - point.Y);
        }

        #endregion MouseEventHelper

        #region abstract Methods and Properties

        /// <summary>
        /// ハンドル数を取得する抽象プロパティ
        /// </summary>
        public abstract int HandleCount { get; }

        /// <summary>
        /// ハンドル番号(1～)に対するハンドルの位置を返す抽象メソッド
        /// </summary>
        public abstract Point GetHandle(int handleNumber);

        public abstract void ChangeTrackerControlVisiblity(bool visible);

        #endregion abstract Methods and Properties

        #region Tracker

        /// <summary>
        /// トラッカの可視性
        /// </summary>
        public bool TrackerVisible
        {
            get
            {
                return (bool)GetValue(TrackerVisibleProperty);
            }
            set
            {
                SetValue(TrackerVisibleProperty, value);
            }
        }


        /// <summary>
        /// トラッカの1を起点とするハンドル番号に対応した矩形を返す
        /// </summary>
        public Rect GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rect(point.X - HandleSize / 2, point.Y - HandleSize / 2,
                HandleSize, HandleSize);
        }

        /// <summary>
        /// 選択トラッカトラッカ用のハンドルのヒットテスト
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected int HitTestHandle(Point p)
        {
            for (int i = 1; i <= HandleCount; i++)
            {
                Rect r = GetHandleRectangle(i);
                if (r.Contains(p))
                {
                    return i;
                }
            }
            return 0;
        }
        #endregion Tracker

    }
}
