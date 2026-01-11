using System.ComponentModel.DataAnnotations;

namespace StockApi.Dtos
{
    public class ProductCreateDto
    {

        public string Name { get; set; } = string.Empty;
        public int UnitMl { get; set; } = 750;
        public float TareWeightG { get; set; } = 0;
        public float LossAllowance { get; set; } = 0;
        public float SellingPrice { get; set; }
    }

    public class ProductUpdateDto
    {

        public string Name { get; set; } = string.Empty;
        public int UnitMl { get; set; } = 750;
        public float TareWeightG { get; set; } = 0;
        public float LossAllowance { get; set; } = 0;
        public float SellingPrice { get; set; }
    }
}
