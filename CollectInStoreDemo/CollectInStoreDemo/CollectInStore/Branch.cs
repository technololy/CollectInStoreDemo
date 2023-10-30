using System.Collections.Generic;

namespace TotalPlatformCommon.Shared.Models.CollectInStore
{
    public class Branch
    {
        public string BranchNumber { get; set; }
        public int Stock { get; set; }
        public BranchData BranchData { get; set; }
        public Eligibility Eligibility { get; set; }
        public IReadOnlyList<NextOpeningTime> NextOpeningTimes { get; set; }
        public bool IsEligible { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosingSoon { get; set; }
        public int ItemId { get; set; }
    }
}
