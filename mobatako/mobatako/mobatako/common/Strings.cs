using System;
using System.Collections.Generic;
using System.Text;

namespace mobatako.common
{
    class Strings
    {
        //public const string xxxxx = "yyyyy";

        //global
        public const string WEB_API_BASE_URL = "https://dev-webapi.mobatacho.com/";
        public const string SERVER_API_KEY = "bpKvV0bN11Uf@:Y+8gXP";

        public const string server_api_key = "bpKvV0bN11Uf@:Y+8gXP";
        public const string web_api_base_url = "https://dev-webapi.mobatacho.com/";
        
        //共通文言
        public const string permission_ok = "OK";
        public const string permission_announce_background_gps_title = "位置情報アクセスについて";
        public const string permission_announce_background_gps = "モバタコは位置情報をバックグラウンドで送信します。クラウド管理アプリで位置情報を表示する為に必要な情報ですので、直後に表示される許可ダイアログで「常に許可」を選択してください。";
        public const string remind_start_crewing = "乗務開始を忘れていませんか？";

        //エラー
        public const string error_connection_fail = "サーバーとの通信ができません";
        public const string error_connection_fail_description = "現在、サーバーと通信できません。しばらくしてから起動してください。";
        public const string error_login_fail = "ログインエラー";
        public const string error_login_fail_description = "ログインに失敗しました。ログインID・パスワードを再度確認してからログインしてください。";
        public const string error_login_without_company_code = "ユーザーコード@企業団体コード の形式で入力して下さい。";
        public const string error_internal = "内部処理失敗";
        public const string error_internal_description = "内部処理で異常が発生しました。ログイン画面に戻ります";
        public const string error_end_crewing = "乗務終了失敗";
        public const string error_end_crewing_description = "乗務終了に失敗しました。通信状況を確認して再度乗務終了をしてください。";
    }

    //乗務状態
    class RIDE_TYPE
    {
        public const long UNKNOWN = 0;
        public const long RIDE_ON = 1;
        public const long GET_OFF = 2;
    }
}
