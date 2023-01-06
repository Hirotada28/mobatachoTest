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
using mobatako.model;

namespace mobatako
{
	//乗務開始確認画面処理
	public partial class StartCrewingConfirmActivity : ContentPage
	{
		//API処理用
		RestProcessor restProcessor = new RestProcessor();
		LoginRestProcessor loginRestProcessor = new LoginRestProcessor();
		CompanyProcessor companyProcessor = new CompanyProcessor();
		OperationProcessor operationProcessor = new OperationProcessor();

		//内部保持用
		UserPreference userPreference = new UserPreference();


		//初期処理
		public StartCrewingConfirmActivity()
		{
			InitializeComponent();

			//選択車両名をローカル保持から取得し表示
			choosedVehcle.Text = UserPreference.vehicleInfo.vehicleName;
		}


		protected override void OnAppearing()
		{
			//乗車車両情報がローカル保持されていない場合は異常終了
			if (UserPreference.vehicleInfo == null)
			{
				DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);

				//現在の内部保持をクリアしアプリ終了
				var clear = new MobatachoCmmnFunc();
				clear.clearUserPreference();
				System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
			}
		}


		//「乗務開始する」ボタン押下時の処理
		void StartCrewingButton(System.Object sender, System.EventArgs e)
		{
			Preferences.Set("userPreferences.isStartCrewing", true);
          
            //API処理
            AsyncMethod_StartCrewingConfirmActivity();
		}


		//「キャンセル」ボタン押下時の処理
		void StartCrewingCancelButton(System.Object sender, System.EventArgs e)
		{
			//車両選択画面に戻る
			Navigation.PushAsync(new ChoosingVehicleActivity());
		}


		//API実行
		async Task AsyncMethod_StartCrewingConfirmActivity()
		{
			//企業情報取得
			var getCompanyConfiguationsResult = await companyProcessor.GetCompanyConfiguations();

			//サーバー疎通確認
			var serverIsAliveResult = await restProcessor.ServerIsAlive();
			if (serverIsAliveResult == false) 
			{
				//サーバー疎通不可の場合はアプリ終了
				await DisplayAlert(Strings.error_connection_fail, Strings.error_connection_fail_description, Strings.permission_ok);
				System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
				return;
			}

			//乗務開始API
			StartCrewingRequest bodyInfo = new StartCrewingRequest();
			CommonDate.StartAt = DateTime.Now.ToString();
			bodyInfo.CrewingAt = CommonDate.StartAt;
			bodyInfo.GpsAccuracy = 0.0;                             
			bodyInfo.GpsAzimuth = 0;
			bodyInfo.GpsLatitude = 0.0;
			bodyInfo.GpsLongitude = 0.0;
			bodyInfo.GpsSatelliteCount = 0;
			bodyInfo.GpsSpeed = 0.0;
			bodyInfo.StartRidingAt = CommonDate.StartAt;
			string body = JsonConvert.SerializeObject(bodyInfo);
			var PostStartCrewingResult = await operationProcessor.PostStartCrewing(body);
            if (PostStartCrewingResult)
			{
                Preferences.Set("userPreferences.isSelectVehicle", false);
                Preferences.Set("userPreferences.isStartCrewing", false);

				//乗務開始API実行に成功した場合、乗務情報を保持してホーム画面に遷移
				userPreference.setCrewingInformation_StartCrewingOrTransitRiding();
				Navigation.PushAsync(new Home());
			}
			else 
			{
				//乗務開始API実行に失敗した場合、内部エラーを表示
				DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
			}

		}

	}
}

