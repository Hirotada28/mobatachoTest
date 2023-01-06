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
using mobatako.utils.network;

namespace mobatako.utils
{
    public class UserPreference
    {
        //ログイン情報
        public static LoginUserInformation userInfo = new LoginUserInformation();

        //乗務情報
        public static CrewingInformation crewingInfo = new CrewingInformation();

        //乗車車両情報
        public static RidingVehicleInformation vehicleInfo = new RidingVehicleInformation();


        //ログイン情報保持メソッド_ユーザ,企業
        public void setLoginUserInformation() 
        {
            userInfo.user.userSeq = LoginRestProcessor.userObj.userSeq;
            userInfo.user.logNo = LoginRestProcessor.userObj.logNo;
            userInfo.user.userName = LoginRestProcessor.userObj.userName;
            userInfo.user.logitachoDriverId = LoginRestProcessor.userObj.logitachoDriverId;
            userInfo.company.companySeq = LoginRestProcessor.companyObj.companySeq;
            userInfo.company.logNo = LoginRestProcessor.companyObj.logNo;
            userInfo.company.companyName = LoginRestProcessor.companyObj.companyName;
            return;
        }

        //ログイン情報保持メソッド_作業状態
        public void setLoginUserInformation_WorkState()
        {
            userInfo.lastWorkState.state = LoginRestProcessor.loggedInUserCrewingInfo.lastWorkState;
            userInfo.lastWorkState.startWorking = LoginRestProcessor.loggedInUserCrewingInfo.lastWorkState.workStateName;
            userInfo.lastWorkState.changeDeviceId = LoginRestProcessor.loggedInUserCrewingInfo.startWorkingDeviceId;   
            return;
        }

        //乗務情報保持メソッド
        public void setCrewingInformation()
        {
            crewingInfo.LeavingAt = LoginRestProcessor.loggedInUserCrewingInfo.leavingAt;
            crewingInfo.LeavingDeviceId = LoginRestProcessor.loggedInUserCrewingInfo.leavingDeviceId;
            crewingInfo.LeavingVehicleSeq = LoginRestProcessor.loggedInUserCrewingInfo.leavingVehicleSeq;
            crewingInfo.LeavingVehicleLogNo = LoginRestProcessor.loggedInUserCrewingInfo.leavingVehicleLogNo;
            crewingInfo.RidingStartAt = LoginRestProcessor.loggedInUserCrewingInfo.startRidingAt;
            crewingInfo.RideStartDeviceId = LoginRestProcessor.loggedInUserCrewingInfo.rideStartDeviceId;
            crewingInfo.RideStartDeviceSeq = LoginRestProcessor.loggedInUserCrewingInfo.rideStartDeviceSeq;
            crewingInfo.CrewingStartAt = LoginRestProcessor.loggedInUserCrewingInfo.startCrewingAt;
            crewingInfo.CrewingStartDeviceId = LoginRestProcessor.loggedInUserCrewingInfo.crewingStartDeviceId;
            crewingInfo.CrewingStartDeviceSeq = LoginRestProcessor.loggedInUserCrewingInfo.crewingStartDeviceSeq;
            return;
        }

        //乗務車両情報保持メソッド(乗務中再ログイン時)
        public void setRidingVehicleInformation()
        {
            vehicleInfo.vehicleSeq = LoginRestProcessor.loggedInUserCrewingInfo.ridingVehicle.vehicleSeq;
            vehicleInfo.logNo = LoginRestProcessor.loggedInUserCrewingInfo.ridingVehicle.logNo;
            vehicleInfo.vehicleCode = LoginRestProcessor.loggedInUserCrewingInfo.ridingVehicle.vehicleCode;
            vehicleInfo.vehicleName = LoginRestProcessor.loggedInUserCrewingInfo.ridingVehicle.vehicleName;
            vehicleInfo.logitachoVehicleId = LoginRestProcessor.loggedInUserCrewingInfo.ridingVehicle.logitachoVehicleId;
            return;
        }

        //乗車車両情報保持メソッド(一覧から車両選択時)
        public void setRidingVehicleInformation_ChoosingVehcle(VehicleInformation iVehicleInfo)
        {
            vehicleInfo.vehicleSeq = iVehicleInfo.vehicleSeq;
            vehicleInfo.logNo = iVehicleInfo.logNo;
            vehicleInfo.vehicleCode = iVehicleInfo.vehicleCode;
            vehicleInfo.vehicleName = iVehicleInfo.vehicleName;
            vehicleInfo.logitachoVehicleId = iVehicleInfo.logitachoVehicleId;
            return;
        }

        //乗務or乗換開始情報保持メソッド
        public void setCrewingInformation_StartCrewingOrTransitRiding()
        {
            crewingInfo = OperationProcessor.crewingInformation;
            return;
        }

    }
}
