using System.Collections.Generic;

namespace TotalPlatformCommon.Shared.Models.CollectInStore
{
    public class CollectInStoreResponse
    {
        public IReadOnlyList<ItemOption> ItemOptions { get; set; }
        public GlobalEligibility GlobalEligibility { get; set; }
        public string LastCheckedRule { get; set; }
    }
}
