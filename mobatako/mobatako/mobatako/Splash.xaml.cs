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
using mobatako.model;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.Extensions;

namespace mobatako
{
    //起動スプラッシュ画面処理
    public partial class Splash : ContentPage
    {
        //API処理用
        RestProcessor restProcessor = new RestProcessor();
        LoginRestProcessor loginRestProcessor = new LoginRestProcessor();
        CompanyProcessor companyProcessor = new CompanyProcessor();

        //内部保持用
        UserPreference userPreference = new UserPreference();


        //初期処理
        public Splash()
        {
            InitializeComponent();

            //【ToDo】テスト用なので削除：ローカル保持クリア
            var clear = new MobatachoCmmnFunc();
            clear.clearUserPreference();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //バージョン情報表示
            AppVersionShowing();

            //【ToDo】アップデートチェック

            //位置情報アクセス確認
            if (!await CheckPermissionLocationStatus()) System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            //乗車開始忘れ通知
            if ((Preferences.Get("userPreferences.isSelectVehicle", false) == false && Preferences.Get("userPreferences.isStartCrewing", false) == true)
                || (Preferences.Get("userPreferences.isSelectVehicle", false) == true && Preferences.Get("userPreferences.isStartCrewing", false) == false))
            {
                await ShowForgetStartCrewing();
            }

            //ログイン情報保持を確認し、保持されていなければログイン画面へ遷移
            if (Preferences.Get("userPreferences.usercode", "") == ""
                || Preferences.Get("userPreferences.userPassword", "") == "")
            {
                await Navigation.PushAsync(new Login());
                return;
            }

            //API処理
            AsyncMethod_Splash();
        }


        //API実行
        async Task AsyncMethod_Splash()
        {
            //サーバー疎通確認し、疎通不可の場合はエラー表示
            var serverIsAliveResult = await restProcessor.ServerIsAlive();
            if (serverIsAliveResult == false)
            {
                await DisplayAlert(Strings.error_connection_fail, Strings.error_connection_fail_description, Strings.permission_ok);
                return;
            }
            
            //ログイン要求
            loginInfo info = new loginInfo();
            info.userCode = Preferences.Get("userPreferences.usercode", "");
            info.companyCode = Preferences.Get("userPreferences.companycode", "");
            info.userPassword = Preferences.Get("userPreferences.userPassword", "");
            info.deviceId = Preferences.Get("userPreferences.deviceid", "");
            string body = JsonConvert.SerializeObject(info);
            var LoginResult = await loginRestProcessor.PostLogin(body);
            if (LoginResult == false)
            {
                //失敗したらログイン画面に遷移
                await DisplayAlert(Strings.error_login_fail, Strings.error_login_fail_description, Strings.permission_ok);
                await Navigation.PushAsync(new Login());
                return;
            }

            //ログインユーザとして保持
            userPreference.setLoginUserInformation();

            //ユーザ情報取得
            var GetUserInformationResult = await loginRestProcessor.GetUserInformation();
            if (GetUserInformationResult)
            {
                //ユーザ情報取得できた場合、乗務情報,乗車車両情報を保持
                userPreference.setCrewingInformation();

                //乗車中の場合、乗車車両情報,作業状態を保持しホーム画面へ遷移
                if (LoginRestProcessor.loggedInUserCrewingInfo.vehicleRideType == RIDE_TYPE.RIDE_ON)
                {
                    userPreference.setRidingVehicleInformation();
                    userPreference.setLoginUserInformation_WorkState();
                    var getCompanyConfiguationsResult = await companyProcessor.GetCompanyConfiguations();
                    MobatachoCmmnMbr.isFromSplash = true;
                    Navigation.PushAsync(new Home());
                    return;
                }
                //乗車中でなければ乗換と判断
                else 
                {
                    TransitInformation.isTransit = true;
                    userPreference.setLoginUserInformation_WorkState();
                    MobatachoCmmnMbr.isFromSplash = true;
                }
            }


            //車両検索スプラッシュ画面に遷移
            await Navigation.PushAsync(new ActivityAutoSearchingVehicleSplash());
            return;

        }


        //位置情報アクセス確認実行
        async Task<bool> CheckPermissionLocationStatus()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert(Strings.permission_announce_background_gps_title, Strings.permission_announce_background_gps, Strings.permission_ok);
                AppInfo.ShowSettingsUI();
                await Task.Delay(1000);
                return false;
            }
            return true;
        }


        //バージョン表示実行
        void AppVersionShowing()
        {
            version.Text = $"{VersionTracking.CurrentVersion}({VersionTracking.CurrentBuild})";
        }

        async Task ShowForgetStartCrewing()
        {
            var options = new SnackBarOptions()
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = Strings.remind_start_crewing
                },
                BackgroundColor = Color.FromHex("3399ff"),
                Duration = TimeSpan.FromSeconds(3)
            };
            await this.DisplayToastAsync(options);
        }
    }

}

