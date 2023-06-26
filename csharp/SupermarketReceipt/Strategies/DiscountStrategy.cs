using SupermarketReceipt.Constants;
using System.Globalization;

namespace SupermarketReceipt.Strategies
{
    public abstract class DiscountStrategy
    {
        public double _argument;  
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");
        public abstract Discount Compute(Product product, double quantity, double price);

        protected double ComputeNormalPrice(double price, double quantity)
        {
            return price * quantity;
        }

        protected int ComputeQualifiedSet(int quantity, int discountedQty)
        {
            return quantity / discountedQty;
        }

        protected string PrintPrice(double price)
        {
            return price.ToString(Format.PRICE_FORMAT, Culture);
        }
    }

    public class PercentageDiscountStrategy : DiscountStrategy
    {
        //argument will be the discount percentage
        public override Discount Compute(Product product, double quantity, double price)
        {
            var percentage = _argument;
            var normalPrice = ComputeNormalPrice(price, quantity);
            var percentageDiscount = (percentage / 100);
            var totalDiscount = normalPrice * percentageDiscount;
            return new Discount(product, string.Format(Format.PERCENT_DISCOUNT_FORMAT, percentage), -totalDiscount);
        }
    }

    public class QuantityDiscountStrategy : DiscountStrategy
    {
        private readonly int _requiredQuantity;
        private readonly int _discountedQuantity;
        public QuantityDiscountStrategy(int requiredQuantity, int discountedQuantity)
        {
            _requiredQuantity = requiredQuantity;
            _discountedQuantity = discountedQuantity;
        }

        public override Discount Compute(Product product, double quantity, double price)
        {
            if (quantity < _requiredQuantity)
                return null;

            var normalPrice = ComputeNormalPrice(price, quantity);
            var qualifiedSet = ComputeQualifiedSet((int)quantity, _requiredQuantity);
            var totalDiscount = normalPrice - (qualifiedSet * _discountedQuantity * price + quantity % _requiredQuantity * price);
            return new Discount(product, string.Format(Format.QUANTITY_DISCOUNT_FORMAT, _requiredQuantity, _discountedQuantity), -totalDiscount);
        }
    }

    public class VolumeDiscount : DiscountStrategy
    {
        private readonly int _requiredQuantity;
        public VolumeDiscount(int requiredQuantity) => _requiredQuantity = requiredQuantity;

        public override Discount Compute(Product product, double quantity, double price)
        {
            if (quantity < _requiredQuantity)
                return null;

            var discountedAmount = _argument;
            var normalPrice = ComputeNormalPrice(price, quantity);
            var qualifiedSet = ComputeQualifiedSet((int)quantity, _requiredQuantity);

            var totalDiscount = normalPrice - (discountedAmount * qualifiedSet + quantity % _requiredQuantity * price);
            return new Discount(product, string.Format(Format.VOLUME_DISCOUNT_FORMAT, _requiredQuantity, PrintPrice(discountedAmount)), -totalDiscount);
        }
    }
}
