using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApiTest
{
    public class ProductControllerTest
    {
        [Test]
        public void GetAllProductsTest()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.GetAll()).Returns(GetProducts());
            var controller = new ProductController(mock.Object);
            // Act
            var result = controller.GetAllProducts().Result;
            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(mock.Object.GetAll().Result.Count, result.Count());
            Assert.IsTrue(mock.Object.GetAll().Result.TrueForAll(m => result.Any(r => r.Id == m.Id &&
                r.Name == m.Name && r.Slug == m.Slug && r.CategoryId == m.CategoryId && r.Description == m.Description &&
                r.Price == m.Price && r.Quantity == m.Quantity)));
        }
        private Task<List<Product>> GetProducts()
        {
            var products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Product #1", Slug = "product-#1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 },
                new Product() { Id = 2, Name = "Product #2", Slug = "product-#2", CategoryId = 2, Description = "Product #2 Description", Price = 22, Quantity = 222 },
                new Product() { Id = 3, Name = "Product #3", Slug = "product-#3", CategoryId = 3, Description = "Product #3 Description", Price = 33, Quantity = 333 },
                new Product() { Id = 4, Name = "Product #4", Slug = "product-#4", CategoryId = 1, Description = "Product #4 Description", Price = 44, Quantity = 444 }
            };

            return Task.FromResult(products);
        }
        [Test]
        public void GetProductTest() 
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetProduct());
            var controller = new ProductController(mock.Object);
            // Act
            var okResult = controller.GetProduct(1).Result as OkObjectResult;
            var productResult = okResult.Value as Product;
            // Assert
            var expexted = mock.Object.GetById(1).Result;
            Assert.NotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsTrue(expexted.Id == productResult.Id && expexted.Name == productResult.Name &&
                expexted.Slug == productResult.Slug && expexted.CategoryId == productResult.CategoryId &&
                expexted.Description == productResult.Description && expexted.Price == productResult.Price &&
                expexted.Quantity == productResult.Quantity);
        }
        private Task<Product> GetProduct()
        {
            return Task.FromResult(new Product() { Id = 1, Name = "Product #1", Slug = "product-#1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 });
        }
        [Test]
        public void GetProductsByCategoryTest()
        {
            int id = 1;
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.GetByCategory(id)).Returns(GetProducts());
            var controller = new ProductController(mock.Object);
            // Act
            var okResult = controller.GetProductsByCategory(id).Result as OkObjectResult;
            var productsResult = okResult.Value as List<Product>;
            //Assert
            Assert.NotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(mock.Object.GetByCategory(id).Result.Count, productsResult.Count());
            Assert.IsTrue(mock.Object.GetByCategory(id).Result.TrueForAll(m => productsResult.Any(r => r.Id == m.Id &&
                r.Name == m.Name && r.Slug == m.Slug && r.CategoryId == m.CategoryId && r.Description == m.Description &&
                r.Price == m.Price && r.Quantity == m.Quantity)));
        }
        [Test]
        public void AddRangeTest()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            var controller = new ProductController(mock.Object);
            // Act
            var result = controller.AddRange(new List<WebApi.ViewModels.Product>() {
                new WebApi.ViewModels.Product() { Id = 1, Name = "Product #1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 },
                new WebApi.ViewModels.Product() { Id = 2, Name = "Product #2", CategoryId = 2, Description = "Product #2 Description", Price = 22, Quantity = 222 }
            }).Result as OkResult;
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void AddTest()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            var controller = new ProductController(mock.Object);
            // Act
            var result = controller.Add(new WebApi.ViewModels.Product() { Id = 1, Name = "Product #1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 }).Result as OkResult;
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void EditTest()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetProduct());
            var controller = new ProductController(mock.Object);
            // Act
            var result = controller.Edit(new WebApi.ViewModels.Product() { Id = 1, Name = "Product #1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 }).Result as OkResult;
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void DeleteTest()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetProduct());
            var controller = new ProductController(mock.Object);
            // Act
            var result = controller.Delete(1).Result as OkResult;
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
