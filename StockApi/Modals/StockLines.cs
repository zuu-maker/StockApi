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

        public double FullGrossG {  get; set; }

        public double CurrentGrossG { get; set; }

        public double RemainingVolumeMl { get; set; }
        public double RemainingServingsExact { get; set; }
        public int RemainngServingsWhole { get; set; }

        public double SellingPrice { get; set; }
        public double LineValue { get; set; }
        public string CreatedAt { get; set; } = string.Empty;


    }
}
