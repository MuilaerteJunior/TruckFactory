using TruckFactory.Web.Domain.Data;

namespace TruckFactory.Web.Domain
{
    public class FactoryContext : DbContext
    {

        public FactoryContext(DbContextOptions<FactoryContext>  opts) : base(opts)
        {
#if DEBUG
            base.Database.EnsureCreated();
#endif
        }

        public DbSet<TruckEntity> Trucks { get; set; } = null!;
        public DbSet<TruckModelEntity> TruckModels { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder opts)
        {
            base.OnConfiguring(opts);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("dbo");//database objects
            modelBuilder.Entity<TruckEntity>().HasKey(t => t.Id).HasName("PK_TRUCK");
            modelBuilder.Entity<TruckEntity>().Property(t => t.Name).IsRequired().HasMaxLength(200).HasColumnName("NAME");
            modelBuilder.Entity<TruckEntity>().Property(t => t.ModelYear).IsRequired().HasColumnName("MODEL_YEAR");
            modelBuilder.Entity<TruckEntity>().Property(t => t.ProductionYear).IsRequired().HasColumnName("PRODUCTION_YEAR");
            modelBuilder.Entity<TruckEntity>()
                                            .HasOne(t => t.Model)
                                            .WithMany(tm => tm.Trucks)
                                            .HasConstraintName("FK_TRUCK_MODEL")
                                            .HasForeignKey("ID_TRUCK_MODEL")
                                            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TruckModelEntity>().HasKey(t => t.Id).HasName("PK_TRUCK_MODEL");
            modelBuilder.Entity<TruckModelEntity>().Property(t => t.Name).IsRequired().HasMaxLength(200).HasColumnName("NAME");
            modelBuilder.Entity<TruckModelEntity>().Property(t => t.EnumValue).HasColumnName("ENUM_VALUE");

            modelBuilder.Entity<TruckEntity>().ToTable("T_TRUCK").HasComment("Truck table");
            modelBuilder.Entity<TruckModelEntity>().ToTable("T_TRUCK_MODEL").HasComment("Truck model's table");



#if DEBUG
            var modelFH = new TruckModelEntity
            {
                Id = 1,
                Name = "Model FH",
                EnumValue = (short)Application.Shared.EnumTruckModel.FH
            };
            var modelFM = new TruckModelEntity
            {
                Id = 2,
                Name = "Model FM",
                EnumValue = (short)Application.Shared.EnumTruckModel.FM
            };
            modelBuilder.Entity<TruckModelEntity>().HasData(modelFH, modelFM);
            modelBuilder.Entity<TruckEntity>().HasData(new 
            {
                Id = 1,
                Name = "Volvo 1",
                ModelYear = 2021,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFH.Id

            }, new 
            {
                Id = 2,
                Name = "Volvo 2",
                ModelYear = 2021,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFH.Id
            }, new 
            {
                Id = 3,
                Name = "Volvo 3",
                ModelYear = 2022,
                ProductionYear = 2021,
                ID_TRUCK_MODEL = modelFM.Id
            });
#endif
        }
    }
}
