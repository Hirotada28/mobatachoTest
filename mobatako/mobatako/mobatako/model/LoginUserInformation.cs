using System;
using System.Collections.Generic;
using System.Text;

namespace mobatako.model
{
    //ログインしているユーザのユーザ情報データクラス
    public class LoginUserInformation
    {
        public UserInformation user { get; set; }
        public CompanyInformation company { get; set; }
        public LastWorkState? lastWorkState { get; set; }

        //コンストラクタ
        public LoginUserInformation() 
        { 
            user = new UserInformation();
            company = new CompanyInformation();
            lastWorkState = new LastWorkState();
        }
    }

    //企業情報
    public class CompanyInformation 
    {
        public long? companySeq { get; set; }
        public long logNo { get; set; }
        public string companyName { get; set; }
    }

    //ユーザ情報
    public class UserInformation
    {
        public long? userSeq { get; set; }
        public long logNo { get; set; }
        public string userName { get; set; }
        public string logitachoDriverId { get; set; }
    }

    //作業情報
    public class LastWorkState
    {
        public WorkState state { get; set; }
        public string startWorking { get; set; }
        public string changeDeviceId { get; set; }

        //コンストラクタ
        public LastWorkState()
        {
            state = new WorkState();
        }
    }

}
