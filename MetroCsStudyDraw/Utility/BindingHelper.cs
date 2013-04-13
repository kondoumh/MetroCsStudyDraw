using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MetroCsStudyDraw.Utility
{
    class BindingHelper
    {
        public static void BindProperty(Control control, object source, string path,
            DependencyProperty property, BindingMode mode)
        {
            var binding = new Binding();
            binding.Path = new PropertyPath(path);
            binding.Source = source;
            binding.Mode = mode;
            control.SetBinding(property, binding);
        }

        public static void BindProperty(FrameworkElement element, object source,
            string path, DependencyProperty property, BindingMode mode)
        {
            var binding = new Binding();
            binding.Path = new PropertyPath(path);
            binding.Source = source;
            binding.Mode = mode;
            element.SetBinding(property, binding);
        }
    }
}
