using System.Windows;
using System.Windows.Controls;

namespace ProDocEstimate.Views
{
    public partial class DetailFeatures : UserControl
    {
        //public float FlatCharge
        //{
        //    get { return (float)GetValue(FlatChargeProperty); }
        //    set { SetValue(FlatChargeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for FlatCharge.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty FlatChargeProperty =
        //    DependencyProperty.Register("FlatCharge", typeof(float), typeof(DetailFeatures), 
        //        new FrameworkPropertyMetadata(
        //            0.00, 
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DetailFeatures() 
        { InitializeComponent(); 
          //FlatCharge = 123.45F; 
        }

    }
}
