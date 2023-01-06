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
using mobatako.utils;
using mobatako.utils.network;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using mobatako.common;

namespace mobatako.utils.network
{
    class CompanyProcessor
    {
        //API実行用
        HttpClient _client = new HttpClient();

        //車両一覧作成用
        public JArray resVehicleListObj = null;
        public int resVehicleListSize = 0;

        //作業状態取得用
        public static List<WorkStates> resWorkStatesObj = null;
        public static int resWorkStatesSize = 0;

        //給油情報取得用
        public static List<RefuelConfiguration> resRefuelConfigurationObj = null;
        public static int resRefuelConfigurationSize = 0;


        //企業情報取得API実行
        public async Task<bool> GetCompanyConfiguations()
        {
            var reqCompanyConfiguations = new HttpRequestMessage(HttpMethod.Get, Strings.WEB_API_BASE_URL + "v1/configuration/getCompanyConfigurations/");
            reqCompanyConfiguations.Headers.Add("ContentType", "application/json");
            reqCompanyConfiguations.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqCompanyConfiguations.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqCompanyConfiguations.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqCompanyConfiguations.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            var resCompanyConfiguations = await _client.SendAsync(reqCompanyConfiguations);
            var resCompanyConfiguationsStr = await resCompanyConfiguations.Content.ReadAsStringAsync();
            JObject resCompanyConfiguationsObj = JObject.Parse(resCompanyConfiguationsStr);
            //作業状態を取得
            resWorkStatesObj = JsonConvert.DeserializeObject<List<WorkStates>>(resCompanyConfiguationsObj["workStates"].ToString());
            resWorkStatesSize = resWorkStatesObj.Count();
            //給油情報を取得
            resRefuelConfigurationObj = JsonConvert.DeserializeObject<List<RefuelConfiguration>>(resCompanyConfiguationsObj["refuelConfigurations"].ToString());
            resRefuelConfigurationSize = resRefuelConfigurationObj.Count();
            return resCompanyConfiguations.IsSuccessStatusCode;
        }


        //車両一覧取得用API実行
        public async Task<bool> GetVehicleList()
        {
            var reqVehicleList = new HttpRequestMessage(HttpMethod.Get, Strings.WEB_API_BASE_URL + "v1/company/vehicleList/");
            reqVehicleList.Headers.Add("ContentType", "application/json");
            reqVehicleList.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqVehicleList.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqVehicleList.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqVehicleList.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            var resVehicleList = await _client.SendAsync(reqVehicleList);
            var resVehicleListStr = await resVehicleList.Content.ReadAsStringAsync();
            resVehicleListObj = JArray.Parse(resVehicleListStr);
            resVehicleListSize = resVehicleListObj.Count();
            return resVehicleList.IsSuccessStatusCode;
        }


        //車両一覧検索用API実行
        public async Task<bool> GetVehicleListCondition()
        {
            var reqVehicleListCondition = new HttpRequestMessage(HttpMethod.Get, Strings.WEB_API_BASE_URL + "v1/company/vehicleListCondition/");
            reqVehicleListCondition.Headers.Add("ContentType", "application/json");
            reqVehicleListCondition.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqVehicleListCondition.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqVehicleListCondition.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqVehicleListCondition.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            var resVehicleListCondition = await _client.SendAsync(reqVehicleListCondition);
            var resVehicleListConditionStr = await resVehicleListCondition.Content.ReadAsStringAsync();
            JArray resVehicleListConditionObj = JArray.Parse(resVehicleListConditionStr);
            var resVehicleListConditionSize = resVehicleListObj.Count();
            return resVehicleListCondition.IsSuccessStatusCode;
        }
    }

    //車両情報
    public class VehicleInformation
    {
        public long vehicleSeq { get; set; }
        public long logNo { get; set; }
        public string vehicleCode { get; set; }
        public string vehicleName { get; set; }
        public string? logitachoVehicleId { get; set; }
        public string? bluetoothMac { get; set; }
        public long rideType { get; set; }
        public long leavingReturningType { get; set; }
        public long loadEmpType { get; set; }
        public bool isLogitachoMounting { get; set; }
        public string logitachoSerialNo { get; set; }
        public string rideTypeLavel { get; set; }

        //public override string ToString()
        //{
        //    return $"{vehicleName}";
        //}
    }


    //乗務情報
    public class ChangeWorkState
    {
        public string PreviousStartWorkingAt { get; set; }
        public string PreviousStartWorkingDeviceId { get; set; }
        public string StartWorkingAt { get; set; }
        public string StartWorkingStateSeq { get; set; }
        public string StartWorkingStateLogNo { get; set; }
        public string GpsLatitude { get; set; }
        public string GpsLongitude { get; set; }
        public string GpsAzimuth { get; set; }
        public string GpsAccuracy { get; set; }
        public string GpsSatelliteCount { get; set; }
        public string GpsSpeed { get; set; }
        public string LoadEmpState { get; set; }
        public string CrewingStartDeviceId { get; set; }
        public string CrewingStartAt { get; set; }
        public string RideStartAt { get; set; }
        public string RideStartDeviceId { get; set; }
        public string RideStartDeviceSeqNo { get; set; }
    }

    //作業状態（workStates）
    public class WorkStates
    {
        
        public long workStateSeq { get; set; }
        public long workStateLogNo { get; set; }
        public string workStateName { get; set; }
        public bool isLinkRun { get; set; }
        public bool isLinkStop { get; set; }
        public bool isLinkCrewingStart { get; set; }
        public bool isLinkCrewingEnd { get; set; }
        public long linkLoadEmpType { get; set; }
        public long workStateType { get; set; }
        public bool isBoarding { get; set; }
        public bool isNotifyGetOff { get; set; }
        public bool isShowRefuelInput { get; set; }
        public bool isButtonHidden { get; set; }
        public long iconType { get; set; }
        public bool isHidden { get; set; }

    }

    //給油情報
    public class RefuelConfiguration
    {
        public long RefuelStationSeq { get; set; }
        public string RefuelStationName { get; set; }
        public bool NeedInputRefuelFee { get; set; }
        public long RefuelStationSortOrder { get; set; }
    }
}