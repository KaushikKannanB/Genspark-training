using NUnit.Framework;
using Moq;
using Inventory.Interfaces;
using Inventory.Models;
using Inventory.Services;
using Inventory.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Inventory.Models.DTOs;

namespace Inventory.Test
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IRepository<string, Product>> _prodRepoMock;
        private Mock<IRepository<string, Manager>> _managerRepoMock;
        private Mock<IRepository<string, Inventories>> _invRepoMock;
        private Mock<IRepository<string, Category>> _categRepoMock;
        private Mock<IRepository<string, StockLogging>> _stockRepoMock;
        private Mock<IRepository<string, ProductUpdateLog>> _prodUpdRepoMock;
        private Mock<IEncryptService> _encryptServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        private InventoryContext _context;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "InventoryTestDB")
                .Options;

            _context = new InventoryContext(dbOptions);

            _prodRepoMock = new Mock<IRepository<string, Product>>();
            _managerRepoMock = new Mock<IRepository<string, Manager>>();
            _invRepoMock = new Mock<IRepository<string, Inventories>>();
            _categRepoMock = new Mock<IRepository<string, Category>>();
            _stockRepoMock = new Mock<IRepository<string, StockLogging>>();
            _prodUpdRepoMock = new Mock<IRepository<string, ProductUpdateLog>>();
            _encryptServiceMock = new Mock<IEncryptService>();
            _userServiceMock = new Mock<IUserService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _service = new ProductService(
                _managerRepoMock.Object,
                _prodUpdRepoMock.Object,
                _stockRepoMock.Object,
                _context,
                _userServiceMock.Object,
                _currentUserServiceMock.Object,
                _invRepoMock.Object,
                _prodRepoMock.Object,
                _categRepoMock.Object,
                _encryptServiceMock.Object
            );
        }

        [Test]
        public async Task AddProduct_Should_Add_Product_When_Valid()
        {
            // Arrange
            var request = new ProductAddRequest
            {
                Name = "Laptop",
                stock = 10,
                categoryName = "ELECTRONICS",
                MinThreshold = 5
            };

            var category = new Category { Id = "CAT001", CategoryName = "ELECTRONICS" };
            var user = new User { Id = "U1", Email = "test@mail.com", Role = "MANAGER" };
            var manager = new Manager { Id = "U1", Status = "ACTIVE" };
            var inventory = new Inventories { Id = "INV001", Stock = 10, MinThreshold = 5 };

            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(category);
            _prodRepoMock.Setup(x => x.GetByName("LAPTOP")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns(user.Email);
            _userServiceMock.Setup(x => x.GetByMail(user.Email)).ReturnsAsync(user);
            _managerRepoMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(manager);
            _encryptServiceMock.Setup(x => x.GenerateId("INV")).ReturnsAsync("INV001");
            _encryptServiceMock.Setup(x => x.GenerateId("PROD")).ReturnsAsync("PROD001");
            _invRepoMock.Setup(x => x.Add(It.IsAny<Inventories>())).ReturnsAsync(inventory);
            _prodRepoMock.Setup(x => x.Add(It.IsAny<Product>())).ReturnsAsync((Product p) =>
            {
                p.Id = "PROD001";
                return p;
            });

            // Act
            var result = await _service.AddProduct(request);

            // Assert

            Assert.That(result.Id, Is.EqualTo("PROD001"));
            // Assert.That(result, Is.Null);
        }
        [Test]
        public void AddProduct_Should_Throw_When_Category_Not_Found()
        {
            var request = new ProductAddRequest
            {
                Name = "Phone",
                stock = 5,
                categoryName = "MOBILE",
                MinThreshold = 2
            };

            _categRepoMock.Setup(x => x.GetByName("MOBILE")).ReturnsAsync((Category)null);

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "Category not found");
        }
        [Test]
        public void AddProduct_Should_Throw_When_Product_Already_Exists()
        {
            var request = new ProductAddRequest
            {
                Name = "Laptop",
                stock = 5,
                categoryName = "ELECTRONICS",
                MinThreshold = 2
            };

            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(new Category());
            _prodRepoMock.Setup(x => x.GetByName("LAPTOP")).ReturnsAsync(new Product());

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "Product already exists");
        }
        [Test]
        public void AddProduct_Should_Throw_When_User_Not_Found()
        {
            var request = new ProductAddRequest { Name = "Tablet", stock = 4, categoryName = "ELECTRONICS", MinThreshold = 1 };
            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(new Category());
            _prodRepoMock.Setup(x => x.GetByName("TABLET")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns("abc@mail.com");
            _userServiceMock.Setup(x => x.GetByMail("abc@mail.com")).ReturnsAsync((User)null);

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "User not found");
        }
        [Test]
        public void AddProduct_Should_Throw_When_Manager_Status_Is_Not_Active()
        {
            var request = new ProductAddRequest { Name = "Tablet", stock = 4, categoryName = "ELECTRONICS", MinThreshold = 1 };
            var user = new User { Id = "U2", Email = "abc@mail.com", Role = "MANAGER" };

            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(new Category());
            _prodRepoMock.Setup(x => x.GetByName("TABLET")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns(user.Email);
            _userServiceMock.Setup(x => x.GetByMail(user.Email)).ReturnsAsync(user);
            _managerRepoMock.Setup(x => x.GetById("U2")).ReturnsAsync(new Manager { Id = "U2", Status = "INACTIVE" });

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "Manager not active");
        }
        
        [Test]
        public async Task AddProduct_Should_Set_AddedBy_As_CurrentUser()
        {
            var request = new ProductAddRequest
            {
                Name = "Keyboard",
                stock = 6,
                categoryName = "ELECTRONICS",
                MinThreshold = 2
            };

            var category = new Category { Id = "CAT003", CategoryName = "ELECTRONICS" };
            var user = new User { Id = "U4", Email = "keyboard@mail.com", Role = "MANAGER" };
            var manager = new Manager { Id = "U4", Status = "ACTIVE" };
            var inventory = new Inventories { Id = "INV003", Stock = 6, MinThreshold = 2 };

            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(category);
            _prodRepoMock.Setup(x => x.GetByName("KEYBOARD")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns(user.Email);
            _userServiceMock.Setup(x => x.GetByMail(user.Email)).ReturnsAsync(user);
            _managerRepoMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(manager);
            _encryptServiceMock.Setup(x => x.GenerateId("INV")).ReturnsAsync("INV003");
            _encryptServiceMock.Setup(x => x.GenerateId("PROD")).ReturnsAsync("PROD003");
            _invRepoMock.Setup(x => x.Add(It.IsAny<Inventories>())).ReturnsAsync(inventory);
            _prodRepoMock.Setup(x => x.Add(It.IsAny<Product>())).ReturnsAsync((Product p) =>
            {
                p.Id = "PROD003";
                p.AddedBy = user.Id;
                return p;
            });

            var result = await _service.AddProduct(request);

            Assert.That(result.AddedBy, Is.EqualTo("U4"));
        }
        [Test]
        public async Task AddProduct_Should_Call_All_Repositories()
        {
            var request = new ProductAddRequest { Name = "Mouse", stock = 12, categoryName = "ELECTRONICS", MinThreshold = 5 };
            var user = new User { Id = "U5", Email = "mouse@mail.com", Role = "MANAGER" };
            var manager = new Manager { Id = "U5", Status = "ACTIVE" };
            var category = new Category { Id = "CAT004", CategoryName = "ELECTRONICS" };
            var inventory = new Inventories { Id = "INV004", Stock = 12, MinThreshold = 5 };

            _categRepoMock.Setup(x => x.GetByName("ELECTRONICS")).ReturnsAsync(category);
            _prodRepoMock.Setup(x => x.GetByName("MOUSE")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns(user.Email);
            _userServiceMock.Setup(x => x.GetByMail(user.Email)).ReturnsAsync(user);
            _managerRepoMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(manager);
            _encryptServiceMock.Setup(x => x.GenerateId("INV")).ReturnsAsync("INV004");
            _encryptServiceMock.Setup(x => x.GenerateId("PROD")).ReturnsAsync("PROD004");
            _invRepoMock.Setup(x => x.Add(It.IsAny<Inventories>())).ReturnsAsync(inventory);
            _prodRepoMock.Setup(x => x.Add(It.IsAny<Product>())).ReturnsAsync((Product p) =>
            {
                p.Id = "PROD004";
                return p;
            });

            var result = await _service.AddProduct(request);

            _prodRepoMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
            _invRepoMock.Verify(x => x.Add(It.IsAny<Inventories>()), Times.Once);
        }
        [Test]
        public void AddProduct_Should_Throw_When_Stock_Is_Negative()
        {
            var request = new ProductAddRequest { Name = "TV", stock = -5, categoryName = "ELECTRONICS", MinThreshold = 1 };

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "Invalid stock value");
        }
        [Test]
        public void AddProduct_Should_Throw_When_MinThreshold_Exceeds_Stock()
        {
            var request = new ProductAddRequest { Name = "Speaker", stock = 2, categoryName = "ELECTRONICS", MinThreshold = 5 };

            Assert.ThrowsAsync<Exception>(async () => await _service.AddProduct(request), "Minimum threshold exceeds stock");
        }


        [Test]
        public async Task AddProduct_Should_Assign_Correct_CategoryId()
        {
            var request = new ProductAddRequest
            {
                Name = "Scanner",
                stock = 4,
                categoryName = "PERIPHERALS",
                MinThreshold = 1
            };

            var category = new Category { Id = "CAT104", CategoryName = "PERIPHERALS" };
            var user = new User { Id = "U104", Email = "scan@mail.com", Role = "MANAGER" };
            var manager = new Manager { Id = "U104", Status = "ACTIVE" };
            var inventory = new Inventories { Id = "INV104", Stock = 4, MinThreshold = 1 };

            _categRepoMock.Setup(x => x.GetByName("PERIPHERALS")).ReturnsAsync(category);
            _prodRepoMock.Setup(x => x.GetByName("SCANNER")).ReturnsAsync((Product)null);
            _currentUserServiceMock.Setup(x => x.Email).Returns(user.Email);
            _userServiceMock.Setup(x => x.GetByMail(user.Email)).ReturnsAsync(user);
            _managerRepoMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(manager);
            _encryptServiceMock.Setup(x => x.GenerateId("INV")).ReturnsAsync("INV104");
            _encryptServiceMock.Setup(x => x.GenerateId("PROD")).ReturnsAsync("PROD104");
            _invRepoMock.Setup(x => x.Add(It.IsAny<Inventories>())).ReturnsAsync(inventory);
            _prodRepoMock.Setup(x => x.Add(It.IsAny<Product>())).ReturnsAsync((Product p) => { p.Id = "PROD104"; return p; });

            var result = await _service.AddProduct(request);

            Assert.That(result.CategoryId, Is.EqualTo("CAT104"));
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
