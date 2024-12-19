using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyMaterialOrganiser.Controllers.UserModule
{
    public class UserGroupController : Controller
    {
        // GET: UserGroupController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserGroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserGroupController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserGroupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserGroupController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserGroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserGroupController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserGroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
