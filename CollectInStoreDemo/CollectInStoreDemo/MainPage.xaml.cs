using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CollectInStoreDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new CollectInStoreViewModel();
        }

        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var label = sender as Label;
            var viewCell = label.Parent.Parent.Parent as ViewCell;
            var viewCellContext =  viewCell.BindingContext as TotalPlatformCommon.Shared.CollectInStore.Rows.StoreDetailsRow;

            var StoreDetailsControl = viewCell.FindByName<StackLayout>("StoreDetailsControl");
            if (StoreDetailsControl is not null)
            {

                //viewCell.Height += StoreDetailsControl.HeightRequest;
                var storeMeasure = StoreDetailsControl.Measure(StoreDetailsControl.Width, double.PositiveInfinity);
                //open the expand. add the height first
                if (!viewCellContext.StoreDetailsVisible)
                {
                    viewCell.View.HeightRequest = viewCell.View.Height + storeMeasure.Minimum.Height+10;
                    CollectInStoreListView.ForceNativeTableUpdate();
                    Task.Delay(500).ContinueWith((a) => {
                        viewCellContext.StoreDetailsVisible = !viewCellContext.StoreDetailsVisible;
                    }
                    );
                }
                //close the expand. remove the height first
                else
                {
                    viewCell.View.HeightRequest = viewCell.View.Height - storeMeasure.Minimum.Height-10;
                    CollectInStoreListView.ForceNativeTableUpdate();
                    viewCellContext.StoreDetailsVisible = !viewCellContext.StoreDetailsVisible;
                }
                //StoreDetailsControl.IsVisible = !StoreDetailsControl.IsVisible;
            }

        }

        void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;
            }
        }
    }
}

