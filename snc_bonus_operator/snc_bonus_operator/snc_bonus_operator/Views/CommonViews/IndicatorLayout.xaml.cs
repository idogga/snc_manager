using System;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class Indicator : Frame
    {
        public void Start()
        {
            this.IsVisible = true;
            indicatorActivity.IsRunning = true;
        }

        public void Stop()
        {
            this.IsVisible = false;
            indicatorActivity.IsRunning = false;
        }

        public Indicator()
        {
            try
            {
                InitializeComponent();
                this.BackgroundColor = (Color)App.Current.Resources["BackgroundColor"];
                this.OutlineColor = (Color)App.Current.Resources["MainColor"];
                this.IsVisible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

        }
    }
}