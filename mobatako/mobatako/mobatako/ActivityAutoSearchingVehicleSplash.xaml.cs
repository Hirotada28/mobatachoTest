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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobatako.utils;
using mobatako.utils.network;
using mobatako.common;

namespace mobatako
{
    //車両検索スプラッシュ画面処理
    public partial class ActivityAutoSearchingVehicleSplash : ContentPage
    {
        //初期処理
        public ActivityAutoSearchingVehicleSplash()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //内部保持チェック
            await InternalCheak();

            //車両選択画面に遷移
            await Navigation.PushAsync(new ChoosingVehicleActivity());
            return;
        }


        //内部保持チェック実行
        async Task<bool> InternalCheak()
        {
            //ローカル保持からユーザ,企業情報取得できない場合、エラー表示しアプリ終了
            if (UserPreference.userInfo.user.userSeq == null || UserPreference.userInfo.company.companySeq == null)
            {
                await DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
                var clear = new MobatachoCmmnFunc();
                clear.clearUserPreference();
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                return false;
            }
            return true;
        }

    }
}

