using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;
namespace TronCell.Queue.Web.Areas.Receiver.Controllers
{
    [Authorize(Roles = "Receiver")]
    public class HistoryDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Receiver/HistoryData/
        public ActionResult Index(string startDate, string endDate , int pageIndex = 1)
        {
            DateTime sdt = new DateTime();
            DateTime edt = DateTime.Now;
            List<QueueCall> model = new List<QueueCall>();
            try
            {
                if (!string.IsNullOrEmpty(startDate)) sdt = DateTime.Parse(startDate);
                if (!string.IsNullOrEmpty(endDate)) edt = DateTime.Parse(endDate);
                model = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt && q.IsProblem == true).OrderBy(q => q.StartTime).ToPagedList(pageIndex, 10);
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
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
