using Microsoft.AspNetCore.Mvc.ModelBinding;
using TruckFactory.Web.Models;

namespace TruckFactory.Web.Application.Services
{
    public interface ITruckService
    {
        TruckFormModel? Get(int id);
        int Save(TruckFormModel truckForm);
        void Update(TruckFormModel truckForm);
        ModelStateDictionary Validate(TruckFormModel truckForm);
    }
}