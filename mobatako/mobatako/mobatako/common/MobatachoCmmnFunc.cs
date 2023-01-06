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

using mobatako.utils.network;

namespace mobatako.common
{
    public class MobatachoCmmnFunc
    {
        //Preference(ﾛｰｶﾙ保持)一覧

        //■ﾕｰｻﾞ情報
        // userPreferences
        //    usercode
        //    companycode
        //    userPassword
        //    checkViewPassword
        //    checkSavingUserInfo
        //    deviceid
        //    userseq
        //    companyseq
        //    devicelogno

        //■乗務車両情報
        // ridingVehicleInformation
        //    vehicleSeq
        //    logNo
        //    vehicleCode
        //    vehicleName
        //    logitachoVehicleId
        //    bluetoothMac
        //    rideType
        //    leavingReturningType
        //    loadEmpType
        //    isLogitachoMounting
        //    logitachoSerialNo


        //ﾛｰｶﾙ保持情報のｸﾘｱ
        public void clearUserPreference()
        {
            Preferences.Set("userPreferences.usercode", "");
            Preferences.Set("userPreferences.companycode", "");
            Preferences.Set("userPreferences.userPassword", "");
            Preferences.Set("userPreferences.checkViewPassword", false);
            Preferences.Set("userPreferences.checkSavingUserInfo", false);
            Preferences.Set("userPreferences.deviceid", "");
            Preferences.Set("userPreferences.userseq", "");
            Preferences.Set("userPreferences.companyseq", "");
            Preferences.Set("userPreferences.devicelogno", "");
        }

        //ﾛｰｶﾙ保持(乗務情報)のｸﾘｱ
        public void clearRidingVehicleInformation()
        {
            Preferences.Set("ridingVehicleInformation.vehicleSeq", "");
            Preferences.Set("ridingVehicleInformation.logNo", "");
            Preferences.Set("ridingVehicleInformation.vehicleCode", "");
            Preferences.Set("ridingVehicleInformation.vehicleName", "");
            Preferences.Set("ridingVehicleInformation.logitachoVehicleId", "");
            Preferences.Set("ridingVehicleInformation.bluetoothMac", "");
            Preferences.Set("ridingVehicleInformation.rideType", "");
            Preferences.Set("ridingVehicleInformation.leavingReturningType", "");
            Preferences.Set("ridingVehicleInformation.loadEmpType", "");
            Preferences.Set("ridingVehicleInformation.isLogitachoMounting", false);
            Preferences.Set("ridingVehicleInformation.logitachoSerialNo", "");
        }
    }
}
