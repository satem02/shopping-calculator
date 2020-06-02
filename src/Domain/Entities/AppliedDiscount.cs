using Domain.Enums;

namespace Domain.Entities
{
    public class AppliedDiscount
    {
        public DiscountType Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }   
    }
}