using System;
using System.Collections.Generic;
using System.Linq;
using Application.XUnit.Base;
using Domain.Entities;
using Infrastructure.Services;
using Xunit;

namespace Application.XUnit.Service
{
    public class TestShoppingBasketService : TestBase
    {
        [Fact]
        public void Add_IsNull_ExceptionThrown()
        {
            var sut = new ShoppingBasketService(_productService.Object);
            // Assert
            Assert.Throws<NullReferenceException>(() => sut.AddProducts(null));
        }

        [Theory, AutoMoqData]
        public void Can_Add_One_Product(Product product)
        {
            // Arrange
            var productName = product.Name;
            var productNames = new string[]{productName}; 
            _productService.Setup(x=>x.GetProductByName(productName)).Returns(product);
            var sut = new ShoppingBasketService(_productService.Object);

            // Act
            var result = sut.AddProducts(productNames);

            // Assert
            Assert.Equal(result.Count, 1);
        }

        [Theory, AutoMoqData]
        public void AddProducts_And_CheckProductsCount(List<Product> products)
        {
            // Arrange
            var productNames = products.Select(x=>x.Name).ToArray();
            
            foreach (var item in products)
            {
                _productService.Setup(x=>x.GetProductByName(item.Name)).Returns(item);
            }
            
            var sut = new ShoppingBasketService(_productService.Object);

            // Act
            var result = sut.AddProducts(productNames);

            // Assert
            Assert.Equal(result.Count, products.Count);
        }

        [Theory, AutoMoqData]
        public void GetProductByName_Is_Null_ExceptionThrown(Product product,string exeptedProductName)
        {
            // Arrange
            var productName = product.Name;
            var productNames = new string[]{exeptedProductName}; 
            _productService.Setup(x=>x.GetProductByName(productName)).Returns(product);
            var sut = new ShoppingBasketService(_productService.Object);

            // Assert
            Assert.Throws<NotSupportedException>(() => sut.AddProducts(productNames));
        }

        [Theory, AutoMoqData]
        public void Add_Product_And_Check_Quantity(Product product)
        {
            // Arrange
            var productName = product.Name;
            var productNames = new string[]{productName,productName}; 
            _productService.Setup(x=>x.GetProductByName(productName)).Returns(product);
            var sut = new ShoppingBasketService(_productService.Object);

            // Act
            var result = sut.AddProducts(productNames);
            var productQuantity = result.SingleOrDefault(x=>x.Product.Name == productName);

            // Assert
            Assert.Equal(productQuantity.Quantity, 2);
        }

    }
}