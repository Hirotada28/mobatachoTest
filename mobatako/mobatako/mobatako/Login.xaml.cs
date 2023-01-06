using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using mobatako.common;

namespace mobatako
{
	//ログイン画面処理
	public partial class Login : ContentPage
	{
		//初期処理
		public Login()
		{
			InitializeComponent();

            //内部保持からログイン情報セット
            if (Preferences.Get("userPreferences.usercode", "")==""|| Preferences.Get("userPreferences.companycode", "")=="") 
			{
				userid.Text = "";
			}
			else
            {
				userid.Text = Preferences.Get("userPreferences.usercode", "") + '@' + Preferences.Get("userPreferences.companycode", "");
			}
			userPassword.Text = Preferences.Get("userPreferences.userPassword", "");
			checkViewPassword.IsChecked = Preferences.Get("userPreferences.checkViewPassword", false);
			checkSavingUserInfo.IsChecked = Preferences.Get("userPreferences.checkSavingUserInfo", false);
		}

		
		//「ログインする」ボタン押下処理
		private void Btn_Login(object sender, EventArgs e)
		{
			//ユーザID分解
			string[] SplitUserId = userid.Text.Split('@');
			if(SplitUserId.Length != 2)
            {
				DisplayAlert(Strings.error_login_fail, Strings.error_login_without_company_code, Strings.permission_ok);
				return;
            }

			//入力情報をローカル保持し、起動スプラッシュ画面へ戻る
			if (checkSavingUserInfo.IsChecked == false)
			{
				var clear = new MobatachoCmmnFunc();
				clear.clearUserPreference();
			}
			else
			{
				Preferences.Set("userPreferences.usercode", SplitUserId[0]);
				Preferences.Set("userPreferences.companycode", SplitUserId[1]);
				Preferences.Set("userPreferences.userPassword", userPassword.Text);
				Preferences.Set("userPreferences.checkViewPassword", checkViewPassword.IsChecked);
				Preferences.Set("userPreferences.checkSavingUserInfo", checkSavingUserInfo.IsChecked);
				Preferences.Set("userPreferences.deviceid", "5224133d61af565e");
				Preferences.Set("userPreferences.devicelogno", "1");            
			}
			Navigation.PopAsync();
		}


        //パスワード表示/非表示チェックボックス押下時の処理
        private void Btn_CheckViewPassword(object sender, EventArgs e)
		{
			if (checkViewPassword.IsChecked == false)
			{
				userPassword.IsPassword = true;
			}
			else
			{
				userPassword.IsPassword = false;
			}
		}
	}
}

