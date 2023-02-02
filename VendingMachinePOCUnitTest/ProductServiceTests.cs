using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VendingMachinePOC;
using VendingMachinePOCUnitTest.Helpers;

namespace VendingMachinePOCUnitTest
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> productRepository;
        private Mock<IProductInventoryRepository> productInventoryRepository;

        [TestInitialize]
        public void Initialise()
        {
            productRepository = new Mock<IProductRepository>();
            productInventoryRepository = new Mock<IProductInventoryRepository>();
        }

        [TestMethod]
        public void ProductServiceProductRepositoryIsNullExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(null, productInventoryRepository.Object), "Value cannot be null.\r\nParameter name: productRepository parameter is null");
        }

        [TestMethod]
        public void ProductServiceProductInventoryRepositoryIsNullExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(productRepository.Object, null), "Value cannot be null.\r\nParameter name: productInventoryRepository parameter is null");
        }

        [TestMethod]
        public void ProductServiceProductRepositoryGetAllProductsIsCalledVerifyGetProductListIsCalledOnce()
        {
            // Arrange
            productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(productRepository.Object,productInventoryRepository.Object);

            var result = productService.GetAllProducts();

            // Assert
            productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [TestMethod]
        public void ProductServiceGetProductIsCallledVerifyGetProductListIsCalledOnce()
        {
            // Arrange
            productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(productRepository.Object, productInventoryRepository.Object);

            var result = productService.GetProduct(It.IsAny<string>());

            // Assert
            productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [TestMethod]
        public void ProductServiceGetProductIsValidProductReturned()
        {
            // Arrange
            productRepository.Setup(mock => mock.GetProductList()).Returns(CreateProducts());

            // Act
            var productService = new ProductService(productRepository.Object, productInventoryRepository.Object);

            var result = productService.GetProduct("COLA1");

            // Assert
           
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Code, "COLA1");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Name, "Cola");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Price, 1.00m);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result.Type, ProductItemType.Cola);
        }

        [TestMethod]
        public void ProductServiceGetProductQuantityIsValidProductQuantityReturned()
        {
            // Arrange
            productInventoryRepository.Setup(mock => mock.GetInventory()).Returns(CreateProductInventory());

            // Act
            var productService = new ProductService(productRepository.Object, productInventoryRepository.Object);

            var result = productService.GetProductQuantity("COLA1");

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void ProductServiceUpdateProductQuantityVerifyUpdateInventoryIsCalledOnce()
        {
            // Arrange
            productInventoryRepository.Setup(mock => mock.UpdateInventory(It.IsAny<string>()));

            // Act
            var productService = new ProductService(productRepository.Object, productInventoryRepository.Object);

            productService.UpdateProductQuantity("COLA1");

            // Assert
            productInventoryRepository.Verify(mock => mock.UpdateInventory(It.IsAny<string>()), Times.Once);
        }

        private List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Code = "COLA1",
                    Name = "Cola",
                    Price = 1.00m,
                    Type = ProductItemType.Cola
                }
            };
        }

        private Dictionary<string, int> CreateProductInventory()
        {
            return new Dictionary<string, int> { { "COLA1", 2 }, { "CHIPS1", 0 }, { "CANDY1", 10 } };
        }
    }
}
