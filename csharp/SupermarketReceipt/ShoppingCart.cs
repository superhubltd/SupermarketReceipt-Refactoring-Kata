using System;
using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new List<ProductQuantity>();
        private readonly Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");

        private const string PRICE_FORMAT = "N2";
        private const string PERCENT_FORMAT = "{0}% off";
        private const string QTY_FOR_AMOUNT_FORMAT = "{0} for {1}";
        private const string THREE_FOR_TWO = "3 for 2";
        private const double ONE_HUNDRED_PERCENT = 100.0;
        private const int THREE_QTY = 3;
        private const int TWO_QTY = 2;
        private const int FIVE_QTY = 5;

        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
        }

        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
            if (_productQuantities.ContainsKey(product))
            {
                var newAmount = _productQuantities[product] + quantity;
                _productQuantities[product] = newAmount;
            }
            else
            {
                _productQuantities.Add(product, quantity);
            }
        }

        public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, ISupermarketCatalog catalog)
        {
            foreach (var product in _productQuantities.Keys)
            {
                Discount discount = null;
                if (!offers.ContainsKey(product))
                    continue;

                var unitPrice = catalog.GetUnitPrice(product);
                var offer = offers[product];
                switch (offer.OfferType)
                {
                    case SpecialOfferType.ThreeForTwo:
                        discount = ComputeForThreeForTwoDiscount(product, unitPrice);
                        break;
                    case SpecialOfferType.TwoForAmount:
                        discount = ComputeForTwoForAmountDiscount(product, unitPrice, offer.Argument);
                        break;
                    case SpecialOfferType.FiveForAmount:
                        discount = ComputeForFiveForAmountDiscount(product, unitPrice, offer.Argument);
                        break;
                    default: /* Should be SpecialOfferType.PercentageDiscount */
                        discount = ComputeForPercentageDiscount(product, unitPrice, offer.Argument);
                        break;
                }

                if (discount != null)
                    receipt.AddDiscount(discount);
            } 
        }

        private Discount ComputeForPercentageDiscount(Product product, double price, double discount)
        {
            var quantity = _productQuantities[product];
            var normalPrice = ComputeNormalPrice(price, quantity);
            var percentageDiscount = (discount / ONE_HUNDRED_PERCENT);
            var totalDiscount = normalPrice * percentageDiscount;
            return new Discount(product, string.Format(PERCENT_FORMAT, discount), -totalDiscount);
        } 

        private Discount ComputeForThreeForTwoDiscount(Product product, double price)
        {
            var quantity = _productQuantities[product];
            if (quantity <= TWO_QTY)
                return null;

            var normalPrice = ComputeNormalPrice(price, quantity);
            var qualifiedSet = ComputeQualifiedSet((int)quantity, THREE_QTY);
            var totalDiscount = normalPrice - (qualifiedSet * TWO_QTY * price + quantity % THREE_QTY * price);
            return new Discount(product, THREE_FOR_TWO, -totalDiscount);
        }

        private Discount ComputeForTwoForAmountDiscount(Product product, double price, double assumedPrice)
        {
            return ComputeForSpecificQtyDiscount(product, price, assumedPrice, TWO_QTY);
        }

        private Discount ComputeForFiveForAmountDiscount(Product product, double price, double assumedPrice)
        {
            return ComputeForSpecificQtyDiscount(product, price, assumedPrice, FIVE_QTY);
        }

        private Discount ComputeForSpecificQtyDiscount(Product product, double price, double assumedPrice, int discountedQty)
        {
            var quantity = _productQuantities[product];
            if (quantity < discountedQty)
                return null;

            var normalPrice = ComputeNormalPrice(price, quantity);
            var qualifiedSet = ComputeQualifiedSet((int)quantity,  discountedQty);
            var totalDiscount = normalPrice - (assumedPrice * qualifiedSet + quantity % discountedQty * price);
            return new Discount(product, string.Format(QTY_FOR_AMOUNT_FORMAT, discountedQty, PrintPrice(assumedPrice)), -totalDiscount);
        }

        private double ComputeNormalPrice(double price, double quantity)
        {
            return price * quantity;
        }

        private int ComputeQualifiedSet(int quantity, int discountedQty)
        {
            return quantity / discountedQty;
        }

        private string PrintPrice(double price)
        {
            return price.ToString(PRICE_FORMAT, Culture);
        }
    }
}