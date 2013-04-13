using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MetroCsStudyDraw.Utility;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace MetroCsStudyDraw
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Document.Instance.setView(canvas);
            ToolBar.DataContext = ToolButtonState.Instance;
            ToolButtonState.Instance.FigureButtonState = FigureButtonStates.Select;
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
