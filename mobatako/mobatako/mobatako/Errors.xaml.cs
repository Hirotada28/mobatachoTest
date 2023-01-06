using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mobatako
{
    public partial class Errors : ContentPage
    {
        public Errors()
        {
            InitializeComponent();
        }

        void ServiceUnavailableError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void LoginError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void GateAPIResponseError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void NetworkError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void TachoResponseError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void GpsResponseError(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "エラーが発生しました。", "承認", "キャンセル");
        }

        void ForgetStartCrewing(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "乗務開始忘れ", "キャンセル");
        }

        void ForgetEndCrewing(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("警告", "乗務終了忘れ", "キャンセル");
        }

        void Back(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}

