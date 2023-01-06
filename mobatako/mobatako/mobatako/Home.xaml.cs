using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Net;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobatako.utils;
using mobatako.utils.network;
using mobatako.common;
using mobatako.model;

namespace mobatako
{
    //ホーム画面処理
    public partial class Home : ContentPage
    {
        //API処理用
        OperationProcessor operationProcessor = new OperationProcessor();

        //作業状態保持
        WorkStates selectedWork = null;


        //初期処理
        public Home()
        {
            InitializeComponent();

            TransitInformation.isTransit = false;

            //ユーザ,選択車両名,作業状態をローカル保持から取得し表示
            loginUserNameView.Text = UserPreference.userInfo.user.userName;
            vehicleNameView.Text = UserPreference.vehicleInfo.vehicleName;

            //デザイン構築
            Thickness padding;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    padding = new Thickness(0, 0, 0, 60);
                    break;
                default:
                    padding = new Thickness();
                    break;
            }
            Padding = padding;

            //作業状態ボタンリスト作成
            CreateWorkStateBtns();

            //起動スプラッシュから直接遷移してきた場合
            if (MobatachoCmmnMbr.isFromSplash)
            {
                //最終作業状態をセット
                workStateView.Text = UserPreference.userInfo.lastWorkState.startWorking;
                MobatachoCmmnMbr.isFromSplash = false;
            }
            else if(TransitInformation.isTransit || MobatachoCmmnMbr.isFromInputRefuel)
            {
                //乗換作業をセット
                workStateView.Text = MobatachoCmmnMbr.tmpWorkName;
                MobatachoCmmnMbr.isFromInputRefuel = false;
            }
            //乗務開始確認から遷移してきた場合
            else
            {
                //作業状態を乗務連動する作業に変更し、作業状態変更API実行
                for (int i = 0; i < CompanyProcessor.resWorkStatesSize; i++)
                {
                    if (CompanyProcessor.resWorkStatesObj[i].isLinkCrewingStart)
                    {
                        workStateView.Text = CompanyProcessor.resWorkStatesObj[i].workStateName;
                        AsyncMethod_Home(true, i);
                        break;
                    }
                }
            }

        }


        protected override void OnAppearing()
        {
            //必要情報(ユーザ,車両)がない場合は、エラー表示しログイン画面に戻る
            if (UserPreference.userInfo.user == null || UserPreference.vehicleInfo == null)
            {
                //内部処理失敗エラー
                DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
            }
        }


        //「車両乗換」ボタン押下時の処理
        void TransitVehicleButton(System.Object sender, System.EventArgs e)
        {
            //乗換画面に遷移
            Navigation.PushModalAsync(new NavigationPage(new TransitVehicleChooseWorkDialog()));
        }


        //各作業内容ボタン押下時の処理
        void WorkStateButton(System.Object sender, System.EventArgs e)
        {

            //作業状態変更API実行
            Button btn = (Button)sender;
            int id = int.Parse(btn.ClassId);
            //WorkStates selectedWork = CompanyProcessor.resWorkStatesObj[id];
            selectedWork = CompanyProcessor.resWorkStatesObj[id];
            AsyncMethod_Home(false, 0);

            //選択した作業状態名を取得し、状態ラベルにセットする
            workStateView.Text = selectedWork.workStateName;
            MobatachoCmmnMbr.tmpWorkName = selectedWork.workStateName;
            if (selectedWork.isShowRefuelInput)
            {
                MobatachoCmmnMbr.isFromHome = true;
                Navigation.PushModalAsync(new NavigationPage(new RefuelStatioChooseDialog()));
            }
        }


        //「乗務を終了する」ボタン押下時の処理
        void CrewingEndButton(System.Object sender, System.EventArgs e)
        {
            //乗務終了確認画面へ遷移
            Navigation.PushAsync(new EndCrewingConfirmActivity());
        }


        //作業状態ボタンリスト作成
        void CreateWorkStateBtns()
        {
            if (CompanyProcessor.resWorkStatesObj.Count() == 0) return;
            for (int i = 0; i < CompanyProcessor.resWorkStatesObj.Count(); i++)
            {
                Button btn = new Button
                {
                    Text = CompanyProcessor.resWorkStatesObj[i].workStateName,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 250,
                    TextColor = Color.White,
                    Background = Color.FromRgb(51, 153, 255),
                    ClassId = i.ToString(),
                    IsVisible = !CompanyProcessor.resWorkStatesObj[i].isButtonHidden,
                };
                btn.Clicked += WorkStateButton;
                workStateBtns.Children.Add(btn);
            }
        }


        //API実行
        async Task AsyncMethod_Home(bool isDef, int index)
        {
            //作業状態
            ChangeWorkStateRequest bodyInfo = new ChangeWorkStateRequest();


            if (isDef)
            {
                bodyInfo.StartWorkingStateSeq = CompanyProcessor.resWorkStatesObj[index].workStateSeq;
                bodyInfo.StartWorkingStateLogNo = CompanyProcessor.resWorkStatesObj[index].workStateLogNo;
                bodyInfo.StartWorkingAt = UserPreference.crewingInfo.CrewingStartAt;
                bodyInfo.PreviousStartWorkingAt = UserPreference.crewingInfo.CrewingStartAt;
                bodyInfo.PreviousStartWorkingDeviceId = UserPreference.crewingInfo.CrewingStartDeviceId;
            }
            else
            {
                CommonDate.StartAt = DateTime.Now.ToString();
                bodyInfo.StartWorkingStateSeq = selectedWork.workStateSeq;
                bodyInfo.StartWorkingStateLogNo = selectedWork.workStateLogNo;
                bodyInfo.StartWorkingAt = CommonDate.StartAt;
                bodyInfo.PreviousStartWorkingAt = PreviousCrewingInformation.PreviousStartWorkingAt;
                bodyInfo.PreviousStartWorkingDeviceId = PreviousCrewingInformation.PreviousStartWorkingDeviceId;
            }
            bodyInfo.GpsLatitude = 0.0;
            bodyInfo.GpsLongitude = 0.0;
            bodyInfo.GpsAzimuth = 0;
            bodyInfo.GpsAccuracy = 0.0;
            bodyInfo.GpsSatelliteCount = 0;
            bodyInfo.GpsSpeed = 0.0;
            bodyInfo.LoadEmpState = LoadEmpType.UNKNOWN;
            bodyInfo.CrewingStartDeviceId = UserPreference.crewingInfo.CrewingStartDeviceId;
            bodyInfo.CrewingStartAt = UserPreference.crewingInfo.CrewingStartAt;
            bodyInfo.RideStartAt = UserPreference.crewingInfo.RidingStartAt;
            bodyInfo.RideStartDeviceId = UserPreference.crewingInfo.RideStartDeviceId;
            bodyInfo.RideStartDeviceSeqNo = UserPreference.crewingInfo.RideStartDeviceSeq;

            string body = JsonConvert.SerializeObject(bodyInfo);
            var ChangeWorkStateResult = await operationProcessor.PostChangeWorkState(body);
            if (ChangeWorkStateResult)
            {
                //次回用に前回作業変更情報を更新
                PreviousCrewingInformation.PreviousStartWorkingAt = bodyInfo.StartWorkingAt;
                PreviousCrewingInformation.PreviousStartWorkingDeviceId = UserPreference.crewingInfo.CrewingStartDeviceId;
            }
            else 
            {
                await DisplayAlert("作業状態変更エラー", "", "OK");
                return;
            }

            return;
        }


        //停車自動判定処理(※GPS追跡は一時保留)
        //async void AutoJudgeStopVehicleButton(System.Object sender, System.EventArgs e)
        //{
        //    await TrackingLocation();
        //    //GPS追跡は一時保留
        //    //Navigation.PushModalAsync(new NavigationPage(new ID0202_stop_vehicle_choose_work_dialog()));
        //}
        //async Task TrackingLocation()
        //{
        //    Console.WriteLine(CrossGeolocator.Current.IsListening);
        //    if (CrossGeolocator.Current.IsListening)
        //    {
        //        return;
        //    }
        //    await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(2), 10);
        //    CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        //}
        //private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        //{
        //    location.Text += $"【{e.Position.Accuracy}】,【{e.Position.Latitude}】, 【{e.Position.Longitude}】, 【{e.Position.Timestamp.TimeOfDay}】,【{e.Position.Speed}】,【{e.Position.Altitude}】,【{e.Position.AltitudeAccuracy}】,【{e.Position.Heading}】";
        //    Console.WriteLine($"【{e.Position.Latitude}】, 【{e.Position.Longitude}】, 【{e.Position.Timestamp.TimeOfDay}】");
        //}
        //async Task StopTrackingLocation()
        //{
        //    if (!CrossGeolocator.Current.IsListening) return;
        //    await CrossGeolocator.Current.StopListeningAsync();
        //    CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
        //}

    }
}



