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
	//乗換確認画面処理
    public partial class TransitRidingVehicleConfirmActivity : ContentPage
    {
		//API処理用
		RestProcessor restProcessor = new RestProcessor();
		LoginRestProcessor loginRestProcessor = new LoginRestProcessor();
		CompanyProcessor companyProcessor = new CompanyProcessor();
		OperationProcessor operationProcessor = new OperationProcessor();

		//内部保持用
		UserPreference userPreference = new UserPreference();


		//初期処理
		public TransitRidingVehicleConfirmActivity()
        {
            InitializeComponent();
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


		//「乗換する」ボタン押下時の処理
		void TransistRidingButton(System.Object sender, System.EventArgs e)
		{
            Preferences.Set("userPreferences.isStartCrewing", true);
            //API処理
            AsyncMethod_TransitRidingVehicleConfirmActivity();
		}


		//「キャンセル」ボタン押下時の処理
		void TransistRidingCancelButton(System.Object sender, System.EventArgs e)
		{
			//車両選択画面に戻る
			Navigation.PushAsync(new ChoosingVehicleActivity());
		}


		//API実行
		async Task AsyncMethod_TransitRidingVehicleConfirmActivity()
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

			//乗換乗車API ※CommonDate.StartAt：乗換画面での作業状態変更API実行時の日時を設定している
			RideToVehicleByTransitRequest bodyInfo = new RideToVehicleByTransitRequest();
			if (MobatachoCmmnMbr.isFromSplash) { CommonDate.StartAt = DateTime.Now.ToString(); }
			bodyInfo.RideAt = CommonDate.StartAt;
			bodyInfo.GpsLatitude = 0.0;
			bodyInfo.GpsLongitude = 0.0;
			bodyInfo.GpsAzimuth = 0;
			bodyInfo.GpsAccuracy = 0.0;
			bodyInfo.GpsSatelliteCount = 0;
			bodyInfo.GpsSpeed = 0.0;
			bodyInfo.CrewingStartAt = UserPreference.crewingInfo.CrewingStartAt;
			bodyInfo.CrewingStartDeviceID = UserPreference.crewingInfo.CrewingStartDeviceId;
			bodyInfo.CrewingStartDeviceSeqNo = UserPreference.crewingInfo.CrewingStartDeviceSeq;
			string body = JsonConvert.SerializeObject(bodyInfo);
			var PostStartCrewingResult = await operationProcessor.PostRideToVehicleByTransist(body);
			if (PostStartCrewingResult)
			{
                Preferences.Set("userPreferences.isSelectVehicle", false);
                Preferences.Set("userPreferences.isStartCrewing", false);
                //乗換乗車API実行に成功した場合、ホーム画面に遷移
                userPreference.setCrewingInformation_StartCrewingOrTransitRiding();
				//TransitInformation.isTransit = false;
				Navigation.PushAsync(new Home());
			}
			else
			{
				//乗換乗車API実行に失敗した場合、内部エラーを表示
				TransitInformation.isTransit = false;
				DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
			}
		}
	}
}