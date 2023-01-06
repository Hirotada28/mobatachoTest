using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mobatako
{
    //車両検索条件変更画面処理
    public partial class ChoosingVehicleConditionDialog : ContentPage
    {
        //初期処理
        public ChoosingVehicleConditionDialog()
        {
            InitializeComponent();

            //iosのデフォルトデザイン設定
            Thickness padding;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    padding = new Thickness(0, 10, 0, 0);
                    break;
                default:
                    padding = new Thickness();
                    break;
            }
            Padding = padding;
            choosingConditionVehicleName.Text = ChooseVehicleCondition.prevVehicleName;
            choosingVehicleGetOffOnly.IsChecked = ChooseVehicleCondition.prevIsChecked;
        }


        //「表示変更」ボタン押下時の処理
        async void ChangeConditionButton(System.Object sender, System.EventArgs e)
        {
            string vehicleName = choosingConditionVehicleName.Text;
            bool isChecked = choosingVehicleGetOffOnly.IsChecked;
            
            //検索条件が前回から変更があった場合のみ検索する
            if (!ChooseVehicleCondition.prevVehicleName.Equals(vehicleName) || ChooseVehicleCondition.prevIsChecked != isChecked)
            {
                //車両検索する
                ChooseVehicleCondition.prevVehicleName = vehicleName;
                ChooseVehicleCondition.prevIsChecked = isChecked;
                await Navigation.PushAsync(new ChoosingVehicleActivity());
            }
            else
            {
                //検索せずにモーダルを閉じる
                Navigation.PopModalAsync();
            }
        }
    }

    //検索条件の一時保持用
    class ChooseVehicleCondition
    {
        //前回入力車両名
        static public string prevVehicleName = "";
        //前回チェック
        static public bool prevIsChecked = false;
    }
}