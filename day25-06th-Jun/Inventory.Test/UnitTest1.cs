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

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
