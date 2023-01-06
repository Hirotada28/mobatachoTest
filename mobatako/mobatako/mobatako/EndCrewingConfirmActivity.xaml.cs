using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobatako.utils;
using mobatako.utils.network;
using mobatako.common;

namespace mobatako
{
    //乗務終了確認画面処理
    public partial class EndCrewingConfirmActivity : ContentPage
    {
        //初期処理
        public EndCrewingConfirmActivity()
        {
            InitializeComponent();

            //選択車両名をローカル保持から取得し表示
            endCrewingVehicleNameLabel.Text = UserPreference.vehicleInfo.vehicleName;
        }


        //「乗務終了する」ボタン押下時の処理
        void EndCrewingButton(System.Object sender, System.EventArgs e)
        {
            //乗務終了確認ダイアログ表示
            Navigation.PushModalAsync(new NavigationPage(new EndCrewingConfirmDialog()));
        }


        //「キャンセル」ボタン押下時の処理
        void EndCrewingCancalButton(System.Object sender, System.EventArgs e)
        {
            //ホーム画面に遷移
            Navigation.PopAsync();
        }
    }
}

