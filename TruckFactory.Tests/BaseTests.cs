using Microsoft.EntityFrameworkCore;
using System;
using TruckFactory.Web.Application.Shared;
using TruckFactory.Web.Controllers;
using TruckFactory.Web.Domain;
using TruckFactory.Web.Domain.Data;

namespace TruckFactory.Tests
{
    public class BaseTests
    {
        protected DbContextOptions<FactoryContext> _contextOptions;

        public BaseTests(DbContextOptions<FactoryContext> ctxOpts)
        {
            _contextOptions = ctxOpts;

            Seed();
        }

        private void Seed()
        {
            using var context = new FactoryContext(_contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var modelFH = new TruckModelEntity
            {
                Id = 11,
                Name = "Model FH",
                EnumValue = (short)EnumTruckModel.FH
            };
            var modelFM = new TruckModelEntity
            {
                Id = 22,
                Name = "Model FM",
                EnumValue = (short)EnumTruckModel.FM
            };
            var truck1 = new TruckEntity
            {
                Id = 33,
                Name = "Volvo 33",
                ModelYear = 2021,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFH.Id
            };
            var truck2 = new TruckEntity
            {
                Id = 44,
                Name = "Volvo 44",
                ModelYear = 2021,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFH.Id
            };
            var truck3 = new TruckEntity
            {
                Id = 55,
                Name = "Volvo 55",
                ModelYear = 2022,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFM.Id
            };

            context.AddRange(modelFH, modelFM);
            context.AddRange(truck1, truck2, truck3);

            context.SaveChanges();
        }

    }
}