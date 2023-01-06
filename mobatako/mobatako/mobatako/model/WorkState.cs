using System;
using System.Collections.Generic;
using System.Text;

namespace mobatako.model
{
    //作業状態を格納するデータクラス
    public class WorkState
    {
        public long workStateSeq { get; set; }
        public long workStateLogNo { get; set; }
        public string workStateName { get; set; }
        public bool isLinkRun { get; set; }
        public bool isLinkStop { get; set; }
        public bool isLinkCrewingStart { get; set; }
        public bool isLinkCrewingEnd { get; set; }
        public short linkLoadEmpType { get; set; }
        public short workStateType { get; set; }
        public bool isBoarding { get; set; }
        public bool isNotifyGetOff { get; set; }
        public bool isShowRefuelInput { get; set; }
        public bool isButtonHidden { get; set; }
    }
}
