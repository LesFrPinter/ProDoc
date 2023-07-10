using System.Windows;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
    public partial class YellowLabel : UserControl
    {
        public string BoxTitle
        {
            get { return (string)GetValue(BoxTitleProperty); }
            set { SetValue(BoxTitleProperty, value); }
        }

        public static readonly DependencyProperty BoxTitleProperty =
            DependencyProperty.Register("BoxTitle", typeof(string), typeof(YellowLabel), new UIPropertyMetadata(string.Empty));

        public YellowLabel()
        {
            InitializeComponent();
        }
    }
}