using System;
using System.Collections.Generic;
using Xamarin.Forms;
using mobatako.common;
using mobatako.utils;

namespace mobatako
{
    //ホーム(未乗車)画面処理
    public partial class AfterCrewingActivity : ContentPage
    {
        //初期処理
        public AfterCrewingActivity()
        {
            InitializeComponent();

            //ユーザ名表示
            loginUserNameViewOfAfterCrewing.Text = UserPreference.userInfo.user.userName;
        }


        //「ログアウト」ボタン押下時の処理
        void LogoutButton(System.Object sender, System.EventArgs e)
        {
            var clear = new MobatachoCmmnFunc();
            clear.clearUserPreference();

            //起動スプラッシュ画面へ遷移
            Navigation.PushAsync(new Splash());
        }


        //「車両を選択する」ボタン押下時の処理
        void ChooseVehicleButton(System.Object sender, System.EventArgs e)
        {
            //車両検索スプラッシュ画面へ遷移
            Navigation.PushAsync(new ActivityAutoSearchingVehicleSplash());
        }
    }
}

