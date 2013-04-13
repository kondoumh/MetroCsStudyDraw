using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroCsStudyDraw.Entity;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.Foundation;

namespace MetroCsStudyDraw
{
    class Document
    {
        private static Document instance = new Document();
        private int _sequence = 0;
        private ObservableCollection<Element> _elements = new ObservableCollection<Element>();
        private const double MIN_WIDTH = 600;
        private const double MIN_HEIGHT = 600;
        private double minWidth = MIN_WIDTH;
        private double minHeight = MIN_HEIGHT;
        private double zoomRatio = 1;
        private IView _iView;
        
        private Document()
        {
            _elements.CollectionChanged +=
                new NotifyCollectionChangedEventHandler(elements_CollectionChanged);
        }

        /// <summary>
        /// IView を設定する。
        /// </summary>
        /// <param name="view"></param>
        public void setView(IView view)
        {
            _iView = view;
            _elements.CollectionChanged += new NotifyCollectionChangedEventHandler(view.Update);
            addMainNode();
        }

        private void addMainNode()
        {
            var node = new Node();
            node.Name = "主題";
            node.Width = 80;
            node.Height = 45;
            node.Left = 100;
            node.Top = 100;
            node.IsSelected = false;
            _elements.Add(node);
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        public static Document Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 要素を追加する
        /// </summary>
        /// <param name="element">追加する要素</param>
        public void Add(Element element)
        {
            _elements.Add(element);
        }
        
        // 要素の集合に変更があったら再計算
        private void elements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalcMinSize();
        }

        /// <summary>
        /// 選択された要素を列挙するためのIEnumerableを得る
        /// </summary>
        private IEnumerable<Element> Selection
        {
            get
            {
                return (from e in _elements
                        where e.IsSelected
                        select e);
            }
        }

        /// <summary>
        /// ElementsからNodeの集合を取得する
        /// </summary>
        private IEnumerable<Node> Nodes
        {
            get
            {
                return (from e in _elements
                        where e is Node
                        select e as Node);
            }
        }

        /// <summary>
        /// ElementsからEdgeの集合を取得する
        /// </summary>
        private IEnumerable<Edge> Edges
        {
            get
            {
                return (from e in _elements
                        where e is Edge
                        select e as Edge);
            }
        }
        
        /// <summary>
        /// 全要素を包含する最小幅
        /// </summary>
        public double MinWidth
        {
            get { return minWidth; }
            set
            {
                if ((double)value >= MIN_WIDTH * zoomRatio)
                {
                    minWidth = value;
                    OnPropertyChanged("MinWidth");
                }
            }
        }

        /// <summary>
        /// 全要素を包含する最小高
        /// </summary>
        public double MinHeight
        {
            get { return minHeight; }
            set
            {
                if ((double)value >= MIN_HEIGHT * zoomRatio)
                {
                    minHeight = value;
                    OnPropertyChanged("MinHeight");
                }
            }
        }

        /// <summary>
        /// ズーム表示の倍率を設定取得する
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public double ZoomRatio
        {
            get { return zoomRatio; }
            set
            {
                zoomRatio = value;
                CalcMinSize();
            }
        }

        /// <summary>
        /// 連番を取得する
        /// </summary>
        public int Sequence
        {
            get
            {
                _sequence++;
                return _sequence;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
       
        /// <summary>
        /// すべての要素を包含する最小のサイズを再計算する
        /// 今はpublicにしておいて、移動イベントなどの終了時に呼ぶようにしている
        /// </summary>
        public void CalcMinSize()
        {
            MinWidth = MIN_WIDTH * zoomRatio;
            MinHeight = MIN_HEIGHT * zoomRatio;

            foreach (var node in Nodes)
            {
                double right = node.Left + node.Width;
                if (right * zoomRatio > MinWidth)
                {
                    MinWidth = right * zoomRatio;
                }
                double bottom = node.Top + node.Height;
                if (bottom * zoomRatio > MinHeight)
                {
                    MinHeight = bottom * zoomRatio;
                }
            }
        }

        /// <summary>
        /// すべての要素を非選択にする
        /// </summary>
        public void UnSelectAllElements()
        {
            foreach (var elem in _elements)
            {
                elem.IsSelected = false;
            }
        }

        /// <summary>
        /// 指定した矩形内にある要素を選択状態にする
        /// </summary>
        /// <param name="left">矩形の左辺</param>
        /// <param name="top">矩形の上辺</param>
        /// <param name="width">矩形の幅</param>
        /// <param name="height">矩形の高さ</param>
        public void SelectElementsInBound(double left, double top, double width, double height)
        {
            foreach (var element in _elements)
            {
                // TODO 判定メソッドをElementの仮想クラスとして持たせた方がよい
                element.IsSelected = element.Contained(left, top, width, height);
            }
        }

        /// <summary>
        /// 選択された要素を移動する
        /// </summary>
        /// <param name="dx">X方向の移動量</param>
        /// <param name="dy">Y方向の移動量</param>
        public void MoveElementsSelected(double dx, double dy)
        {
            foreach (var element in Selection)
            {
                element.MoveTo(dx, dy);
            }
        }

        /// <summary>
        /// 要素のHitTest
        /// </summary>
        /// <param name="point">テストするPoint</param>
        /// <returns>ヒットした要素</returns>
        // TODO:DocumentでHitTestせずにControlにクリックイベントを
        //  トンネリング(バブル?)させた方がいろいろな形状に対応できる
        //  BindされたFigureとElementの対応表が必要
        public Element HitTest(Point point)
        {
            foreach (var element in _elements)
            {
                if (element.HitTest(point))
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// Edgeの位置を再設定する
        /// </summary>
        public void ResetEdges()
        {
            foreach (var edge in Edges)
            {
                edge.Reset();
            }
        }

    }
}
