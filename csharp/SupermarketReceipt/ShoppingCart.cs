using SupermarketReceipt.Constants;
using SupermarketReceipt.Strategies;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new List<ProductQuantity>();
        private readonly Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();
        private readonly Dictionary<SpecialOfferType, DiscountStrategy> _strategies;

        public ShoppingCart()
        {
            _strategies = new Dictionary<SpecialOfferType, DiscountStrategy>
            {
                { SpecialOfferType.ThreeForTwo, new QuantityDiscountStrategy(Default.THREE_REQUIRED_QUANTITY, Default.TWO_DISCOUNTED_QUANTITY) },
                { SpecialOfferType.PercentageDiscount, new PercentageDiscountStrategy() },
                { SpecialOfferType.TwoForAmount, new VolumeDiscount(Default.TWO_REQUIRED_QUANTITY) },
                { SpecialOfferType.FiveForAmount, new VolumeDiscount(Default.FIVE_REQUIRED_QUANTITY) },
            };
        }

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

                //Prepare Data
                var offer = offers[product];
                var quantity = _productQuantities[product];
                var unitPrice = catalog.GetUnitPrice(product);

                var strategy = _strategies.GetValueOrDefault(offer.OfferType, null);

                if (strategy == null)
                    throw new Exception(Message.ERROR_INVALID_OFFER_TYPE);

                if (offer.Argument > 0)
                    strategy._argument = offer.Argument;

                discount = strategy.Compute(product, quantity, unitPrice);

                if (discount != null)
                    receipt.AddDiscount(discount);
            } 
        }
    }
}