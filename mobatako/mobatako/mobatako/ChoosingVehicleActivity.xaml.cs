using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    //車両選択画面処理
    public partial class ChoosingVehicleActivity : ContentPage
    {
        //API処理用
        CompanyProcessor companyProcessor = new CompanyProcessor();

        //内部保持用
        UserPreference userPreference = new UserPreference();

        //車両一覧作成用                                              
        ObservableCollection<VehicleInformation> tmpVehicleList;

        //選択車両情報保持用
        public VehicleInformation selectVehicleInfo = new VehicleInformation();


        //初期処理
        public ChoosingVehicleActivity()
        {
            InitializeComponent();
        }


        protected override void OnAppearing() 
        {
            //API処理
            AsyncMethod_ChoosingVehicleActivity();
        }


        //API実行し車両一覧作成
        async Task AsyncMethod_ChoosingVehicleActivity()
        {
            //車両情報取得
            var getVehicleListResult = await companyProcessor.GetVehicleList();
            tmpVehicleList = new ObservableCollection<VehicleInformation>();
            if (getVehicleListResult)
            {
                //取得車両が1件以上の場合、一覧作成
                if (companyProcessor.resVehicleListSize > 0)
                {
                    for (int i = 0; i < companyProcessor.resVehicleListSize; i++) 
                    {
                        //乗務中の場合
                        if ((long)companyProcessor.resVehicleListObj[i]["rideType"] == RIDE_TYPE.RIDE_ON)
                        {
                            if (ResearchVehicleList(companyProcessor.resVehicleListObj[i]))
                            {
                                tmpVehicleList.Add(
                                    new VehicleInformation()
                                    {
                                        vehicleSeq = (long)companyProcessor.resVehicleListObj[i]["vehicleSeq"],
                                        logNo = (long)companyProcessor.resVehicleListObj[i]["logNo"],
                                        vehicleCode = (string)companyProcessor.resVehicleListObj[i]["vehicleCode"],
                                        vehicleName = (string)companyProcessor.resVehicleListObj[i]["vehicleName"],
                                        logitachoVehicleId = (string)companyProcessor.resVehicleListObj[i]["logitachoVehicleId"],
                                        bluetoothMac = (string)companyProcessor.resVehicleListObj[i]["bluetoothMac"],
                                        rideType = (long)companyProcessor.resVehicleListObj[i]["rideType"],
                                        leavingReturningType = (long)companyProcessor.resVehicleListObj[i]["leavingReturningType"],
                                        loadEmpType = (long)companyProcessor.resVehicleListObj[i]["loadEmpType"],
                                        isLogitachoMounting = (bool)companyProcessor.resVehicleListObj[i]["isLogitachoMounting"],
                                        logitachoSerialNo = (string)companyProcessor.resVehicleListObj[i]["logitachoSerialNo"],
                                        rideTypeLavel = " 乗務中 "
                                    }
                                );
                            }
                        }
                        //乗務中でない場合
                        else
                        {
                            if (ResearchVehicleList(companyProcessor.resVehicleListObj[i]))
                            {
                                tmpVehicleList.Add(
                                    new VehicleInformation()
                                    {
                                        vehicleSeq = (long)companyProcessor.resVehicleListObj[i]["vehicleSeq"],
                                        logNo = (long)companyProcessor.resVehicleListObj[i]["logNo"],
                                        vehicleCode = (string)companyProcessor.resVehicleListObj[i]["vehicleCode"],
                                        vehicleName = (string)companyProcessor.resVehicleListObj[i]["vehicleName"],
                                        logitachoVehicleId = (string)companyProcessor.resVehicleListObj[i]["logitachoVehicleId"],
                                        bluetoothMac = (string)companyProcessor.resVehicleListObj[i]["bluetoothMac"],
                                        rideType = (long)companyProcessor.resVehicleListObj[i]["rideType"],
                                        leavingReturningType = (long)companyProcessor.resVehicleListObj[i]["leavingReturningType"],
                                        loadEmpType = (long)companyProcessor.resVehicleListObj[i]["loadEmpType"],
                                        isLogitachoMounting = (bool)companyProcessor.resVehicleListObj[i]["isLogitachoMounting"],
                                        logitachoSerialNo = (string)companyProcessor.resVehicleListObj[i]["logitachoSerialNo"],
                                        rideTypeLavel = ""
                                    }
                                );
                            }
                        }
                    }
                    vehicleList.ItemsSource = tmpVehicleList;
                }
            }
        }


        //再検索
        private bool ResearchVehicleList(JToken item)
        {
            var name = ChooseVehicleCondition.prevVehicleName;
            var isGetOffOnly = ChooseVehicleCondition.prevIsChecked;
            if(name.Equals(""))
            {
                if (isGetOffOnly)
                {
                    if ((long)item["rideType"] != 1) return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (isGetOffOnly)
                {
                    if (item["vehicleName"].ToString().Contains(name) && (long)item["rideType"] != 1) return true;
                }
                else
                {
                    if (item["vehicleName"].ToString().Contains(name)) return true;
                }
            }
            return false;
        }


        //「表示条件」ボタン押下時処理
        void ConditionButton(System.Object sender, System.EventArgs e)
        {
            //条件変更画面へ遷移
            Navigation.PushModalAsync(new NavigationPage(new ChoosingVehicleConditionDialog()));
        }


        //「車両を再検索する」ボタン押下時処理
        void ResarchButton(System.Object sender, System.EventArgs e)
        {
            //車両一覧再作成(車両検索API)
            AsyncMethod_ChoosingVehicleActivity();
        }


        //車両一覧から車両選択時の処理
        void OnVehicleSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //ローカル保持を一度クリア
            var clear = new MobatachoCmmnFunc();
            clear.clearRidingVehicleInformation();

            //選択された車両情報が乗務中の場合、何もしない
            ListView listView = (ListView)sender;
            VehicleInformation selectVehicle = (VehicleInformation)listView.SelectedItem;
            if (selectVehicle.rideType == RIDE_TYPE.RIDE_ON)
            {
                return;
            }

            //(乗務中でない場合)選択車両の情報をローカル保持
            userPreference.setRidingVehicleInformation_ChoosingVehcle(selectVehicle);
            Preferences.Set("userPreferences.isSelectVehicle", true);

            //乗換画面から遷移した(乗換フラグが立っている)場合は、乗換確認画面へ遷移
            if (TransitInformation.isTransit)
            {
                //乗換確認画面に遷移
                Navigation.PushAsync(new TransitRidingVehicleConfirmActivity());
            }
            else
            {
                //乗務確認画面に遷移
                Navigation.PushAsync(new StartCrewingConfirmActivity());
            }

        }

    }
}

