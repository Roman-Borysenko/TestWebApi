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
    public class CategoryControllerTest
    {
        [Test]
        public void GetAllTest()
        {
            // Arrange
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(r => r.GetAll()).Returns(GetCategories());
            var controller = new CategoryController(mock.Object);
            //Act
            var result = controller.GetAll();
            // Assert
            Assert.AreEqual(mock.Object.GetAll().Result.Count, result.Result.Count());
            Assert.IsTrue(mock.Object.GetAll().Result.TrueForAll(m => result.Result.Any(r => r.Id == m.Id &&
                            r.Name == m.Name && r.Slug == m.Slug)));
        }
        private Task<List<Category>> GetCategories() {
            var categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Category #1", Slug = "category-#1" },
                new Category() { Id = 2, Name = "Category #2", Slug = "category-#2" },
                new Category() { Id = 3, Name = "Category #3", Slug = "category-#3" }
            };

            return Task.FromResult(categories);
        }
        [Test]
        public void GetByIdTest()
        {
            //Arrange
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetCategory());
            var controller = new CategoryController(mock.Object);
            //Act
            var okResult = controller.GetOne(1).Result as OkObjectResult;
            var categoryResult = okResult.Value as Category;
            //Assert
            var mockExpected = mock.Object.GetById(1).Result;
            Assert.NotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsTrue(mockExpected.Id == categoryResult.Id && 
                            mockExpected.Name == categoryResult.Name &&
                            mockExpected.Slug == categoryResult.Slug);
        }
        private Task<Category> GetCategory()
        {
            return Task.FromResult(new Category() { Id = 1, Name = "Category #1", Slug = "category-#1" });
        }
        [Test]
        public void AddRangeTest()
        {
            // Arrange
            var mock = new Mock<ICategoryRepository>();
            var controller = new CategoryController(mock.Object);
            // Act
            var result = controller.AddRange(new List<WebApi.ViewModels.Category>() {
                new WebApi.ViewModels.Category() { Name = "Category-Test" }
            }).Result as OkResult;
            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void AddTest()
        {
            // Arrange
            var mock = new Mock<ICategoryRepository>();
            var controller = new CategoryController(mock.Object);
            // Act
            var result = controller.Add(new WebApi.ViewModels.Category() { Name = "Category-Add-Test" }).Result as OkResult;
            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void EditTest()
        {
            // Arrange
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetCategory());
            var controller = new CategoryController(mock.Object);
            // Act
            var result = controller.Edit(new WebApi.ViewModels.Category() { Id = 1, Name = "Category-Edit-Test" }).Result as OkResult;
            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
        [Test]
        public void DeleteTest()
        {
            // Arrange
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(r => r.GetById(1)).Returns(GetCategory());
            var controller = new CategoryController(mock.Object);
            // Act
            var result = controller.Delete(1).Result as OkResult;
            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
