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
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using mobatako.utils.network;
using mobatako.common;

namespace mobatako
{
    //帰庫宿泊選択画面処理
    public partial class EndCrewingConfirmDialog : ContentPage
    {
        //初期処理
        public EndCrewingConfirmDialog()
        {
            InitializeComponent();
        }


        //「帰庫」,「宿泊」ボタン押下時の処理
        async void EndCrewingDialogButton(System.Object sender, System.EventArgs e)
        {
            //押下ボタンを取得しローカル保持
            Button selectWorkBtn = (Button)sender;
            switch (selectWorkBtn.Text)
            {
                case "帰庫":
                    Preferences.Set("EndCrewingState", "帰庫");
                    break;
                case "宿泊":
                    Preferences.Set("EndCrewingState", "宿泊");
                    break;
                default:
                    break;
            }

            //タコデータ送信スプラッシュ画面に遷移し、乗務終了確認とダイアログは閉じる
            Navigation.InsertPageBefore(new SendingTachoDataActivity(), this);
            await Navigation.PopAsync();
            await Navigation.PopModalAsync();
        }


        //「キャンセル」ボタン押下時の処理
        void EndCrewingDialogCancelButton(System.Object sender, System.EventArgs e)
        {
            //乗務終了確認ダイアログを閉じ、乗務終了確認画面に戻る
            Navigation.PopModalAsync();
        }
    }
}

