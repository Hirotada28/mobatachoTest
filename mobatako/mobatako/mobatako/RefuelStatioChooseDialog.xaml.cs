using System;
using System.Collections.Generic;
using mobatako.common;
using mobatako.utils.network;
using mobatako.model;

using Xamarin.Forms;
using static System.Collections.Specialized.BitVector32;

namespace mobatako
{
    //給油所選択画面処理
    public partial class RefuelStatioChooseDialog : ContentPage
    {
        //初期処理
        public RefuelStatioChooseDialog()
        {
            InitializeComponent();
            Thickness padding;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    padding = new Thickness(0, 30, 0, 60);
                    break;
                default:
                    padding = new Thickness();
                    break;
            }
            Padding = padding;

            //給油所ボタンリスト作成
            CreateRefuelBtns();
        }


        //給油所ボタン押下時の処理
        void RefuelTypeClicked(System.Object sender, System.EventArgs e)
        {
            //選択給油所情報を保持して給油入力画面へ遷移
            Button btn = (Button)sender;
            int id = int.Parse(btn.ClassId);
            RefuelConfiguration selectedRefuelStation = CompanyProcessor.resRefuelConfigurationObj[id];
            Navigation.PushModalAsync(new NavigationPage(new InputRefuelAmountDialog(selectedRefuelStation)));
        }


        //「キャンセル」ボタン押下時の処理
        void CancelBtn(System.Object sender, System.EventArgs e)
        {
            ////乗換フラグによって遷移画面切替
            //if (TransitInformation.isTransit)
            //{
            //    //乗換画面へ遷移
            //    Navigation.PushAsync(new TransitVehicleChooseWorkDialog());
            //}
            //else
            //{
            //    //前画面へ戻る
            //}
            Navigation.PopModalAsync();
        }


        //給油所ボタンリスト作成
        void CreateRefuelBtns()
        {
            CompanyProcessor.resRefuelConfigurationObj.Sort((a, b) => (int)a.RefuelStationSortOrder - (int)b.RefuelStationSortOrder);
            if (CompanyProcessor.resRefuelConfigurationSize == 0) return;
            for (int i = 0; i < CompanyProcessor.resRefuelConfigurationSize; i++)
            {
                Button btn = new Button
                {
                    Text = CompanyProcessor.resRefuelConfigurationObj[i].RefuelStationName,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 250,
                    TextColor = Color.White,
                    Background = Color.FromRgb(51, 153, 255),
                    ClassId = i.ToString(),
                };
                btn.Clicked += RefuelTypeClicked;
                RefuelTypeButtons.Children.Add(btn);
            }
        }
    }
}

