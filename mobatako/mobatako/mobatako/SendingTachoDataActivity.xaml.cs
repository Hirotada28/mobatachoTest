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
    //タコデータ送信スプラッシュ画面処理
    public partial class SendingTachoDataActivity : ContentPage
    {
        //API処理用
        OperationProcessor operationProcessor = new OperationProcessor();

        //初期処理
        public SendingTachoDataActivity()
        {
            InitializeComponent();
        }


        protected override void OnAppearing() 
        {
            //API実行
            AsyncMethod_SendingTachoDataActivity();
        }


        //API実行処理
        async Task AsyncMethod_SendingTachoDataActivity()
        {
            //乗換降車or乗務終了API実行結果フラグ
            var PostResult = false;

            //乗換ﾌﾗｸﾞによって実行APIを切り替える
            if (TransitInformation.isTransit)
            {
                //乗換API実行メソッドﾞ呼び出し ※CommonDate.StartAt：乗換画面での作業状態変更API実行時の日時を設定している
                GetOffVehicleBTransitRequest bodyInfo_Transit = new GetOffVehicleBTransitRequest();
                bodyInfo_Transit.GetoffAt = CommonDate.StartAt;
                bodyInfo_Transit.StartRidingAt = UserPreference.crewingInfo.RidingStartAt;
                bodyInfo_Transit.LeavingAt = CommonDate.StartAt;    
                bodyInfo_Transit.LeavingDeviceId = UserPreference.crewingInfo.LeavingDeviceId;
                bodyInfo_Transit.LeavingVehicleSeq = UserPreference.crewingInfo.LeavingVehicleSeq;
                bodyInfo_Transit.IsReturning = false;
                bodyInfo_Transit.RideAt = CommonDate.StartAt;   
                bodyInfo_Transit.GpsLatitude = 0.0;
                bodyInfo_Transit.GpsLongitude = 0.0;
                bodyInfo_Transit.GpsAzimuth = 0;
                bodyInfo_Transit.GpsAccuracy = 0.0;
                bodyInfo_Transit.GpsSatelliteCount = 0;
                bodyInfo_Transit.GpsSpeed = 0.0;
                bodyInfo_Transit.CrewingStartAt = UserPreference.crewingInfo.CrewingStartAt;
                bodyInfo_Transit.CrewingStartDeviceID = UserPreference.crewingInfo.CrewingStartDeviceId;
                bodyInfo_Transit.CrewingStartDeviceSeqNo = UserPreference.crewingInfo.CrewingStartDeviceSeq;
                string body_Transit = JsonConvert.SerializeObject(bodyInfo_Transit);
                PostResult = await operationProcessor.PostGetOffVehicleBTransit(body_Transit);
            }
            else
            {
                //乗務終了API実行メソッド呼び出し		
                EndCrewingRequest bodyInfo_EndCrewing = new EndCrewingRequest();
                bodyInfo_EndCrewing.EndCrewingAt = DateTime.Now.ToString();
                bodyInfo_EndCrewing.StartCrewingAt = UserPreference.crewingInfo.RidingStartAt;
                bodyInfo_EndCrewing.LeavingAt = UserPreference.crewingInfo.LeavingAt;
                bodyInfo_EndCrewing.LeavingDeviceId = UserPreference.crewingInfo.LeavingDeviceId;
                bodyInfo_EndCrewing.LeavingVehicleSeq = UserPreference.crewingInfo.LeavingVehicleSeq;
                bodyInfo_EndCrewing.CrewingAt = UserPreference.crewingInfo.CrewingStartAt;
                bodyInfo_EndCrewing.StartRidingAt = UserPreference.crewingInfo.RidingStartAt;
                bodyInfo_EndCrewing.GpsLatitude = 0.0;
                bodyInfo_EndCrewing.GpsLongitude = 0.0;
                bodyInfo_EndCrewing.GpsAccuracy = 0.0;
                bodyInfo_EndCrewing.GpsAzimuth = 0;
                bodyInfo_EndCrewing.GpsSatelliteCount = 0;
                bodyInfo_EndCrewing.GpsSpeed = 0.0;
                bodyInfo_EndCrewing.IsRest = false;
                string body_EndCrewing = JsonConvert.SerializeObject(bodyInfo_EndCrewing);
                PostResult = await operationProcessor.PostEndCrewing(body_EndCrewing);
            }

            //API実行に失敗した場合
            if(PostResult== false)
            {
                //エラー表示して乗務終了確認画面に戻る
                await DisplayAlert(Strings.error_end_crewing, Strings.error_end_crewing_description, Strings.permission_ok);
                Navigation.PushAsync(new EndCrewingConfirmActivity());
                return;
            }


            //乗換フラグによって遷移画面を切り替える
            if (TransitInformation.isTransit)
            {
                //乗換の場合、車両選択画面へ遷移
                Navigation.PushAsync(new ChoosingVehicleActivity());
                return;
            }
            else 
            {
                //乗務終了の場合、API実行に成功したらホーム(未乗車)画面へ遷移
                Navigation.PushAsync(new AfterCrewingActivity());
                return;
            }
                
        }

    }
}

