namespace TotalPlatformCommon.Shared.Models.CollectInStore
{
    public class CollectInStoreTodayItem
    {
        public string Id { get; set; }
        public string Option { get; set; }
        public string CistStoreId { get; set; }
        public int Quantity { get; set; }
        public string? Chain { get; set; }
    }
}
