using System;
namespace mobatako.common
{
	public class MobatachoCmmnMbr
	{
        //　給油情報入力画面から遷移する時に、遷移元がホーム画面か乗換画面かで遷移先を変更するのに必要なフラグ
        public static bool isFromHome = false;

        //　ホーム画面に遷移する際に実行する作業状態変更APIの実行有無の判断に必要なフラグ
        public static bool isFromSplash = false;

        // 給油情報画面に遷移するフラグ
        public static bool isSelectRefuel = false;

        // 給油情報画面に遷移するフラグ
        public static string tmpWorkName = "";

        // 給油入力画面からホーム画面に遷移する時に立てるフラグ
        public static bool isFromInputRefuel = false;
    }
}

