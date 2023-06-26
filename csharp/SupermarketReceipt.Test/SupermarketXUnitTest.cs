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
            teller.AddSpecialOffer(SpecialOfferType.PercentageDiscount, toothbrush, 10.0);

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
        #region Item with no Special Offer
        [InlineData("Item with no Special Offer", ProductUnit.Each, 10, 1, 10)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 20, 1, 20)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 30, 1, 30)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 40, 1, 40)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 50, 1, 50)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Each, 10, 5, 50)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 20, 5, 100)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 30, 5, 150)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 40, 5, 200)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 50, 5, 250)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Each, 10, 10, 100)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 20, 10, 200)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 30, 10, 300)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 40, 10, 400)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 50, 10, 500)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 10, 1.5, 15)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 20, 1.5, 30)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 30, 1.5, 45)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 40, 1.5, 60)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 50, 1.5, 75)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 10, 5.7, 57)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 20, 5.7, 114)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 30, 5.7, 171)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 40, 5.7, 228)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 50, 5.7, 285)]
        /*-----------r------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 10, 10.9, 109)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 20, 10.9, 218)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 30, 10.9, 327)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 40, 10.9, 436)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 50, 10.9, 545)]
        #endregion
        public void Single_Item_Test(string name,
                                     ProductUnit unit,
                                     double price,
                                     double quantity,
                                     double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            var teller = new Teller(catalog);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Single(receipt.GetItems());
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(quantity, receipt.GetItems()[0].Quantity);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
        }

        [Theory]
        #region Item with Three for Two special offer
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 3, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 3, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 3, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 3, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 3, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 4, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 4, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 4, SpecialOfferType.ThreeForTwo, 0, 90)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 4, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 4, SpecialOfferType.ThreeForTwo, 0, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 6, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 6, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 6, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 6, SpecialOfferType.ThreeForTwo, 0, 160)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 6, SpecialOfferType.ThreeForTwo, 0, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 8, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 8, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 8, SpecialOfferType.ThreeForTwo, 0, 180)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 8, SpecialOfferType.ThreeForTwo, 0, 240)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 8, SpecialOfferType.ThreeForTwo, 0, 300)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 3, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 3, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 3, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 3, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 3, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 4, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 4, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 4, SpecialOfferType.ThreeForTwo, 0, 90)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 4, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 4, SpecialOfferType.ThreeForTwo, 0, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 6, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 6, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 6, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 6, SpecialOfferType.ThreeForTwo, 0, 160)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 6, SpecialOfferType.ThreeForTwo, 0, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 8, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 8, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 8, SpecialOfferType.ThreeForTwo, 0, 180)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 8, SpecialOfferType.ThreeForTwo, 0, 240)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 8, SpecialOfferType.ThreeForTwo, 0, 300)]
        #endregion

        #region Item with Percentage base special offer
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 0, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 10, 90)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 20, 80)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 30, 70)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 40, 60)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 50, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 60, 40)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 70, 30)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 80, 20)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 90, 10)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 1, SpecialOfferType.PercentageDiscount, 100, 0)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 0, 500)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 10, 450)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 20, 400)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 30, 350)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 40, 300)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 50, 250)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 60, 200)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 70, 150)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 80, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 90, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 5, SpecialOfferType.PercentageDiscount, 100, 0)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 0, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 10, 90)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 20, 80)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 30, 70)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 40, 60)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 50, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 60, 40)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 70, 30)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 80, 20)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 90, 10)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 1, SpecialOfferType.PercentageDiscount, 100, 0)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 0, 500)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 10, 450)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 20, 400)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 30, 350)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 40, 300)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 50, 250)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 60, 200)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 70, 150)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 80, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 90, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 5, SpecialOfferType.PercentageDiscount, 100, 0)]
        #endregion

        #region Item with Five for Amount (ex. Five for 100)
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 5, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 5, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 5, SpecialOfferType.FiveForAmount, 145, 145)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 5, SpecialOfferType.FiveForAmount, 185, 185)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 5, SpecialOfferType.FiveForAmount, 230, 230)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 8, SpecialOfferType.FiveForAmount, 40, 70)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 8, SpecialOfferType.FiveForAmount, 90, 150)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 8, SpecialOfferType.FiveForAmount, 145, 235)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 8, SpecialOfferType.FiveForAmount, 185, 305)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 8, SpecialOfferType.FiveForAmount, 230, 380)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 10, SpecialOfferType.FiveForAmount, 90, 180)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 10, SpecialOfferType.FiveForAmount, 145, 290)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 10, SpecialOfferType.FiveForAmount, 185, 370)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 10, SpecialOfferType.FiveForAmount, 230, 460)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 12, SpecialOfferType.FiveForAmount, 40, 100)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 12, SpecialOfferType.FiveForAmount, 90, 220)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 12, SpecialOfferType.FiveForAmount, 145, 350)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 12, SpecialOfferType.FiveForAmount, 185, 450)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 12, SpecialOfferType.FiveForAmount, 230, 560)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 5, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 5, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 5, SpecialOfferType.FiveForAmount, 145, 145)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 5, SpecialOfferType.FiveForAmount, 185, 185)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 5, SpecialOfferType.FiveForAmount, 230, 230)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 8, SpecialOfferType.FiveForAmount, 40, 70)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 8, SpecialOfferType.FiveForAmount, 90, 150)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 8, SpecialOfferType.FiveForAmount, 145, 235)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 8, SpecialOfferType.FiveForAmount, 185, 305)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 8, SpecialOfferType.FiveForAmount, 230, 380)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 10, SpecialOfferType.FiveForAmount, 40, 80)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 10, SpecialOfferType.FiveForAmount, 90, 180)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 10, SpecialOfferType.FiveForAmount, 145, 290)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 10, SpecialOfferType.FiveForAmount, 185, 370)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 10, SpecialOfferType.FiveForAmount, 230, 460)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 12, SpecialOfferType.FiveForAmount, 40, 100)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 12, SpecialOfferType.FiveForAmount, 90, 220)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 12, SpecialOfferType.FiveForAmount, 145, 350)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 12, SpecialOfferType.FiveForAmount, 185, 450)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 12, SpecialOfferType.FiveForAmount, 230, 560)]
        #endregion

        #region Item with Two for Amount (ex. Two for 100)
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 2, SpecialOfferType.TwoForAmount, 150, 150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 2, SpecialOfferType.TwoForAmount, 480, 480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 2, SpecialOfferType.TwoForAmount, 500, 500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 2, SpecialOfferType.TwoForAmount, 750, 750)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 2, SpecialOfferType.TwoForAmount, 990, 990)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 2, SpecialOfferType.TwoForAmount, 1000, 1000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 3, SpecialOfferType.TwoForAmount, 150, 250)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 3, SpecialOfferType.TwoForAmount, 480, 680)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 3, SpecialOfferType.TwoForAmount, 500, 800)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 3, SpecialOfferType.TwoForAmount, 750, 1150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 3, SpecialOfferType.TwoForAmount, 990, 1490)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 3, SpecialOfferType.TwoForAmount, 1000, 1600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 4, SpecialOfferType.TwoForAmount, 150, 300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 4, SpecialOfferType.TwoForAmount, 480, 960)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 4, SpecialOfferType.TwoForAmount, 500, 1000)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 4, SpecialOfferType.TwoForAmount, 750, 1500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 4, SpecialOfferType.TwoForAmount, 990, 1980)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 4, SpecialOfferType.TwoForAmount, 1000, 2000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 5, SpecialOfferType.TwoForAmount, 150, 400)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 5, SpecialOfferType.TwoForAmount, 480, 1160)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 5, SpecialOfferType.TwoForAmount, 500, 1300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 5, SpecialOfferType.TwoForAmount, 750, 1900)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 5, SpecialOfferType.TwoForAmount, 990, 2480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 5, SpecialOfferType.TwoForAmount, 1000, 2600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 2, SpecialOfferType.TwoForAmount, 150, 150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 2, SpecialOfferType.TwoForAmount, 480, 480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 2, SpecialOfferType.TwoForAmount, 500, 500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 2, SpecialOfferType.TwoForAmount, 750, 750)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 2, SpecialOfferType.TwoForAmount, 990, 990)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 2, SpecialOfferType.TwoForAmount, 1000, 1000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 3, SpecialOfferType.TwoForAmount, 150, 250)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 3, SpecialOfferType.TwoForAmount, 480, 680)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 3, SpecialOfferType.TwoForAmount, 500, 800)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 3, SpecialOfferType.TwoForAmount, 750, 1150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 3, SpecialOfferType.TwoForAmount, 990, 1490)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 3, SpecialOfferType.TwoForAmount, 1000, 1600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 4, SpecialOfferType.TwoForAmount, 150, 300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 4, SpecialOfferType.TwoForAmount, 480, 960)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 4, SpecialOfferType.TwoForAmount, 500, 1000)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 4, SpecialOfferType.TwoForAmount, 750, 1500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 4, SpecialOfferType.TwoForAmount, 990, 1980)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 4, SpecialOfferType.TwoForAmount, 1000, 2000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 5, SpecialOfferType.TwoForAmount, 150, 400)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 5, SpecialOfferType.TwoForAmount, 480, 1160)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 5, SpecialOfferType.TwoForAmount, 500, 1300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 5, SpecialOfferType.TwoForAmount, 750, 1900)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 5, SpecialOfferType.TwoForAmount, 990, 2480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 5, SpecialOfferType.TwoForAmount, 1000, 2600)]
        #endregion
        public void Single_Item_With_Special_Offers_Test(string name,
                                              ProductUnit unit,
                                              double price,
                                              double quantity,
                                              SpecialOfferType offer,
                                              double percentOrAmount,
                                              double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(offer, item, percentOrAmount);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Single(receipt.GetItems());
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(quantity, receipt.GetItems()[0].Quantity);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
            Assert.Equal(expectedPrice - (price * quantity), receipt.GetDiscounts()[0].DiscountAmount);
        }
        
        [Theory]
        #region Not Qualified Item with Three for Two special offer
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 10, 1, SpecialOfferType.ThreeForTwo, 0, 10)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 20, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 30, 1, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 40, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 50, 1, SpecialOfferType.ThreeForTwo, 0, 50)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 10, 2, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 20, 2, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 30, 2, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 40, 2, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 50, 2, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 10, 1, SpecialOfferType.ThreeForTwo, 0, 10)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 20, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 30, 1, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 40, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 50, 1, SpecialOfferType.ThreeForTwo, 0, 50)]
        /*------------------------------------------------------------*/              
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 10, 2, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 20, 2, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 30, 2, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 40, 2, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 50, 2, SpecialOfferType.ThreeForTwo, 0, 100)]
        #endregion

        #region Not Qualified Item with Five for Amount (ex. Five for 100)
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 1, SpecialOfferType.FiveForAmount, 10, 10)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 1, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 1, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 1, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 1, SpecialOfferType.FiveForAmount, 50, 50)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 2, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 2, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 2, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 2, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 2, SpecialOfferType.FiveForAmount, 100, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 3, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 3, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 3, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 3, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 3, SpecialOfferType.FiveForAmount, 150, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 4, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 4, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 4, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 4, SpecialOfferType.FiveForAmount, 160, 160)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 4, SpecialOfferType.FiveForAmount, 200, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 1, SpecialOfferType.FiveForAmount, 10, 10)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 1, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 1, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 1, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 1, SpecialOfferType.FiveForAmount, 50, 50)]
        /*------------------------------------------------------------*/              
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 2, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 2, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 2, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 2, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 2, SpecialOfferType.FiveForAmount, 100, 100)]
        /*------------------------------------------------------------*/                   
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 3, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 3, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 3, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 3, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 3, SpecialOfferType.FiveForAmount, 150, 150)]
        /*------------------------------------------------------------*/                  
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 4, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 4, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 4, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 4, SpecialOfferType.FiveForAmount, 160, 160)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 4, SpecialOfferType.FiveForAmount, 200, 200)]
        #endregion

        #region Not Qualified Item with Two for Amount (ex. Two for 100)
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 10, 1, SpecialOfferType.TwoForAmount, 10, 10)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 20, 1, SpecialOfferType.TwoForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 30, 1, SpecialOfferType.TwoForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 40, 1, SpecialOfferType.TwoForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 50, 1, SpecialOfferType.TwoForAmount, 50, 50)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 10, 1, SpecialOfferType.TwoForAmount, 10, 10)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 20, 1, SpecialOfferType.TwoForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 30, 1, SpecialOfferType.TwoForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 40, 1, SpecialOfferType.TwoForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 50, 1, SpecialOfferType.TwoForAmount, 50, 50)]
        #endregion
        public void Not_Qualified_Single_Item_With_Special_Offer_Test(string name,
                                                                      ProductUnit unit,
                                                                      double price,
                                                                      double quantity,
                                                                      SpecialOfferType offer,
                                                                      double percentOrAmount,
                                                                      double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(offer, item, percentOrAmount);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Single(receipt.GetItems());
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(quantity, receipt.GetItems()[0].Quantity);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
            Assert.Empty(receipt.GetDiscounts());
        }


        [Theory]
        #region Item with no Special Offer
        [InlineData("Item with no Special Offer", ProductUnit.Each, 10, 1, 2, 30)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 20, 1, 2, 60)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 30, 1, 2, 90)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 40, 1, 2, 120)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 50, 1, 2, 150)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Each, 10, 2, 3, 50)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 20, 2, 3, 100)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 30, 2, 3, 150)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 40, 2, 3, 200)]
        [InlineData("Item with no Special Offer", ProductUnit.Each, 50, 2, 3, 250)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 10, 1.5, 2, 35)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 20, 1.5, 2, 70)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 30, 1.5, 2, 105)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 40, 1.5, 2, 140)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 50, 1.5, 2, 175)]
        /*-----------------------------------------------*/
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 10, 2.5, 3, 55)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 20, 2.5, 3, 110)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 30, 2.5, 3, 165)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 40, 2.5, 3, 220)]
        [InlineData("Item with no Special Offer", ProductUnit.Kilo, 50, 2.5, 3, 275)]
        #endregion
        public void Single_Item_With_Additional_Quantity_Test(string name,
                                     ProductUnit unit,
                                     double price,
                                     double quantity,
                                     double additionalQty,
                                     double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            //Additional Quantity
            cart.AddItemQuantity(item, additionalQty);

            var teller = new Teller(catalog);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
        }

        [Theory]

        #region Item with Three for Two special offer
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 2, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 2, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 2, 1, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 2, 1, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 2, 1, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 2, 2, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 2, 2, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 2, 2, SpecialOfferType.ThreeForTwo, 0, 90)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 2, 2, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 2, 2, SpecialOfferType.ThreeForTwo, 0, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 3, 3, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 3, 3, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 3, 3, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 3, 3, SpecialOfferType.ThreeForTwo, 0, 160)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 3, 3, SpecialOfferType.ThreeForTwo, 0, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 10, 4, 4, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 20, 4, 4, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 30, 4, 4, SpecialOfferType.ThreeForTwo, 0, 180)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 40, 4, 4, SpecialOfferType.ThreeForTwo, 0, 240)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Each, 50, 4, 4, SpecialOfferType.ThreeForTwo, 0, 300)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 2, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 2, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 2, 1, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 2, 1, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 2, 1, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 2, 2, SpecialOfferType.ThreeForTwo, 0, 30)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 2, 2, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 2, 2, SpecialOfferType.ThreeForTwo, 0, 90)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 2, 2, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 2, 2, SpecialOfferType.ThreeForTwo, 0, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 3, 3, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 3, 3, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 3, 3, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 3, 3, SpecialOfferType.ThreeForTwo, 0, 160)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 3, 3, SpecialOfferType.ThreeForTwo, 0, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 10, 4, 4, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 20, 4, 4, SpecialOfferType.ThreeForTwo, 0, 120)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 30, 4, 4, SpecialOfferType.ThreeForTwo, 0, 180)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 40, 4, 4, SpecialOfferType.ThreeForTwo, 0, 240)]
        [InlineData("Item with Three for Two special offer", ProductUnit.Kilo, 50, 4, 4, SpecialOfferType.ThreeForTwo, 0, 300)]
        #endregion

        #region Item with Percentage base special offer
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 0, 500)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 10, 450)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 20, 400)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 30, 350)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 40, 300)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 50, 250)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 60, 200)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 70, 150)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 80, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 90, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Each, 100, 4, 1, SpecialOfferType.PercentageDiscount, 100, 0)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 0, 500)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 10, 450)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 20, 400)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 30, 350)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 40, 300)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 50, 250)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 60, 200)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 70, 150)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 80, 100)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 90, 50)]
        [InlineData("Item with Percentage base special offer", ProductUnit.Kilo, 100, 4, 1, SpecialOfferType.PercentageDiscount, 100, 0)]
        #endregion

        #region Item with Five for Amount (ex. Five for 100)
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 1, 4, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 1, 4, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 1, 4, SpecialOfferType.FiveForAmount, 145, 145)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 1, 4, SpecialOfferType.FiveForAmount, 185, 185)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 1, 4, SpecialOfferType.FiveForAmount, 230, 230)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 4, 4, SpecialOfferType.FiveForAmount, 40, 70)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 4, 4, SpecialOfferType.FiveForAmount, 90, 150)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 4, 4, SpecialOfferType.FiveForAmount, 145, 235)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 4, 4, SpecialOfferType.FiveForAmount, 185, 305)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 4, 4, SpecialOfferType.FiveForAmount, 230, 380)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 7, 3, SpecialOfferType.FiveForAmount, 90, 180)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 7, 3, SpecialOfferType.FiveForAmount, 145, 290)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 7, 3, SpecialOfferType.FiveForAmount, 185, 370)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 7, 3, SpecialOfferType.FiveForAmount, 230, 460)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 10, 2, SpecialOfferType.FiveForAmount, 40, 100)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 10, 2, SpecialOfferType.FiveForAmount, 90, 220)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 10, 2, SpecialOfferType.FiveForAmount, 145, 350)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 10, 2, SpecialOfferType.FiveForAmount, 185, 450)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 10, 2, SpecialOfferType.FiveForAmount, 230, 560)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 1, 4, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 1, 4, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 1, 4, SpecialOfferType.FiveForAmount, 145, 145)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 1, 4, SpecialOfferType.FiveForAmount, 185, 185)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 1, 4, SpecialOfferType.FiveForAmount, 230, 230)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 4, 4, SpecialOfferType.FiveForAmount, 40, 70)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 4, 4, SpecialOfferType.FiveForAmount, 90, 150)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 4, 4, SpecialOfferType.FiveForAmount, 145, 235)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 4, 4, SpecialOfferType.FiveForAmount, 185, 305)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 4, 4, SpecialOfferType.FiveForAmount, 230, 380)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 7, 3, SpecialOfferType.FiveForAmount, 40, 80)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 7, 3, SpecialOfferType.FiveForAmount, 90, 180)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 7, 3, SpecialOfferType.FiveForAmount, 145, 290)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 7, 3, SpecialOfferType.FiveForAmount, 185, 370)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 7, 3, SpecialOfferType.FiveForAmount, 230, 460)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 10, 2, SpecialOfferType.FiveForAmount, 40, 100)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 10, 2, SpecialOfferType.FiveForAmount, 90, 220)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 10, 2, SpecialOfferType.FiveForAmount, 145, 350)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 10, 2, SpecialOfferType.FiveForAmount, 185, 450)]
        [InlineData("Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 10, 2, SpecialOfferType.FiveForAmount, 230, 560)]
        #endregion

        #region Item with Two for Amount (ex. Two for 100)
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 1, 1, SpecialOfferType.TwoForAmount, 150, 150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 1, 1, SpecialOfferType.TwoForAmount, 480, 480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 1, 1, SpecialOfferType.TwoForAmount, 500, 500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 1, 1, SpecialOfferType.TwoForAmount, 750, 750)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 1, 1, SpecialOfferType.TwoForAmount, 990, 990)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 1, 1, SpecialOfferType.TwoForAmount, 1000, 1000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 1, 2, SpecialOfferType.TwoForAmount, 150, 250)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 1, 2, SpecialOfferType.TwoForAmount, 480, 680)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 1, 2, SpecialOfferType.TwoForAmount, 500, 800)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 1, 2, SpecialOfferType.TwoForAmount, 750, 1150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 1, 2, SpecialOfferType.TwoForAmount, 990, 1490)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 1, 2, SpecialOfferType.TwoForAmount, 1000, 1600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 2, 2, SpecialOfferType.TwoForAmount, 150, 300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 2, 2, SpecialOfferType.TwoForAmount, 480, 960)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 2, 2, SpecialOfferType.TwoForAmount, 500, 1000)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 2, 2, SpecialOfferType.TwoForAmount, 750, 1500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 2, 2, SpecialOfferType.TwoForAmount, 990, 1980)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 2, 2, SpecialOfferType.TwoForAmount, 1000, 2000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 100, 2, 3, SpecialOfferType.TwoForAmount, 150, 400)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 200, 2, 3, SpecialOfferType.TwoForAmount, 480, 1160)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 300, 2, 3, SpecialOfferType.TwoForAmount, 500, 1300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 400, 2, 3, SpecialOfferType.TwoForAmount, 750, 1900)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 500, 2, 3, SpecialOfferType.TwoForAmount, 990, 2480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Each, 600, 2, 3, SpecialOfferType.TwoForAmount, 1000, 2600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 1, 1, SpecialOfferType.TwoForAmount, 150, 150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 1, 1, SpecialOfferType.TwoForAmount, 480, 480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 1, 1, SpecialOfferType.TwoForAmount, 500, 500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 1, 1, SpecialOfferType.TwoForAmount, 750, 750)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 1, 1, SpecialOfferType.TwoForAmount, 990, 990)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 1, 1, SpecialOfferType.TwoForAmount, 1000, 1000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 1, 2, SpecialOfferType.TwoForAmount, 150, 250)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 1, 2, SpecialOfferType.TwoForAmount, 480, 680)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 1, 2, SpecialOfferType.TwoForAmount, 500, 800)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 1, 2, SpecialOfferType.TwoForAmount, 750, 1150)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 1, 2, SpecialOfferType.TwoForAmount, 990, 1490)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 1, 2, SpecialOfferType.TwoForAmount, 1000, 1600)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 2, 2, SpecialOfferType.TwoForAmount, 150, 300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 2, 2, SpecialOfferType.TwoForAmount, 480, 960)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 2, 2, SpecialOfferType.TwoForAmount, 500, 1000)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 2, 2, SpecialOfferType.TwoForAmount, 750, 1500)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 2, 2, SpecialOfferType.TwoForAmount, 990, 1980)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 2, 2, SpecialOfferType.TwoForAmount, 1000, 2000)]
        /*------------------------------------------------------------*/
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 100, 2, 3, SpecialOfferType.TwoForAmount, 150, 400)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 200, 2, 3, SpecialOfferType.TwoForAmount, 480, 1160)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 300, 2, 3, SpecialOfferType.TwoForAmount, 500, 1300)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 400, 2, 3, SpecialOfferType.TwoForAmount, 750, 1900)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 500, 2, 3, SpecialOfferType.TwoForAmount, 990, 2480)]
        [InlineData("Item with Two for Amount (ex. Two for 100)", ProductUnit.Kilo, 600, 2, 3, SpecialOfferType.TwoForAmount, 1000, 2600)]
        #endregion
        public void Single_Item_With_Additional_Quantity_And_Special_Offers_Test(string name,
                                              ProductUnit unit,
                                              double price,
                                              double quantity,
                                              double additionalQty,
                                              SpecialOfferType offer,
                                              double percentOrAmount,
                                              double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            //Additional Quantity
            cart.AddItemQuantity(item, additionalQty);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(offer, item, percentOrAmount);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
            Assert.Equal(expectedPrice - (price * (quantity + additionalQty)), receipt.GetDiscounts()[0].DiscountAmount);
        }


        [Theory]
        #region Not Qualified Item with Three for Two special offer
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 10, 1, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 20, 1, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 30, 1, 1, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 40, 1, 1, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Each, 50, 1, 1, SpecialOfferType.ThreeForTwo, 0, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 10, 1, 1, SpecialOfferType.ThreeForTwo, 0, 20)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 20, 1, 1, SpecialOfferType.ThreeForTwo, 0, 40)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 30, 1, 1, SpecialOfferType.ThreeForTwo, 0, 60)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 40, 1, 1, SpecialOfferType.ThreeForTwo, 0, 80)]
        [InlineData("Not Qualified Item with Three for Two special offer", ProductUnit.Kilo, 50, 1, 1, SpecialOfferType.ThreeForTwo, 0, 100)]
        #endregion

        #region Not Qualified Item with Five for Amount (ex. Five for 100)
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 1, 1, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 1, 1, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 1, 1, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 1, 1, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 1, 1, SpecialOfferType.FiveForAmount, 100, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 2, 1, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 2, 1, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 2, 1, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 2, 1, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 2, 1, SpecialOfferType.FiveForAmount, 150, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 10, 2, 2, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 20, 2, 2, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 30, 2, 2, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 40, 2, 2, SpecialOfferType.FiveForAmount, 160, 160)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Each, 50, 2, 2, SpecialOfferType.FiveForAmount, 200, 200)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 1, 1, SpecialOfferType.FiveForAmount, 20, 20)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 1, 1, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 1, 1, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 1, 1, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 1, 1, SpecialOfferType.FiveForAmount, 100, 100)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 2, 1, SpecialOfferType.FiveForAmount, 30, 30)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 2, 1, SpecialOfferType.FiveForAmount, 60, 60)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 2, 1, SpecialOfferType.FiveForAmount, 90, 90)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 2, 1, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 2, 1, SpecialOfferType.FiveForAmount, 150, 150)]
        /*------------------------------------------------------------*/
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 10, 2, 2, SpecialOfferType.FiveForAmount, 40, 40)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 20, 2, 2, SpecialOfferType.FiveForAmount, 80, 80)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 30, 2, 2, SpecialOfferType.FiveForAmount, 120, 120)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 40, 2, 2, SpecialOfferType.FiveForAmount, 160, 160)]
        [InlineData("Not Qualified Item with Five for Amount (ex. Five for 100)", ProductUnit.Kilo, 50, 2, 2, SpecialOfferType.FiveForAmount, 200, 200)]
        #endregion

        public void Not_Qualified_Single_Item_With_Additional_Quantity_And_Special_Offer_Test(string name,
                                                                      ProductUnit unit,
                                                                      double price,
                                                                      double quantity,
                                                                      double additionalQty,
                                                                      SpecialOfferType offer,
                                                                      double percentOrAmount,
                                                                      double expectedPrice)
        {
            //Arrange
            ISupermarketCatalog catalog = new FakeCatalog();
            var item = new Product(name, unit);

            catalog.AddProduct(item, price);

            var cart = new ShoppingCart();
            cart.AddItemQuantity(item, quantity);

            //Additional Quantity
            cart.AddItemQuantity(item, additionalQty);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(offer, item, percentOrAmount);

            // Act 
            var receipt = teller.ChecksOutArticlesFrom(cart);

            //Item Assert
            Assert.Equal(item, receipt.GetItems()[0].Product);
            Assert.Equal(unit, receipt.GetItems()[0].Product.Unit);
            Assert.Equal(price, receipt.GetItems()[0].Price);
            Assert.Equal(expectedPrice, receipt.GetTotalPrice());
            Assert.Empty(receipt.GetDiscounts());
        }
    }
}