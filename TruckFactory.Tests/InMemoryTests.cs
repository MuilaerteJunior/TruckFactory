using Microsoft.EntityFrameworkCore;
using System.Linq;
using TruckFactory.Web.Application.Services;
using TruckFactory.Web.Application.Shared;
using TruckFactory.Web.Domain;
using TruckFactory.Web.Models;
using Xunit;

namespace TruckFactory.Tests
{
    public class InMemoryTests : BaseTests
    {
        public InMemoryTests() : base(new DbContextOptionsBuilder<FactoryContext>()
                                            .UseInMemoryDatabase("Testing")
                                            .Options)
        {
        }

        [Fact]
        public void Validate_GET()
        {
            //Assign - Act - Assert
            using var context = new FactoryContext(_contextOptions);
            var tService = new TruckService(context);

            var item = tService.Get(33);

            Assert.True(item != null);
            if (item != null)
            {
                Assert.Equal("Volvo 33", item.Name);
                Assert.Equal(2021, item.ModelYear);
                Assert.Equal(2021, item.ProductionYear);
            }
        }
        [Fact]
        public void Validate_Create()
        {
            //Assign - Act - Assert
            using var context = new FactoryContext(_contextOptions);
            var tService = new TruckService(context);
            var tFormModel = new TruckFormModel
            {
                Model = EnumTruckModel.FH,
                ModelYear = 2050,
                ProductionYear = 2090,
                Name = "NEW TRUCK"
            };

            var newId = tService.Save(tFormModel);

            var entityCreated = tService.Get(newId);
            Assert.NotNull(tFormModel);
            Assert.NotNull(entityCreated);

            if (entityCreated != null)
            {
                Assert.Equal(tFormModel.Name, entityCreated.Name);
                Assert.Equal(tFormModel.ModelYear, entityCreated.ModelYear);
                Assert.NotEqual(tFormModel.ProductionYear, entityCreated.ProductionYear);
                Assert.Equal(2023, entityCreated.ProductionYear);
                Assert.Equal(newId, entityCreated.Id);
                Assert.Equal(tFormModel.Model, entityCreated.Model);
            }
        }
        [Fact]
        public void Validate_Delete()
        {
            //Assign - Act - Assert
            using var context = new FactoryContext(_contextOptions);
            var tService = new TruckService(context);
            var id = 33;
            var a = tService.Get(id);
            Assert.NotNull(a);

            var success = tService.Delete(id);
            Assert.True(success);

            var b = tService.Get(id);
            Assert.Null(b);
        }
        [Fact]
        public void Validate_Rules()
        {
            //Assign - Act - Assert
            using var context = new FactoryContext(_contextOptions);
            var tService = new TruckService(context);

            var tFormModel = new TruckFormModel
            {
                Id = -1,
                Model = EnumTruckModel.FH,
                ModelYear = 1900,
                ProductionYear = 2000,
                Name = ""//this is a interesting point from service perspective
            };

            var item = tService.Validate(tFormModel);

            Assert.True(item.ErrorCount > 0);
            if (item.ContainsKey("ModelYear"))
            {
                var modelYear = item["ModelYear"];
                if (modelYear != null)
                    Assert.True(modelYear.Errors.Any());
            }

            if (item.ContainsKey("ProductionYear"))
            {
                var productionYear = item["ProductionYear"];
                if (productionYear != null)
                    Assert.True(productionYear.Errors.Any());
            }
        }
    }
}