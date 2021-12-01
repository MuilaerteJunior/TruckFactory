using Microsoft.AspNetCore.Mvc.ModelBinding;
using TruckFactory.Web.Domain.Data;
using TruckFactory.Web.Models;
using TruckFactory.Web.Domain.Repositories;
using TruckFactory.Web.Application.Shared;
using TruckFactory.Web.Domain;

namespace TruckFactory.Web.Application.Services
{
    public class TruckService : ITruckService
    {
        private readonly ModelStateDictionary _modelState;
        private readonly TruckRepository _repo;

        public TruckService(FactoryContext db)
        {
            _modelState = new ModelStateDictionary();
            _repo = new TruckRepository(db);
        }

        internal IEnumerable<TruckFormModel> GetAll()
        {
            var r = _repo.GetAll();
            IEnumerable<TruckFormModel> result = r.Select(item => new TruckFormModel
            {
                Id = item.Id,
                Name = item.Name,
                Model = (EnumTruckModel)item.Model.Id,
                ModelYear = item.ModelYear,
                ProductionYear = item.ProductionYear
            });
            return result;
        }

        internal IEnumerable<TruckModelFormModel> ListTruckModels()
        {
            var r = _repo.GetAllTruckModels();
            return r.Select(e => new TruckModelFormModel
            {
                Id = e.Id,
                Name = e.Name
            });
        }

        internal TruckFormModel GetDefault()
        {
            var dtNow = DateTime.Now;
            return new TruckFormModel
            {
                ProductionYear = dtNow.Year,
                ModelYear = dtNow.Year,
            };
        }

        public ModelStateDictionary Validate(TruckFormModel truckForm)
        {
            var dtNow = DateTime.Now;
            if (truckForm.ModelYear < dtNow.Year)
                _modelState.AddModelError<TruckFormModel>(a => a.ModelYear, "Model year below allowed!");

            if (truckForm.ProductionYear != dtNow.Year)
                _modelState.AddModelError<TruckFormModel>(a => a.ProductionYear, "Production year below allowed!");

            if (truckForm.Model != EnumTruckModel.FM && truckForm.Model != EnumTruckModel.FH)
                _modelState.AddModelError<TruckFormModel>(a => a.Model, "Model different from expected!");

            return _modelState;
        }

        public int Save(TruckFormModel truckForm)
        {

            var t = new TruckEntity
            {
                Name = truckForm.Name,
                ID_TRUCK_MODEL = (short)truckForm.Model,
                Model = null,
                ModelYear = truckForm.ModelYear,
                ProductionYear = DateTime.Now.Year
            };

            return _repo.Save(t);
        }

        public void Update(TruckFormModel truckForm)
        {
            if (!truckForm.Id.HasValue)
                throw new ArgumentException("Missing truck identification");

            var t2 = _repo.Get(truckForm.Id.Value);

            t2.Name = truckForm.Name;
            t2.ID_TRUCK_MODEL = (short)truckForm.Model;
            t2.ModelYear = truckForm.ModelYear;
            _repo.Update(t2);
        }

        public TruckFormModel Get(int id)
        {
            var r = _repo.Get(id);
            if (r != null)
            {
                return new TruckFormModel()
                {
                    Id = r.Id,
                    Model = (EnumTruckModel)r.Model.Id,
                    ModelYear = r.ModelYear,
                    Name = r.Name,
                    ProductionYear = r.ProductionYear
                };
            }

            return null;
        }

        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
