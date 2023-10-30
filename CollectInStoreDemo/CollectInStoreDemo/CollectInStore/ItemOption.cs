using System.Collections.Generic;
using Newtonsoft.Json;

namespace TotalPlatformCommon.Shared.Models.CollectInStore
{
    public class ItemOption
    {
        [JsonProperty("ItemOption")]
        public string ItemOptionName { get; set; }
        public int QuantityRequested { get; set; }
        public int? QuantityExisting { get; set; }
        public IReadOnlyList<Branch> Branches { get; set; }
        public Eligibility Eligibility { get; set; }
        public bool IsOnlyInStockInClosedStore { get; set; }
    }
}
