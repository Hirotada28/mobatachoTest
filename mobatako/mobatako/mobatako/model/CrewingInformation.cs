using System;
using System.Collections.Generic;
using System.Text;

namespace mobatako.model
{
    //乗務情報
    public class CrewingInformation
    {
        public string LeavingAt { get; set; }
        public string LeavingDeviceId { get; set; }
        public long LeavingVehicleSeq { get; set; }
        public long LeavingVehicleLogNo { get; set; }
        public string RidingStartAt { get; set; }
        public string RideStartDeviceId { get; set; }
        public long RideStartDeviceSeq { get; set; }
        public string CrewingStartAt { get; set; }
        public string CrewingStartDeviceId { get; set; }
        public long CrewingStartDeviceSeq { get; set; }
    }

    //日時
    public class CommonDate
    {
        public static string StartAt = "";  //開始時間
        public static string PreviousStartAt = "";  //前回の開始時間
    }

    public class PreviousCrewingInformation
    {
        public static string PreviousStartWorkingAt = "";  //開始時間
        public static string PreviousStartWorkingDeviceId = "";  //前回の開始時間
    }
}
