using System.ComponentModel;

namespace MetroCsStudyDraw.Utility
{
    public enum FigureButtonStates
    {
        Select,
        Rectangle,
        RoundedRect,
        Ellipse,
        Line,
        Polyline
    }

    class ToolButtonState
    {
        private static ToolButtonState instance = new ToolButtonState();
        private ToolButtonState() { }
        
        public static ToolButtonState Instance
        {
            get { return instance; }
        }
        
        private FigureButtonStates _figureButtonState;
        
        public FigureButtonStates FigureButtonState
        {
            get { return _figureButtonState; }
            set
            {
                _figureButtonState = value;
                OnPropertyChanged("FigureButtonState");
            }
        }

        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
