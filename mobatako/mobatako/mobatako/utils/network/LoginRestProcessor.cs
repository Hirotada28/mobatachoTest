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
using mobatako.model;
using mobatako.utils.network;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using mobatako.common;

namespace mobatako.utils.network
{
    class LoginRestProcessor
    {
        //API実行用
        HttpClient _client = new HttpClient();

        //ユーザ情報,企業情報保持用
        public static UserObj userObj = new UserObj();
        public static CompanyObj companyObj = new CompanyObj();
        public static WorkStates lastWorkState = new WorkStates();
        public static UserInformationStruct loggedInUserCrewingInfo = new UserInformationStruct();

        public UserObj userObj2 = new UserObj();
        public CompanyObj companyObj2 = new CompanyObj();


        //ログインAPI実行
        public async Task<bool> PostLogin(string body)
        {
            var reqLogin = new HttpRequestMessage(HttpMethod.Post, Strings.WEB_API_BASE_URL + "v1/login");
            reqLogin.Headers.Add("ContentType", "application/json");
            reqLogin.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqLogin.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var resLogin = await _client.SendAsync(reqLogin);

            //ログイン失敗したら取得処理中断
            if (resLogin.IsSuccessStatusCode == false) { return resLogin.IsSuccessStatusCode; }

            //ユーザ,企業情報取得
            var resLoginStr = await resLogin.Content.ReadAsStringAsync();
            JObject resLoginObj = JObject.Parse(resLoginStr);
            userObj = JsonConvert.DeserializeObject<UserObj>(resLoginObj["user"].ToString());
            companyObj = JsonConvert.DeserializeObject<CompanyObj>(resLoginObj["company"].ToString());

            //デバイス情報
            Preferences.Set("userPreferences.logno", Convert.ToString(userObj.logNo));
            Preferences.Set("userPreferences.devicelogno", "1");

            return resLogin.IsSuccessStatusCode;
        }


        //ユーザ情報取得API実行
        public async Task<bool> GetUserInformation()
        {
            var reqUserInformation = new HttpRequestMessage(HttpMethod.Get, Strings.WEB_API_BASE_URL + "v1/user/information");
            reqUserInformation.Headers.Add("ContentType", "application/json");
            reqUserInformation.Headers.Add("x-mobatacho-key", Strings.SERVER_API_KEY);
            reqUserInformation.Headers.Add("x-mobatacho-user-seq", UserPreference.userInfo.user.userSeq.ToString());
            reqUserInformation.Headers.Add("x-mobatacho-user-log-no", UserPreference.userInfo.user.logNo.ToString());
            reqUserInformation.Headers.Add("x-mobatacho-device-id", Preferences.Get("userPreferences.deviceid", ""));
            reqUserInformation.Headers.Add("x-mobatacho-device-log-no", Preferences.Get("userPreferences.devicelogno", "1"));
            var resUserInformation = await _client.SendAsync(reqUserInformation);
            var resUserInformationStr = await resUserInformation.Content.ReadAsStringAsync();
            if (resUserInformationStr == "") { return false; }

            //前回の情報を保持
            JObject resUserInformationObj = JObject.Parse(resUserInformationStr);
            lastWorkState = JsonConvert.DeserializeObject<WorkStates>(resUserInformationObj["lastWorkState"].ToString());
            loggedInUserCrewingInfo = JsonConvert.DeserializeObject<UserInformationStruct>(resUserInformationObj.ToString());
            PreviousCrewingInformation.PreviousStartWorkingAt = loggedInUserCrewingInfo.startWorkingAt;
            PreviousCrewingInformation.PreviousStartWorkingDeviceId = loggedInUserCrewingInfo.startWorkingDeviceId;

            return true;
        }
    }


    //ログイン情報
    public class loginInfo
    {
        public string userCode { get; set; }
        public string companyCode { get; set; }
        public string userPassword { get; set; }
        public string deviceId { get; set; }
    }

    //ユーザ情報
    public class UserObj
    {
        public long userSeq { get; set; }
        public long logNo { get; set; }
        public long companySeq { get; set; }
        public string userCode { get; set; }
        public string userName { get; set; }
        public string? logitachoDriverId { get; set; }
        public long isHidden { get; set; }
        public string createdAt { get; set; }
        public long createdUserSeq { get; set; }
        public string? updatedAt { get; set; }
        public long? updatedUserSeq { get; set; }
    }

    //企業情報
    public class CompanyObj
    {
        public long companySeq { get; set; }
        public long logNo { get; set; }
        public long parentCompanySeq { get; set; }
        public string companyCode { get; set; }
        public string companyName { get; set; }
        public string companyNameHiragana { get; set; }
        public string zipCode { get; set; }
        public string addressPrefecture { get; set; }
        public string? addressCounty { get; set; }
        public string addressCity { get; set; }
        public string addressNo { get; set; }
        public string addressEtc { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string? telNo { get; set; }
        public string? faxNo { get; set; }
        public string logitachoCompanyId { get; set; }
        public string createdAt { get; set; }
        public long createdUserSeq { get; set; }
        public string? updatedAt { get; set; }
        public long? updatedUserSeq { get; set; }
        public long closingDate { get; set; }
        public long closingDateType { get; set; }
        public long yearStartMonth { get; set; }
    }

    //ユーザ情報(乗車車両情報,乗務情報,作業状態情報)
    public class UserInformationStruct
    {
        public RidingVehicleInformation ridingVehicle { get; set; }
        public WorkState lastWorkState { get; set; }

        public string leavingAt { get; set; }
        public string leavingDeviceId { get; set; }
        public long leavingVehicleSeq { get; set; }
        public long leavingVehicleLogNo { get; set; }
        public string startRidingAt { get; set; }
        public string rideStartDeviceId { get; set; }
        public long rideStartDeviceSeq { get; set; }
        public string startCrewingAt { get; set; }
        public string crewingStartDeviceId { get; set; }
        public long crewingStartDeviceSeq { get; set; }
        public string startWorkingAt { get; set; }
        public string startWorkingDeviceId { get; set; }
        public string logitachoDriverId { get; set; }
        public string? logitachoSerialNo { get; set; }
        public string? vehicleBluetoothMacAddress { get; set; }
        public short vehicleRideType { get; set; }
        public short vehicleLoadEmpType { get; set; }
        public bool? isLogitachoMounting { get; set; }

        //コンストラクタ
        public UserInformationStruct()
        {
            ridingVehicle = new RidingVehicleInformation();
            lastWorkState = new WorkState();
        }
    }
}
