using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using mobatako.utils;
using mobatako.utils.network;
using mobatako.common;
using mobatako.model;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;

namespace mobatako
{
    //乗換画面処理
    public partial class TransitVehicleChooseWorkDialog : ContentPage
    {
        //API処理用
        RestProcessor restProcessor = new RestProcessor();
        LoginRestProcessor loginRestProcessor = new LoginRestProcessor();
        CompanyProcessor companyProcessor = new CompanyProcessor();
        OperationProcessor operationProcessor = new OperationProcessor();


        //初期処理
        public TransitVehicleChooseWorkDialog()
        {
            InitializeComponent();

            //iOS用のレイアウト調整
            Thickness padding;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    padding = new Thickness(0, 30, 0, 60);
                    break;
                default:
                    padding = new Thickness();
                    break;
            }
            Padding = padding;

            //作業状態ボタンリスト作成
            CreateWorkStateBtns();
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
                clear.clearRidingVehicleInformation();
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

        //「キャンセル」ボタン押下時の処理
        void TransitCancelButton(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }


        //作業状態ボタン押下時の処理
        void WorkStateBtnClicked(System.Object sender, System.EventArgs e)
        {
            MobatachoCmmnMbr.isSelectRefuel = false;
            //作業状態変更API
            Button btn = (Button)sender;
            int id = int.Parse(btn.ClassId);
            MobatachoCmmnMbr.tmpWorkName = btn.Text;
            WorkStates selectedWork = CompanyProcessor.resWorkStatesObj[id];

            //給油入力する作業状態を選択した場合
            if (selectedWork.isShowRefuelInput)
            {
                MobatachoCmmnMbr.isSelectRefuel = true;
                //給油所選択画面へ遷移
                Navigation.PushModalAsync(new NavigationPage(new RefuelStatioChooseDialog()));
            }
            AsyncMethod_TransitVehicleChooseWork(selectedWork);
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
                    IsVisible = !CompanyProcessor.resWorkStatesObj[i].isButtonHidden 
                };
                btn.Clicked += WorkStateBtnClicked;
                workStateBtns.Children.Add(btn);
            }

        }


        //API実行(作業状態変更API)
        async Task AsyncMethod_TransitVehicleChooseWork(WorkStates selectedWork)
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

            //作業状態変更API
            ChangeWorkStateRequest bodyInfo = new ChangeWorkStateRequest();
            CommonDate.StartAt = DateTime.Now.ToString();
            bodyInfo.StartWorkingStateSeq = selectedWork.workStateSeq;
            bodyInfo.StartWorkingStateLogNo = selectedWork.workStateLogNo;
            bodyInfo.PreviousStartWorkingAt = "";
            bodyInfo.PreviousStartWorkingDeviceId = "";
            bodyInfo.StartWorkingAt = CommonDate.StartAt;
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
                if(MobatachoCmmnMbr.isSelectRefuel == false)
                {
                    //作業状態変更API実行に成功した場合、タコデータ送信画面に遷移
                    TransitInformation.isTransit = true;
                    await Navigation.PushAsync(new SendingTachoDataActivity());
                }
            }
            else
            {
                //失敗した場合、内部エラーを表示
                DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
            }
        }
    }
}

