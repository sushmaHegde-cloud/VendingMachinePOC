using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using VendingMachinePOC;
using VendingMachinePOC.Interfaces;
using VendingMachinePOCUnitTest.Helpers;

namespace VendingMachinePOCUnitTest
{
    [TestClass]
    public class VendingMachineTests
    {
        private Mock<IProductService> productService;
        private Mock<ICoinService> coinService;

        [TestInitialize]
        public void Initialise()
        {
            productService = new Mock<IProductService>();
            coinService = new Mock<ICoinService>();
        }

        [TestMethod]
        public void VendingMachineCoinServiceIsNullExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachinePOC.VendingMachinePOC(null, productService.Object), "Value cannot be null.\r\nParameter name: coinService parameter is null");
        }

        [TestMethod]
        public void VendingMachineProductServiceIsNullExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachinePOC.VendingMachinePOC(coinService.Object, null), "Value cannot be null.\r\nParameter name: productService parameter is null");
        }

        [TestMethod]
        public void VendingMachineAcceptCoinNullCoinEntered()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.AcceptCoin(null), "Value cannot be null.\r\nParameter name: Coin parameter null!");
        }

        [TestMethod]
        public void VendingMachineAcceptCoinInvalidCoinEntered()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsRejectedCoin, true);
        }

        [TestMethod]
        public void VendingMachineAcceptCoinValidCoinEntered()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFivePenceCoin);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsRejectedCoin, false);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "0.05");

        }

        [TestMethod]
        public void VendingMachineAcceptCoinVerifyCoinServiceIsCalledOnce()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            coinService.Verify(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachineSelectProductInvalidCodeExceptionThrown()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.SelectProduct(""), "Value cannot be null.\r\nParameter name: Code parameter empty!");
        }

        [TestMethod]
        public void VendingMachineSelectProductInvalidCodeEntered()
        {
            // Arrange
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, false);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "Invalid Product Selected. Please try again");
        }

        [TestMethod]
        public void VendingMachineSelectProductInvalidCodeEnteredVerifyGetProductCalledOnce()
        {
            // Arrange
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachineSelectProductNoCoinsEntered()
        {
            // Arrange
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, false);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "Insert Coin");
        }

        [TestMethod]
        public void VendingMachineSelectProductNoCoinsEnteredVerifyGetProductCalledOnce()
        {
            // Arrange
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredLessThanProductPrice()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFivePenceCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, false);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, string.Format("Price : {0}", 1.00m));
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredIsValid()
        {
            // Arrange
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "Thank You");
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredAndReturnCoinsIsValid()
        {
            // Arrange            
            coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "Thank You");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyPence) == null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FiftyPence) == null, true);
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredAndReturnMoreThanOneCoinIsValid()
        {
            // Arrange            
            coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            coinService.Setup(mock => mock.GetCoin(5.0m, 21.4m, 1.7m)).Returns(CreateValidAcceptedTwentyPenceCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyPenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.IsSuccess, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Message, "Thank You");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyPence).Number, 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FiftyPence) == null, true);
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredAndReturnedVerifyGetProductQuantityCalledOnce()
        {
            // Arrange            
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            productService.Verify(mock => mock.GetProductQuantity(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachineSelectProductCoinsEnteredAndReturnedVerifyUpdateProductQuantityCalledOnce()
        {
            // Arrange            
            coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);
            productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert           
            productService.Verify(mock => mock.UpdateProductQuantity(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachineReturnCoinsCoinsEnteredAndReturnedIsValid()
        {
            // Arrange            
            coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            coinService.Setup(mock => mock.GetCoin(5.0m, 21.4m, 1.7m)).Returns(CreateValidAcceptedTwentyPenceCoin);

            // Act
            var vendingMachine = new VendingMachinePOC.VendingMachinePOC(coinService.Object, productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyPenceCoin());

            var result = vendingMachine.ReturnCoins();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result != null, true);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.TwentyPence).Number, 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.OnePound).Number, 1);
        }

        private InputCoin CreateValidFivePenceCoin()
        {
            return new InputCoin()
            {
                Diameter = 18.0m,
                Thickness = 1.7m,
                Weight = 3.25m
            };
        }

        private InputCoin CreateValidTwentyPenceCoin()
        {
            return new InputCoin()
            {
                Diameter = 21.4m,
                Thickness = 1.7m,
                Weight = 5.0m
            };
        }
        private InputCoin CreateValidOnePoundCoin()
        {
            return new InputCoin()
            {
                Diameter = 22.5m,
                Thickness = 3.15m,
                Weight = 9.5m
            };
        }
        private InputCoin CreateInvalidCoin()
        {
            return new InputCoin()
            {
                Diameter = 999m,
                Thickness = 999m,
                Weight = 999m
            };
        }

        private ValidCoin CreateValidAcceptedFivePenceCoin()
        {
            return new ValidCoin()
            {
                Diameter = 18.0m,
                Thickness = 1.7m,
                Weight = 3.25m,
                Type = CoinType.FivePence,
                Value = 0.05m
            };
        }

        private ValidCoin CreateValidAcceptedTwentyPenceCoin()
        {
            return new ValidCoin()
            {
                Diameter = 21.4m,
                Thickness = 1.7m,
                Type = CoinType.TwentyPence,
                Weight = 5.0m,
                Value = 0.20m
            };
        }

        private ValidCoin CreateValidAcceptedOnePoundCoin()
        {
            return new ValidCoin()
            {
                Diameter = 22.5m,
                Thickness = 3.15m,
                Type = CoinType.OnePound,
                Weight = 9.5m,
                Value = 1.00m
            };
        }

        private Product CreateProduct()
        {
            return new Product
            {
                Name = "COLA",
                Code = "COLA1",
                Price = 1.00m,
                Type = ProductItemType.Cola
            };
        }
    }
}
