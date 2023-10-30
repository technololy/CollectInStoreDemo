using System;
using Xamarin.Forms;

namespace CollectInStoreDemo
{
    public class EnhancedListView : ListView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedListView"/> class.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        public EnhancedListView(ListViewCachingStrategy strategy) : base(strategy)
        {
        }

        public void ForceNativeTableUpdate()
        {
            ViewCellSizeChangedEvent?.Invoke();
        }

        public event Action ViewCellSizeChangedEvent;
    }
}

