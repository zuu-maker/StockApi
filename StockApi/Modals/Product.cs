using System.ComponentModel.DataAnnotations;

namespace StockApi.Modals
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public int UnitMl { get; set; } = 750;
        public float TareWeightG { get; set; } = 0;
        public float LossAllowance { get; set; } = 0;
        public float SellingPrice { get; set; }


    }
}
