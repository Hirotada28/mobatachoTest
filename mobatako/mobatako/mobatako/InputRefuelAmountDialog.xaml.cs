using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using mobatako.common;
using Xamarin.Essentials;
using mobatako.utils.network;
using Newtonsoft.Json;
using mobatako.model;

namespace mobatako
{
    //給油入力画面処理
    public partial class InputRefuelAmountDialog : ContentPage
    {
        //API処理用
        RestProcessor restProcessor = new RestProcessor();
        CompanyProcessor companyProcessor = new CompanyProcessor();
        OperationProcessor operationProcessor = new OperationProcessor();
        RefuelConfiguration refuelInfo;

        //給油量,料金用
        public double InputRefuelAmount { get; set; }
        public double maxAmount = 9999.9;
        public long InputRefuelFee { get; set; }
        public long maxFee = 99999999;


        //初期処理
        public InputRefuelAmountDialog(RefuelConfiguration refuelConfiguration)
        {
            InitializeComponent();
            refuelInfo = refuelConfiguration;
            fee.IsVisible = refuelConfiguration.NeedInputRefuelFee;
            inputFee.IsVisible = refuelConfiguration.NeedInputRefuelFee;
            SelectedRefuelType.Text = refuelConfiguration.RefuelStationName;
            Thickness padding;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    padding = new Thickness(0, 30, 0, 0);
                    break;
                default:
                    padding = new Thickness();
                    break;
            }
            Padding = padding;
        }


        //「送信」ボタン押下時の処理
        void SendRefuelingInfoBtn(System.Object sender, System.EventArgs e)
        {
            CheckInputValue();
            AsyncMethod_SendRefuelInfo();
        }

        void CloseBtn(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
            //if (MobatachoCmmnMbr.isFromHome)
            //{
            //    MobatachoCmmnMbr.isFromHome = false;
            //    Navigation.PushAsync(new Home());
            //}
            //else
            //{
            //    Navigation.PushAsync(new TransitVehicleChooseWorkDialog());
            //}
        }

        //API実行
        async Task AsyncMethod_SendRefuelInfo()
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

            //給油情報送信API
            RefuelInfoRequest bodyInfo = new RefuelInfoRequest();
            bodyInfo.RefuelAt = DateTime.Now.ToString();
            bodyInfo.RefuelStationSeq = refuelInfo.RefuelStationSeq;
            bodyInfo.RefuelFee = InputRefuelFee;
            bodyInfo.RefuelAmount = InputRefuelAmount;
            string body = JsonConvert.SerializeObject(bodyInfo);
            var PostRefuelInfoResult = await operationProcessor.PostRefuelInfo(body);
            if (PostRefuelInfoResult)
            {
                if (MobatachoCmmnMbr.isFromHome)
                {
                    MobatachoCmmnMbr.isFromHome = false;
                    MobatachoCmmnMbr.isFromInputRefuel = true;
                    //Navigation.PopModalAsync();
                    //Navigation.PopModalAsync();
                    Navigation.PushAsync(new Home());
                }
                else
                {
                    //遷移元が乗換画面の時はたこデータ送信画面に遷移
                    TransitInformation.isTransit = true;
                    Navigation.PushAsync(new SendingTachoDataActivity());
                }
            }
            else
            {
                //乗務開始API実行に失敗した場合、内部ｴﾗｰを表示
                DisplayAlert(Strings.error_internal, Strings.error_internal_description, Strings.permission_ok);
            }

        }

        /// <summary>
        /// 入力された値を上限をチェックする
        /// </summary>
        void CheckInputValue()
        {
            string amount = refuelAmount.Text;
            string fee = refuelFee.Text;
            if (amount == null || fee == null)
            {
                return;
            }
            InputRefuelAmount = (double.Parse(amount) < maxAmount) ? double.Parse(amount) : maxAmount;
            InputRefuelFee = (long.Parse(fee) < maxFee) ? long.Parse(fee) : maxFee;
        }
    }
}

