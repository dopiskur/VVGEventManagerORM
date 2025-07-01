using eventLib.Dal;
using eventLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PerformerManagementController : Controller
    {
        private readonly IRepository _repository;

        public PerformerManagementController(IRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public ActionResult Index(string? search = null)
        {
            PerformerVM performerVM = new PerformerVM();
            performerVM.Performers = _repository.PerformersGet(search);
            return View(performerVM);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PerformerVM performerVM)
        {
            try
            {
                _repository.PerformerAdd(performerVM.Performer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(int idPerformer)
        {
            PerformerVM performerVM = new PerformerVM();
            performerVM.Performer = _repository.PerformerGet(idPerformer);
            return View(performerVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PerformerVM value)
        {
            try
            {
                _repository.PerformerUpdate(value.Performer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Delete(int? idPerformer)
        {
            Performer value = _repository.PerformerGet(idPerformer);
            return View(value);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int idPerformer)
        {
            try
            {
                _repository.PerformerDelete(idPerformer);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
