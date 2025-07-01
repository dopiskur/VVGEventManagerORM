using eventLib.Dal;
using eventLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IRepository _repository;

        public EventsController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: EventsController
        public ActionResult Index(string? search=null)
        {
            EventVM eventVM = new EventVM();

            eventVM.Events = _repository.MyEventsGet(search);

            return View(eventVM);
        }

        [Authorize]
        public ActionResult Details(int? idEvent)
        {
            EventVM eventVM = new EventVM();

            eventVM.Event = _repository.EventGet(idEvent);
            eventVM.EventPerformers = _repository.EventPerformersGet(idEvent);
            eventVM.EventTypes = _repository.EventTypesGet();

            return View(eventVM);
        }

        [Authorize]
        public ActionResult MyEventsDetails(int? idEvent)
        {
            EventVM eventVM = new EventVM();

            eventVM.Event = _repository.EventGet(idEvent);
            eventVM.EventPerformers = _repository.EventPerformersGet(idEvent);
            eventVM.EventTypes = _repository.EventTypesGet();

            return View(eventVM);
        }

        [Authorize]
        public ActionResult Add(int? idEvent)
        {
            EventVM eventVM = new EventVM();

            eventVM.Event = _repository.EventGet(idEvent);
            eventVM.EventPerformers = _repository.EventPerformersGet(idEvent);
            eventVM.EventTypes = _repository.EventTypesGet();

            return View(eventVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrationConfirm(int? eventID)
        {
            try
            {
                string? username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                User user = _repository.UserGet(null, username);

                _repository.EventRegistrationAdd(eventID, user.IDUser);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult MyEvents(string? search = null)
        {
            EventVM eventVM = new EventVM();

            string? username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            User user = _repository.UserGet(null, username);

            eventVM.EventRegistrations = _repository.EventRegistrationsGet(user.IDUser, search);

            return View(eventVM);
        }

        [Authorize]
        public ActionResult Remove(int? idEvent)
        {
            EventVM eventVM = new EventVM();

            eventVM.Event = _repository.EventGet(idEvent);
            eventVM.EventPerformers = _repository.EventPerformersGet(idEvent);
            eventVM.EventTypes = _repository.EventTypesGet();

            return View(eventVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrationRemoveConfirm(int? eventID)
        {
            try
            {
                string? username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                User user = _repository.UserGet(null, username);

                _repository.EventRegistrationDelete(eventID, user.IDUser);

                return RedirectToAction(nameof(MyEvents));
            }
            catch
            {
                return View();
            }
        }
    }
}
