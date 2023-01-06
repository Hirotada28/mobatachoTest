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
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using mobatako.common;
using mobatako.model;


namespace mobatako.utils.network
{
    class OperationProcessor
    {
        //API実行用
        HttpClient _client = new HttpClient();

        //乗務or乗換開始情報
        public static CrewingInformation crewingInformation = new CrewingInformation();

        //乗務開始API実行
        public async Task<bool> PostStartCrewing(string body)
        {
            var reqStartCrewing = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/operation/startCrewing");
            reqStartCrewing.Headers.Add("ContentType", "application/json");
            reqStartCrewing.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqStartCrewing.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqStartCrewing.Headers.Add("x-mobatacho-company-log-no", UserPreference.userInfo.company.logNo.ToString());
            reqStartCrewing.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqStartCrewing.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqStartCrewing.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqStartCrewing.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqStartCrewing.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqStartCrewing.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqStartCrewing.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resStartCrewing = await _client.SendAsync(reqStartCrewing);

            //乗務失敗したら取得処理中断
            if (resStartCrewing.IsSuccessStatusCode == false) { return resStartCrewing.IsSuccessStatusCode; }

            //乗務情報取得
            var resStartCrewingStr = await resStartCrewing.Content.ReadAsStringAsync();
            JObject resStartCrewingObj = JObject.Parse(resStartCrewingStr);
            crewingInformation = JsonConvert.DeserializeObject<CrewingInformation>(resStartCrewingObj.ToString());
            return resStartCrewing.IsSuccessStatusCode;
        }


        //乗務終了API実行
        public async Task<bool> PostEndCrewing(string body)
        {
            var reqEndCrewing = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/operation/endCrewing");
            reqEndCrewing.Headers.Add("ContentType", "application/json");
            reqEndCrewing.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqEndCrewing.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-company-log-no", UserPreference.userInfo.company.logNo.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqEndCrewing.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqEndCrewing.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqEndCrewing.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resEndCrewing = await _client.SendAsync(reqEndCrewing);
            return resEndCrewing.IsSuccessStatusCode;
        }


        //乗換開始API実行
        public async Task<bool> PostRideToVehicleByTransist(string body)
        {
            var reqTransitRide = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/operation/rideToVehicleByTransit");
            reqTransitRide.Headers.Add("ContentType", "application/json");
            reqTransitRide.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqTransitRide.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-company-log-no", UserPreference.userInfo.company.logNo.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqTransitRide.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqTransitRide.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqTransitRide.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resTransitRide = await _client.SendAsync(reqTransitRide);

            //乗換失敗したら取得処理中断
            if (resTransitRide.IsSuccessStatusCode == false) { return resTransitRide.IsSuccessStatusCode; }

            //乗換開始情報取得
            var resTransitRideStr = await resTransitRide.Content.ReadAsStringAsync();
            JObject resTransitRideObj = JObject.Parse(resTransitRideStr);
            crewingInformation = JsonConvert.DeserializeObject<CrewingInformation>(resTransitRideObj.ToString());
            return resTransitRide.IsSuccessStatusCode;
        }


        //乗換降車API実行
        public async Task<bool> PostGetOffVehicleBTransit(string body)
        {
            var reqTransitWork = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/operation/getOffVehicleByTransit");
            reqTransitWork.Headers.Add("ContentType", "application/json");
            reqTransitWork.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqTransitWork.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-company-log-no", UserPreference.userInfo.company.logNo.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqTransitWork.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqTransitWork.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqTransitWork.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resTransitWork = await _client.SendAsync(reqTransitWork);
            return resTransitWork.IsSuccessStatusCode;
        }


        //作業状態変更API実行
        public async Task<bool> PostChangeWorkState(string body)
        {
            var reqChangeWorkState = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/operation/changeWorkState");
            reqChangeWorkState.Headers.Add("ContentType", "application/json");
            reqChangeWorkState.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqChangeWorkState.Headers.Add("x-mobatacho-company-seq", UserPreference.userInfo.company.companySeq.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-company-log-no", UserPreference.userInfo.company.logNo.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqChangeWorkState.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqChangeWorkState.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqChangeWorkState.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resChangeWorkState = await _client.SendAsync(reqChangeWorkState);

            var resChangeWorkStateStr = await resChangeWorkState.Content.ReadAsStringAsync();
            JObject resChangeWorkStateObj = JObject.Parse(resChangeWorkStateStr);

            return resChangeWorkState.IsSuccessStatusCode;
        }

        //給油情報送信API実行
        public async Task<bool> PostRefuelInfo(string body)
        {
            var reqRefuelInfo = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/vehicle/refuel");
            reqRefuelInfo.Headers.Add("ContentType", "application/json");
            reqRefuelInfo.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqRefuelInfo.Headers.Add("x-mobatacho-vehicle-seq", UserPreference.vehicleInfo.vehicleSeq.ToString());
            reqRefuelInfo.Headers.Add("x-mobatacho-vehicle-log-no", UserPreference.vehicleInfo.logNo.ToString());
            reqRefuelInfo.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqRefuelInfo.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            reqRefuelInfo.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resRefuelInfo = await _client.SendAsync(reqRefuelInfo);
            return resRefuelInfo.IsSuccessStatusCode;
        }

    }


    // 乗務開始要求に必要なリクエストパラメータ
    public class StartCrewingRequest
    {
        public string CrewingAt { get; set; }
        public string StartRidingAt { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }
        public int GpsAzimuth { get; set; }
        public double GpsAccuracy { get; set; }
        public int GpsSatelliteCount { get; set; }
        public double GpsSpeed { get; set; }
    }

    // 乗務終了要求に必要なリクエストパラメータ
    public class EndCrewingRequest
    {
        public string EndCrewingAt { get; set; }
        public string StartCrewingAt { get; set; }
        public string LeavingAt { get; set; }
        public string LeavingDeviceId { get; set; }
        public long LeavingVehicleSeq { get; set; }
        public bool IsReturning { get; set; }
        public string CrewingAt { get; set; }
        public string StartRidingAt { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }
        public int GpsAzimuth { get; set; }
        public double GpsAccuracy { get; set; }
        public int GpsSatelliteCount { get; set; }
        public double GpsSpeed { get; set; }
        public bool IsRest { get; set; }
    }

    // 乗換乗車要求のリクエストパラメータ
    public class RideToVehicleByTransitRequest
    {
        public string RideAt { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }
        public int GpsAzimuth { get; set; }
        public double GpsAccuracy { get; set; }
        public int GpsSatelliteCount { get; set; }
        public double GpsSpeed { get; set; }
        public string CrewingStartAt { get; set; }
        public string CrewingStartDeviceID { get; set; }
        public long CrewingStartDeviceSeqNo { get; set; }
    }

    // 乗換降車要求のリクエストパラメータ
    public class GetOffVehicleBTransitRequest
    {
        public string GetoffAt { get; set; }
        public string StartRidingAt { get; set; }
        public string LeavingAt { get; set; }
        public string LeavingDeviceId { get; set; }
        public long LeavingVehicleSeq { get; set; }
        public bool IsReturning { get; set; }
        public string RideAt { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }
        public int GpsAzimuth { get; set; }
        public double GpsAccuracy { get; set; }
        public int GpsSatelliteCount { get; set; }
        public double GpsSpeed { get; set; }
        public string CrewingStartAt { get; set; }
        public string CrewingStartDeviceID { get; set; }
        public long CrewingStartDeviceSeqNo { get; set; }
    }

    // 作業状態変更に必要なリクエストパラメータ
    public class ChangeWorkStateRequest
    {
        public string PreviousStartWorkingAt { get; set; }
        public string PreviousStartWorkingDeviceId { get; set; }
        public string StartWorkingAt { get; set; }
        public long StartWorkingStateSeq { get; set; }
        public long StartWorkingStateLogNo { get; set; }
        public double GpsLatitude { get; set; }
        public double GpsLongitude { get; set; }
        public int GpsAzimuth { get; set; }
        public double GpsAccuracy { get; set; }
        public int GpsSatelliteCount { get; set; }
        public double GpsSpeed { get; set; }
        public short LoadEmpState { get; set; }
        public string CrewingStartDeviceId { get; set; }
        public string CrewingStartAt { get; set; }
        public string RideStartAt { get; set; }
        public string RideStartDeviceId { get; set; }
        public long RideStartDeviceSeqNo { get; set; }
    }

    public class RefuelInfoRequest
    {
        public string RefuelAt { get; set; }
        public long RefuelStationSeq { get; set; }
        public long RefuelFee { get; set; }
        public double RefuelAmount { get; set; }
    }
}
