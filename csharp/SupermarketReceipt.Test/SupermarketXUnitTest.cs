using System;
using System.Collections.Generic;
using Xunit;

namespace SupermarketReceipt.Test
{
    public class SupermarketXUnitTest
    {
        [Fact]
        public void TenPercentDiscount()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();
            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 0.99);
            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 1.99);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            Assert.Equal(4.975, receipt.GetTotalPrice());
            Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
            Assert.Single(receipt.GetItems());
            var receiptItem = receipt.GetItems()[0];
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(1.99, receiptItem.Price);
            Assert.Equal(2.5 * 1.99, receiptItem.TotalPrice);
            Assert.Equal(2.5, receiptItem.Quantity);
        }


        [Theory]
        [InlineData("Item1",  ProductUnit.Each,  10,  1)]
        [InlineData("Item2",  ProductUnit.Each,  20,  2)]
        [InlineData("Item3",  ProductUnit.Kilo,  30,  3)]
        [InlineData("Item4",  ProductUnit.Kilo,  40,  4)]
        [InlineData("Item5",  ProductUnit.Kilo,  50,  5)]
        [InlineData("Item6",  ProductUnit.Kilo,  60,  6)]
        public void SingeItem_Normal_Test(string prodName, ProductUnit prodUnit, double prodPrice, double prodQuantity)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(prodName, prodUnit);

            catalog.AddProduct(item, prodPrice);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, prodQuantity);

            var teller = new Teller(catalog);

            // Act
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Assert
            Assert.Single(receipt.GetItems());
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(prodUnit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(prodPrice, receipt.GetItems()[0].Price);
            Assert.Equal(prodQuantity, receipt.GetItems()[0].Quantity);
            Assert.Equal(prodPrice * prodQuantity, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData("Item1", ProductUnit.Each, 10, 3, null, 0, 30)]
        [InlineData("Item2", ProductUnit.Each, 10, 3, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Item3", ProductUnit.Each, 10, 3, SpecialOfferType.TenPercentDiscount, 10, 27)]
        [InlineData("Item4", ProductUnit.Each, 10, 5, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Item5", ProductUnit.Each, 10, 2, SpecialOfferType.TwoForAmount, 40, 40)]
        public void SingleItem_SpecialOffer_ThreeForTwo_Test(string name, ProductUnit unit, double price, double quantity, SpecialOfferType? offer, double amount, double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            var teller = new Teller(catalog);

            if(offer != null)
                teller.AddSpecialOffer((SpecialOfferType)offer, item, amount);

            // Act
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Single(receipt.GetItems());
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(quantity, receipt.GetItems()[0].Quantity);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
            if(offer != null)
                Assert.Equal(expectedPrice - (price * quantity), receipt.GetDiscounts()[0].DiscountAmount);
        }
    }
}