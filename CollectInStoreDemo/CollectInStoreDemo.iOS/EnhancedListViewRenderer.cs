using System;
using System.ComponentModel;
using CollectInStoreDemo;
using CollectInStoreDemo.iOS;
using CoreFoundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EnhancedListView), typeof(EnhancedListViewRenderer))]

namespace CollectInStoreDemo.iOS
{
    public class EnhancedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is EnhancedListView enhancedListView)
            {
                enhancedListView.ViewCellSizeChangedEvent += UpdateTableView;
            }
        }

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);

        //    if (Control == null)
        //    {
        //        return;
        //    }

        //    if (e.PropertyName == "SelectedItem")
        //    {
        //        foreach (var cell in Control.VisibleCells)
        //        {
        //            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
        //        }
        //    }
        //}

        private void UpdateTableView()
        {
            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(0.2)), () =>
            {
                if (!(Control is UITableView tv)) return;
                tv.BeginUpdates(); tv.EndUpdates();
            });
        }
    }
}

