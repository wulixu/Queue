using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
namespace TronCell.Queue.Web.Areas.Manager.Controllers
{
    [Authorize]
    public class QueueCallController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Manager/QueueCall/
        public ActionResult Index(int pageIndex=1)
        {
            DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime tomorrow = today.AddDays(1);
            //db.Queues.Include(q => q.Operation).Include(q => q.QueueUser).Include(q => q.ReceiveArea);
            var model = db.Queues.Include(q => q.Operation).Include(q => q.QueueUser).Include(q => q.ReceiveArea).Where(p => p.QueueNum != null && p.CreateTime >= today && p.CreateTime < tomorrow && (p.State == TronCell.Queue.Web.Models.ProcessStatus.GotQueueNumber || p.State == TronCell.Queue.Web.Models.ProcessStatus.Processing) && p.State != TronCell.Queue.Web.Models.ProcessStatus.Processed && p.State != TronCell.Queue.Web.Models.ProcessStatus.NoQueueNumber && p.State != TronCell.Queue.Web.Models.ProcessStatus.LazyProcess && p.Deleted == false).OrderBy(w => w.Priority).ThenBy(v => v.QueueNum).ToList();
            return View(model);
        }

        // GET: /Manager/QueueCall/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QueueCall queuecall = await db.Queues.FindAsync(id);
            if (queuecall == null)
            {
                return HttpNotFound();
            }
            return View(queuecall);
        }

        // GET: /Manager/QueueCall/Create
        public ActionResult Create()
        {
            ViewBag.OperationId = new SelectList(db.Users, "Id", "IDCard");
            ViewBag.QueueUserId = new SelectList(db.Users, "Id", "IDCard");
            ViewBag.ReceiveAreaId = new SelectList(db.ReceiveArea, "ReceiveAreaId", "AreaName");
            return View();
        }

        // POST: /Manager/QueueCall/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="QueueCallId,QueueNum,State,Priority,CreateTime,StartTime,EndTime,QueueUserId,OperationId,ReceiveAreaId,Deleted")] QueueCall queuecall)
        {
            if (ModelState.IsValid)
            {
                db.Queues.Add(queuecall);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OperationId = new SelectList(db.Users, "Id", "IDCard", queuecall.OperationId);
            ViewBag.QueueUserId = new SelectList(db.Users, "Id", "IDCard", queuecall.QueueUserId);
            ViewBag.ReceiveAreaId = new SelectList(db.ReceiveArea, "ReceiveAreaId", "AreaName", queuecall.ReceiveAreaId);
            return View(queuecall);
        }
        /// <summary>
        /// 进行插队操作，将送货优先级提高
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> JumpQueue(int id)
        {
            try
            {
                QueueCall queuecall = await db.Queues.FindAsync(id);
                if (queuecall == null)
                {
                    return HttpNotFound();
                }
                queuecall.Priority = PriorityStatus.Urgent;
                db.Entry(queuecall).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e) {
                return RedirectToAction("Index");
            }
        }
        // GET: /Manager/QueueCall/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QueueCall queuecall = await db.Queues.FindAsync(id);
            if (queuecall == null)
            {
                return HttpNotFound();
            }
            ViewBag.OperationId = new SelectList(db.Users, "Id", "IDCard", queuecall.OperationId);
            ViewBag.QueueUserId = new SelectList(db.Users, "Id", "IDCard", queuecall.QueueUserId);
            ViewBag.ReceiveAreaId = new SelectList(db.ReceiveArea, "ReceiveAreaId", "AreaName", queuecall.ReceiveAreaId);
            return View(queuecall);
        }

        // POST: /Manager/QueueCall/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="QueueCallId,QueueNum,State,Priority,CreateTime,StartTime,EndTime,QueueUserId,OperationId,ReceiveAreaId,Deleted")] QueueCall queuecall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(queuecall).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OperationId = new SelectList(db.Users, "Id", "IDCard", queuecall.OperationId);
            ViewBag.QueueUserId = new SelectList(db.Users, "Id", "IDCard", queuecall.QueueUserId);
            ViewBag.ReceiveAreaId = new SelectList(db.ReceiveArea, "ReceiveAreaId", "AreaName", queuecall.ReceiveAreaId);
            return View(queuecall);
        }

        // GET: /Manager/QueueCall/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QueueCall queuecall = await db.Queues.FindAsync(id);
            if (queuecall == null)
            {
                return HttpNotFound();
            }
            return View(queuecall);
        }

        // POST: /Manager/QueueCall/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QueueCall queuecall = await db.Queues.FindAsync(id);
            db.Queues.Remove(queuecall);
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
