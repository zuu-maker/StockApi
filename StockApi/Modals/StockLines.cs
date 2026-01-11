namespace StockApi.Modals
{
    public class StockLine
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ProductId { get; set; }
        public string ProductionName { get; set; } = string.Empty;
        public int ProductUnitMl { get; set; }
        public int UnitOfMeasure { get; set; }

        public float FullGrossG {  get; set; }

        public float CurrentGrossG { get; set; }

        public float RemainingVolumeMl { get; set; }
        public float RemainingServingsExact { get; set; }
        public int RemainngServingsWhole { get; set; }

        public float SellingPrice { get; set; }
        public float LineValue { get; set; }
        public string CreatedAt { get; set; } = string.Empty;


    }
}
