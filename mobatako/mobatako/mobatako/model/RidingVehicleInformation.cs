using System;
using System.Collections.Generic;
using System.Text;

namespace mobatako.model
{
    //ログインしているユーザーが乗車している車輌情報のデータクラス
    public class RidingVehicleInformation
    {
        public long vehicleSeq { get; set; }
        public long logNo { get; set; }
        public string vehicleCode { get; set; }
        public string vehicleName { get; set; }
        public string? logitachoVehicleId { get; set; }
    }
}
