using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ObsControlMobile.Views
{
    public class AspectRatioContainer: ContentView
    {
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
        }
        public static BindableProperty AspectRatioProperty = BindableProperty.Create(nameof(AspectRatio), typeof(double), typeof(AspectRatioContainer), (double)1);
        
        public double AspectRatio
        {
            get { return (double)this.GetValue(AspectRatioProperty); }
            set
            {
                this.SetValue(AspectRatioProperty, value);
            }
        }
        
    }
}
