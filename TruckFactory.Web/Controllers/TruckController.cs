using Microsoft.AspNetCore.Mvc;
using TruckFactory.Web.Models;
using TruckFactory.Web.Application.Services;
using TruckFactory.Web.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TruckFactory.Web.Controllers
{
    public class TruckController : Controller
    {
        private readonly TruckService _truckService;

        public TruckController(FactoryContext db)
        {
            _truckService = new TruckService(db);
        }

        public ActionResult Index()
        {
            IEnumerable<TruckFormModel> results = _truckService.GetAll();
            return View(results);
        }

        [HttpGet]
        public ActionResult Create()
        {
            TruckFormModel formModel = _truckService.GetDefault();
            ViewData["TruckModelOptions"] = GenerateSelectListOptions();
            return View("CreateEdit", formModel);
        }

        private IEnumerable<SelectListItem> GenerateSelectListOptions()
        {
            IEnumerable<TruckModelFormModel> r = _truckService.ListTruckModels();
            return r.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckFormModel truckForm)
        {
            try
            {
                this.ModelState.Merge(_truckService.Validate(truckForm));
                ViewData["TruckModelOptions"] = GenerateSelectListOptions();
                if ( ModelState.IsValid)
                {
                    _truckService.Save(truckForm);
                    return RedirectToAction(nameof(Index));
                } else
                    return View("CreateEdit");
            }
            catch
            {
                this.ModelState.AddModelError("Model", "Unable to perform save operation.");
                return View("CreateEdit");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var truckFormModel = _truckService.Get(id);
            ViewData["TruckModelOptions"] = GenerateSelectListOptions();
            if (truckFormModel != null)
                return View("CreateEdit",truckFormModel);
            else
               return new NotFoundObjectResult(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TruckFormModel truckForm)
        {
            try
            {
                this.ModelState.Merge(_truckService.Validate(truckForm));
                ViewData["TruckModelOptions"] = GenerateSelectListOptions();
                if (ModelState.IsValid)
                {
                    _truckService.Update(truckForm);
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View("CreateEdit");
            }
            catch
            {
                this.ModelState.AddModelError("Model", "Unable to perform save operation.");
                return View("CreateEdit");
            }
        }

        public ActionResult Delete(int id)
        {
            if (_truckService.Delete(id))
                return RedirectToAction(nameof(Index));

            return View();
        }
    }
}
