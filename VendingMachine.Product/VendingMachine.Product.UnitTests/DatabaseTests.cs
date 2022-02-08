using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using VendingMachine.ProductCtx.Context;

namespace VendingMachine.UnitTests
{
    public class DatabaseTests
    {
        private DbContextOptions<ProductContext> dbContextOptions = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: "ProductDatabaseUnitTest2")
            .Options;

        private ProductContext _productContext;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(1 == 1);
        }
    }
}