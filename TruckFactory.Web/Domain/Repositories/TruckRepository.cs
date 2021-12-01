using TruckFactory.Web.Domain.Data;

namespace TruckFactory.Web.Domain.Repositories
{
    public class TruckRepository
    {
        private FactoryContext _db;

        public TruckRepository(FactoryContext db)
        {
            _db = db;
        }

        public int Save(TruckEntity entity)
        {
            _db.Trucks.Add(entity);
            _db.SaveChanges();

            return entity.Id.Value;
        }

        public TruckEntity Get(int id)
        {
            return _db.Trucks
                    .Include(a => a.Model)//This could be done using lazy loading
                    .FirstOrDefault(t => t.Id == id);
        }

        internal bool Delete(int id)
        {
            var truck = _db.Trucks.FirstOrDefault(t => t.Id == id);
            _db.Trucks.Remove(truck);
            _db.SaveChanges();
            return true;
        }

        internal IEnumerable<TruckEntity> GetAll()
        {
            //todo: use pagination here
            return _db.Trucks
                    .Include(a => a.Model)//This could be done using lazy loading
                    .AsEnumerable();
        }

        internal IEnumerable<TruckModelEntity> GetAllTruckModels()
        {
            return _db.TruckModels.AsEnumerable();
        }

        internal void Update(TruckEntity truckEntity)
        {
            _db.Trucks.Update(truckEntity);
            _db.SaveChanges();
        }
    }
}
