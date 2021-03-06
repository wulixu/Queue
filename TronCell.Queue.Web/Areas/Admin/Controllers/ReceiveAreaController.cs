﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace TronCell.Queue.Web.Areas.Admin.Controllers
{
    public class ReceiveAreaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Admin, Manager")]
        // GET: /Admin/ReceiverArea/
        public async Task<ActionResult> Index(int pageIndex = 1)
        {
            //var model = db.ReceiveArea.AsQueryable().OrderBy(a => a.AreaName).ToPagedList(pageIndex, 10);
            List<QueueCall> queueuList = new List<QueueCall>();
            var reAreaList = db.ReceiveArea.AsQueryable().ToList();
            foreach (var item in reAreaList)
            {
                if(item.AreaState==AreaState.IsFree)
                    queueuList.Add(new QueueCall() { ReceiveArea = item });
                if (item.AreaState == AreaState.IsBusy)
                {
                    var queueEnt = db.Queues.Include(a => a.ReceiveArea).Where(a => a.ReceiveArea.ReceiveAreaId == item.ReceiveAreaId).FirstOrDefault();
                    queueuList.Add(queueEnt);
                }
            }
            var model = queueuList.ToPagedList(pageIndex, 10);

            //var model = (from r in db.Queues
            //             where r.ReceiveArea.AreaState==AreaState.IsBusy && r.State==TronCell.Queue.Web.Models.ProcessStatus.Processing
            //             select r
            //             ).OrderBy(a => a.ReceiveArea.AreaName).ToPagedList(pageIndex, 10);
            return View(model);
            //return View(await db.ReceiveArea.ToListAsync());
        }


        // GET: /Admin/ReceiverArea/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiveArea receivearea = await db.ReceiveArea.FindAsync(id);
            if (receivearea == null)
            {
                return HttpNotFound();
            }
            return View(receivearea);
        }
        [Authorize(Roles = "Admin")]
        // GET: /Admin/ReceiverArea/Create
        public ActionResult Create()
        {
            //var statuses = from Wharfs s in Enum.GetValues(typeof(Wharfs))
            //               select new { ID = s, Name = s.ToString() };
            //ViewData["taskStatus"] = new SelectList(statuses, "ID", "Name");
            return View();
        }

        // POST: /Admin/ReceiverArea/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "ReceiveAreaId,AreaName,Description,Category,CreateTime,Wharfs")] ReceiveArea receivearea)
        {
            if (ModelState.IsValid)
            {
                receivearea.CreateTime = DateTime.Now;
                receivearea.AreaState = AreaState.IsFree;
                db.ReceiveArea.Add(receivearea);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(receivearea);
        }

        // GET: /Admin/ReceiverArea/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiveArea receivearea = await db.ReceiveArea.FindAsync(id);
            if (receivearea == null)
            {
                return HttpNotFound();
            }
            return View(receivearea);
        }

        // POST: /Admin/ReceiverArea/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> Edit([Bind(Include = "ReceiveAreaId,AreaName,Description,Category,CreateTime,Wharfs,AreaState")] ReceiveArea receivearea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receivearea).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(receivearea);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReceiveArea receivearea = await db.ReceiveArea.FindAsync(id);
            db.ReceiveArea.Remove(receivearea);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
