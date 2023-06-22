using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new List<ProductQuantity>();
        private readonly Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");


        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
        }

        public void AddItem(Product product)
        {
            AddItemQuantity(product, 1.0);
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

                var offer = offers[product];
                switch (offer.OfferType)
                {
                    case SpecialOfferType.ThreeForTwo:
                        discount = ComputeDiscountForThreeForTwo(product, catalog);
                        break;
                    case SpecialOfferType.TenPercentDiscount:
                        discount = ComputeDiscountForTenPercentDiscount(product, catalog, offer.Argument);
                        break;
                    case SpecialOfferType.TwoForAmount:
                        discount = ComputeDiscountForTwoForAmount(product, catalog, offer.Argument);
                        break;
                    case SpecialOfferType.FiveForAmount:
                        discount = ComputeDiscountForFiveForAmount(product, offers, catalog);
                        break;
                    default:
                        break;
                }

                if (discount != null)
                    receipt.AddDiscount(discount);
            }
            #region Old

            //foreach (var p in _productQuantities.Keys)
            //{
            //    var quantity = _productQuantities[p];
            //    var quantityAsInt = (int) quantity;
            //    if (offers.ContainsKey(p))
            //    {
            //        var offer = offers[p];
            //        var unitPrice = catalog.GetUnitPrice(p);
            //        Discount discount = null;
            //        var x = 1;
            //        if (offer.OfferType == SpecialOfferType.ThreeForTwo)
            //        {
            //            x = 3;
            //        }
            //        else if (offer.OfferType == SpecialOfferType.TwoForAmount)
            //        {
            //            x = 2;
            //            if (quantityAsInt >= 2)
            //            {
            //                var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
            //                var discountN = unitPrice * quantity - total;
            //                discount = new Discount(p, "2 for " + PrintPrice(offer.Argument), -discountN);
            //            }
            //        }

            //        if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
            //        var numberOfXs = quantityAsInt / x;
            //        if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
            //        {
            //            var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
            //            discount = new Discount(p, "3 for 2", -discountAmount);
            //        }

            //        if (offer.OfferType == SpecialOfferType.TenPercentDiscount) discount = new Discount(p, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
            //        if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
            //        {
            //            var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
            //            discount = new Discount(p, x + " for " + PrintPrice(offer.Argument), -discountTotal);
            //        }

            //        if (discount != null)
            //            receipt.AddDiscount(discount);
            //    }
            //}
            #endregion
        }

        private Discount ComputeDiscountForThreeForTwo(Product product, ISupermarketCatalog catalog)
        {
            var quantity = _productQuantities[product];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(product);
            var numberOfXs = quantityAsInt / 3;
            if (quantityAsInt < 2)
                return null;

            var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
            return new Discount(product, "3 for 2", -discountAmount);
        }

        private Discount ComputeDiscountForTenPercentDiscount(Product product, ISupermarketCatalog catalog, double discount)
        {
            var quantity = _productQuantities[product];
            var unitPrice = catalog.GetUnitPrice(product);
            return new Discount(product, discount + "% off", -quantity * unitPrice * discount / 100.0);
        }

        private Discount ComputeDiscountForTwoForAmount(Product product, ISupermarketCatalog catalog, double assumedPrice)
        {
            var quantity = _productQuantities[product];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(product);
            var x = 2;
            if (quantityAsInt < 2)
                return null;

            var total = assumedPrice * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
            var discountN = unitPrice * quantity - total;
            return new Discount(product, "2 for " + PrintPrice(assumedPrice), -discountN);
            
        }
        private Discount ComputeDiscountForFiveForAmount(Product product, Dictionary<Product, Offer> offers, ISupermarketCatalog catalog)
        {
            var quantity = _productQuantities[product];
            var quantityAsInt = (int)quantity;
            var offer = offers[product];
            var unitPrice = catalog.GetUnitPrice(product);
            var x = 5;
            var numberOfXs = quantityAsInt / x;

            if (quantityAsInt < 5)
                return null;

            var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
            return new Discount(product, x + " for " + PrintPrice(offer.Argument), -discountTotal);
        }


        private string PrintPrice(double price)
        {
            return price.ToString("N2", Culture);
        }
    }
}