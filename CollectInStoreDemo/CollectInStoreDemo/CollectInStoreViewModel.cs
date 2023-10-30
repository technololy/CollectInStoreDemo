using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalPlatformCommon.Shared.CollectInStore.Rows;
using TotalPlatformCommon.Shared.Models.CollectInStore;
using System.Text;

namespace CollectInStoreDemo
{
    public class CollectInStoreViewModel : ObservableObject
    {
        private readonly HttpClient _enquiryService;

        private CollectInStoreResponse? _searchResponse;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _accountRestricted;
        public bool AccountRestricted
        {
            get => _accountRestricted;
            set => SetProperty(ref _accountRestricted, value);
        }

        private bool _freeItemsTextVisibility = true;
        public bool FreeItemsTextVisibility
        {
            get => _freeItemsTextVisibility;
            set => SetProperty(ref _freeItemsTextVisibility, value);
        }

        private bool _noItemsTextVisibility;
        public bool NoItemsTextVisibility
        {
            get => _noItemsTextVisibility;
            set => SetProperty(ref _noItemsTextVisibility, value);
        }

        private string? _postcode;
        public string? Postcode
        {
            get => _postcode;
            set => SetProperty(ref _postcode, value);
        }

        public string ProductName { get; set; } = "";
        public string ProductItemCode { get; set; } = "";
        public string ProductOptionNumber { get; set; } = "";

        private string? _summaryText;
        public string SummaryText
        {
            get => _summaryText ?? "";
            set => SetProperty(ref _summaryText, value);
        }

        private bool _summaryTextVisibility;
        public bool SummaryTextVisibility
        {
            get => _summaryTextVisibility;
            set => SetProperty(ref _summaryTextVisibility, value);
        }

        private bool _showStoresWithAvailableStock = true;
        public bool ShowStoresWithAvailableStock
        {
            get => _showStoresWithAvailableStock;
            set
            {
                SetProperty(ref _showStoresWithAvailableStock, value);
                UpdateStockToggle();
            }
        }

        public ObservableCollection<StoreDetailsRow> Rows { get; }

        public ICommand PostcodeEnteredCommand { get; }

        public CollectInStoreViewModel()
        {
            _enquiryService = new HttpClient();
            PostcodeEnteredCommand = new AsyncRelayCommand(UpdatePostcodeAsync);
            Rows = new ObservableCollection<StoreDetailsRow>();
            NoItemsTextVisibility = false;
        }

        private void ClearSearchResults()
        {
            Rows.Clear();
        }

        private async Task ExecuteSearchAsync()
        {
            if (string.IsNullOrEmpty(Postcode))
            {
                return;
            }

            IsLoading = true;
            FreeItemsTextVisibility = false;
            _searchResponse = null;

            try
            {
                var apiResponse = await _enquiryService.GetAsync(@"https://www.next.co.uk/collectInStoreEnquiry?location=London&itemOption=T5095606&_=1696423895970");
                if (apiResponse.IsSuccessStatusCode)
                {
                    var content = await apiResponse.Content.ReadAsStringAsync();
                    _searchResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CollectInStoreResponse>(content);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void RefreshSearchResults()
        {
            if (_searchResponse == null)
            {
                return;
            }

            var stores = ConvertToRows(_searchResponse).ToList();

            if (stores.Count == 0)
            {
                NoItemsTextVisibility = true;
                SummaryTextVisibility = false;
                return;
            }

            var summaryFormat = ("Stores near *{0}* with stock availability of *{1}*");

            SummaryText = string.Format(summaryFormat, Postcode, ProductName);
            NoItemsTextVisibility = false;
            FreeItemsTextVisibility = false;
            SummaryTextVisibility = true;
            try
            {
                // stores.ForEach(store => Rows.Add(store));
                foreach (var item in stores)
                {
                    Rows.Add(item);
                }

            }
            catch (System.Exception ex)
            {

            }
        }

        private async Task UpdatePostcodeAsync()
        {
            ClearSearchResults();
            await ExecuteSearchAsync();
            RefreshSearchResults();
        }

        private void UpdateStockToggle()
        {
            if (string.IsNullOrEmpty(Postcode))
            {
                return;
            }
            ClearSearchResults();
            RefreshSearchResults();
        }

        private IEnumerable<StoreDetailsRow> ConvertToRows(CollectInStoreResponse response)
        {
            foreach (var itemOption in response.ItemOptions)
            {
                //if (itemOption.ItemOptionName != ProductItemCode + ProductOptionNumber)
                //{
                //    continue;
                //}

                foreach (var branch in itemOption.Branches)
                {
                    if (ShowStoresWithAvailableStock && branch.Stock == 0)
                    {
                        continue;
                    }

                    if (!ShowStoresWithAvailableStock && branch.Stock > 0)
                    {
                        continue;
                    }

                    var openingTimeFormat = branch.IsClosingSoon
                        ? ("Collect tomorrow from {0}")
                        : ("Open Today {0}");

                    var openingMessage = branch.IsClosingSoon
                        ? string.Format(openingTimeFormat, branch.BranchData.OpeningTime)
                        : string.Format(openingTimeFormat, branch.BranchData.CistOpeningMessage);

                    var closingMessage = ("*Closing soon:* {0}");
                    closingMessage = string.Format(closingMessage, openingMessage);

                    var storeDetails = branch.BranchData.BranchDisplayAddress
                        .Split(',')
                        .Select(x => x.Trim())
                        .ToList();

                    storeDetails.Add(branch.BranchData.BranchPostcode);

                    yield return new StoreDetailsRow(
                        ProductItemCode,
                        ProductOptionNumber,
                        branch.BranchNumber,
                        AddCollectInStoreItemToBag,
                        branch.BranchData.BranchName,
                        branch.Stock > 0,
                        branch.IsClosingSoon,
                        openingMessage,
                        closingMessage,
                        branch.BranchData.Distance,
                        storeDetails.ToArray());
                }
            }
        }

        private void AddCollectInStoreItemToBag(string branchId)
        {

        }
    }
}

