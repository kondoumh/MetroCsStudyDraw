using System;
using Windows.UI.Xaml.Data;

namespace MetroCsStudyDraw.Utility
{
    class EnumConverter : IValueConverter
    {
        #region IValueConverter メンバー

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueString = value == null ? "????" : value.ToString();

            if (targetType == typeof(string))
                return valueString;

            return string.Compare(valueString, (string)parameter, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            targetType = typeof(FigureButtonStates);
            if (value.GetType() == typeof(string))
                return Enum.Parse(targetType, (string)value, true);

            if (value.GetType() == typeof(bool))
                return Enum.Parse(targetType, (string)parameter, true);
            if ((Boolean)value)
                return Enum.Parse(targetType, (string)parameter, true);
            return null;
        }

        #endregion
    }
}
