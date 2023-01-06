using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobatako
{
    public partial class MainPage : ContentPage
    {
        private string str { get; set; }
        public MainPage()
        {
            InitializeComponent();

            Navigation.PushAsync(new Splash());
            //NavigationPage.SetHasNavigationBar(this, false);
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }
    }
}

