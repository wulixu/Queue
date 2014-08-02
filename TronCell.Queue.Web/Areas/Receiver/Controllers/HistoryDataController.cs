using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TronCell.Queue.Web.Areas.Receiver.Controllers
{
    [Authorize]
    public class HistoryDataController : Controller
    {
        //
        // GET: /Receiver/HistoryData/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Receiver/HistoryData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Receiver/HistoryData/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Receiver/HistoryData/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Receiver/HistoryData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Receiver/HistoryData/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Receiver/HistoryData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Receiver/HistoryData/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
