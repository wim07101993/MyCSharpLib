using System.Windows;
using System.Windows.Controls;

namespace MyCSharpLib.Wpf.Controls.ObjectBrowser
{
    public class ObjectBrowserDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string)
                return StringTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
