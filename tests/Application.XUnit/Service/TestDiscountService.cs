using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Application.Products.Disconts;
using Application.XUnit.Base;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services;
using Xunit;

namespace Application.XUnit.Service
{
    public class TestDiscountService: TestBase
    {
        [Fact]
        public void Add_IsNull_ExceptionThrown()
        {
            var sut = new DiscountService();
            // Assert
            Assert.Throws<NullReferenceException>(() => sut.CalculateBasketDiscounts(null,null));
        }

                
        [Fact]
        public void PercentageDiscount_CalculateAppliedDiscount_With10PercentDiscount()
        {
            
            var basket = new List<ProductQuantity>();
            var discounts = new List<IDiscount>();
            var product = new Product(){
                Id = 1,
                Name = "Apples",
                Price = 100m
            };
        
            var productQuantity = new ProductQuantity(){
                Quantity = 1,
                Product  = product
            };
            basket.Add(productQuantity);

            var discountRate = 0.10m;
            var percentageDiscount = new PercentageDiscount(product,discountRate);
            discounts.Add(percentageDiscount);

            var sut = new DiscountService();

            // Act
            var result = sut.CalculateBasketDiscounts(basket,discounts);
            
            // Assert
            Assert.Equal(result.Any(), true);
            Assert.Equal(result[0].Type, DiscountType.Percentage);
            Assert.Equal(result[0].Amount, 10.00m);
            Assert.Equal(result[0].Description, "Apples 10% OFF: - 10p");
        }


        [Theory, AutoMoqData]
        public void RandomProductFor_PercentageDiscount_CalculateAppliedDiscount_With10PercentDiscount(List<Product> products , PercentageDiscount percentageDiscount)
        {
            var discountRate = 0.10m;
            // Arrange
            percentageDiscount._percentage = discountRate;
            var productNames = products.Select(x=>x.Name).ToList();
            productNames.Add(percentageDiscount._discountedProduct.Name);
            
            var disconts = new List<IDiscount>();
            disconts.Add(percentageDiscount);
            
            _productService.Setup(x=>x.GetProductByName(percentageDiscount._discountedProduct.Name)).Returns(percentageDiscount._discountedProduct);

            foreach (var item in products)
            {
                _productService.Setup(x=>x.GetProductByName(item.Name)).Returns(item);
            }
            
            var sut = new ShoppingBasketService(_productService.Object);
            var basket = sut.AddProducts(productNames.ToArray());
            var sutD = new DiscountService();

            // Act
            var result = sutD.CalculateBasketDiscounts(basket,disconts);
            
            var subTotalPrice = basket.Sum(item => item.Product.Price * item.Quantity);
            var totalPrice = subTotalPrice - result.Sum(item => item.Amount);
            var discontMessage = string.Join(" --- ", result.Select(x => x.Description));

            // Assert
            Assert.Equal(subTotalPrice - totalPrice, percentageDiscount._discountedProduct.Price * percentageDiscount._percentage);
        }


        [Fact]
        public void PercentageDiscount_CalculateAppliedDiscount_HalfPriceDiscount()
        {
            
            var basket = new List<ProductQuantity>();
            var discounts = new List<IDiscount>();
            var product = new Product(){
                Id = 1,
                Name = "Apples",
                Price = 100m
            };
        
            var productQuantity = new ProductQuantity(){
                Quantity = 1,
                Product  = product
            };
            basket.Add(productQuantity);

            var percentageDiscount = new HalfPriceDiscount(product,productQuantity);
            discounts.Add(percentageDiscount);

            var sut = new DiscountService();

            // Act
            var result = sut.CalculateBasketDiscounts(basket,discounts);
            
            // Assert
            Assert.Equal(result.Any(), true);
            Assert.Equal(result[0].Type, DiscountType.HalfPrice);
            Assert.Equal(result[0].Amount, 50.00m);
            Assert.Equal(result[0].Description, "Apples 50% OFF: - 50p");
        }


        [Theory, AutoMoqData]
        public void RandomProductFor_PercentageDiscount_CalculateAppliedDiscount_HalfPriceDiscount(List<Product> products , HalfPriceDiscount halfPriceDiscount)
        {
            var discountRate = 0.5m;
            // Arrange
            halfPriceDiscount._productsThatQualifyBasketforDiscount.Quantity = 1;
            var productNames = products.Select(x=>x.Name).ToList();
            productNames.Add(halfPriceDiscount._discountedProduct.Name);
            productNames.Add(halfPriceDiscount._productsThatQualifyBasketforDiscount.Product.Name);
            
            var disconts = new List<IDiscount>();
            disconts.Add(halfPriceDiscount);
            
            _productService.Setup(x=>x.GetProductByName(halfPriceDiscount._discountedProduct.Name)).Returns(halfPriceDiscount._discountedProduct);
            _productService.Setup(x=>x.GetProductByName(halfPriceDiscount._productsThatQualifyBasketforDiscount.Product.Name)).Returns(halfPriceDiscount._productsThatQualifyBasketforDiscount.Product);

            foreach (var item in products)
            {
                _productService.Setup(x=>x.GetProductByName(item.Name)).Returns(item);
            }
            
            var sut = new ShoppingBasketService(_productService.Object);
            var basket = sut.AddProducts(productNames.ToArray());
            var sutD = new DiscountService();

            // Act
            var result = sutD.CalculateBasketDiscounts(basket,disconts);
            
            var subTotalPrice = basket.Sum(item => item.Product.Price * item.Quantity);
            var totalPrice = subTotalPrice - result.Sum(item => item.Amount);
            var discontMessage = string.Join(" --- ", result.Select(x => x.Description));

            // Assert
            Assert.Equal(subTotalPrice - totalPrice, halfPriceDiscount._discountedProduct.Price * discountRate);
        }
    }
}