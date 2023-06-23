using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly ISupermarketCatalog _catalog;
        private readonly Dictionary<Product, Offer> _offers = new Dictionary<Product, Offer>();

        public Teller(ISupermarketCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddSpecialOffer(SpecialOfferType offerType, Product product, double argument)
        {
            _offers[product] = new Offer(offerType, product, argument);
        }

        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            foreach (var productQty in productQuantities)
            {
                var product = productQty.Product;
                var quantity = productQty.Quantity;
                var unitPrice = _catalog.GetUnitPrice(product);
                var price = quantity * unitPrice;
                receipt.AddProduct(product, quantity, unitPrice, price);
            }

            theCart.HandleOffers(receipt, _offers, _catalog);

            return receipt;
        }
    }
}