using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace TotalPlatformCommon.Shared.CollectInStore.Rows
{
    public class StoreDetailsRow : ObservableObject
    {
        private readonly string _itemCode;
        private readonly string _optionNumber;
        private readonly string _branchId;
        private readonly Action<string> _addToBagAction;

        public string StoreName { get; }
        public bool IsStockAvailable { get; }
        public bool IsClosingSoon { get; }
        public string OpeningMessage { get; }
        public string ClosingSoonMessage { get; }
        public double Distance { get; }
        public IReadOnlyList<string> StoreDetails { get; }

        private bool _storeDetailsVisible;

        public bool StoreDetailsVisible
        {
            get => _storeDetailsVisible;
            set => SetProperty(ref _storeDetailsVisible, value);
        }

        public ICommand? HideStoreDetailsCommand { get; }
        public ICommand? ShowStoreDetailsCommand { get; }
        public ICommand? AddToBagCommand { get; }

        public StoreDetailsRow(
            string itemCode, string optionNumber, string branchId, Action<string> addToBagAction,
            string storeName, bool isStockAvailable, bool isClosingSoon, string openingMessage,
            string closingSoonMessage, string distance, string[] storeDetails)
        {
            _itemCode = itemCode;
            _optionNumber = optionNumber;
            _branchId = branchId;
            _addToBagAction = addToBagAction;

            StoreName = storeName;
            IsStockAvailable = isStockAvailable;
            IsClosingSoon = isClosingSoon;
            OpeningMessage = openingMessage;
            ClosingSoonMessage = closingSoonMessage;
            StoreDetails = new List<string>(storeDetails);

            if (double.TryParse(distance, NumberStyles.AllowDecimalPoint, null, out var d))
            {
                Distance = d;
            }

            HideStoreDetailsCommand = new RelayCommand(() => StoreDetailsVisible = false);
            ShowStoreDetailsCommand = new RelayCommand(() => StoreDetailsVisible = true);

            AddToBagCommand = new RelayCommand(AddToBag, canExecute:() => isStockAvailable);
        }

        private void AddToBag()
        {
            _addToBagAction.Invoke(_branchId);
        }
    }
}
