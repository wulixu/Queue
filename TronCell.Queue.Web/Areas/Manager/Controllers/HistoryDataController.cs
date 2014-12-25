using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Helper;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;
namespace TronCell.Queue.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class HistoryDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Manager/HistoryData/
        public ActionResult Index(string startDate, string endDate, string reState, int pageIndex = 1)
        {
            DateTime sdt = new DateTime();
            DateTime edt = DateTime.Now;
            List<QueueCall> model = new List<QueueCall>();
            List<QueueCall> _charmodel = new List<QueueCall>();
            try
            {
                if (!string.IsNullOrEmpty(startDate)) sdt = DateTime.Parse(startDate);
                if (!string.IsNullOrEmpty(endDate)) edt = DateTime.Parse(endDate);
                if (string.IsNullOrEmpty(reState) || reState == "0")
                {
                    model = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt).OrderBy(q => q.StartTime).ToPagedList(pageIndex, 10);
                    _charmodel = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt).OrderBy(q => q.StartTime).ToList();
                }
                else
                {
                    bool state = false;
                    if (reState == "1") state = false;
                    if (reState == "2") state = true;
                    model = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt && q.IsProblem == state).OrderBy(q => q.StartTime).ToPagedList(pageIndex, 10);
                    _charmodel = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt && q.IsProblem == state).OrderBy(q => q.StartTime).ToList();
                }

                if (_charmodel.Count > 0)
                {
                    var sss = _charmodel.GroupBy(a => a.Operation).Distinct().ToList();
                    List<ApplicationUser> operationList = new List<ApplicationUser>();
                    foreach (var item in sss)
                    {
                        var cou = item.ToList()[0].Operation;
                        operationList.Add(cou);
                    }

                    var _barUserName = "";
                    var _barIsQuestion = "";
                    var _barNotQuestion = "";
                    foreach (var ope in operationList)
                    {
                        _barUserName = "'" + ope.TrueName.ToString() + "'" + "," + _barUserName;
                        _barIsQuestion = _charmodel.Where(a => a.Operation == ope && a.IsProblem == true).ToList().Count.ToString() + "," + _barIsQuestion;
                        _barNotQuestion = _charmodel.Where(a => a.Operation == ope && a.IsProblem == false).ToList().Count.ToString() + "," + _barNotQuestion;
                    }
                    ViewBag.BarUserName = _barUserName.Substring(0, _barUserName.Length - 1);
                    ViewBag.BarIsQuestion = _barIsQuestion.Substring(0, _barIsQuestion.Length - 1);
                    ViewBag.BarNotQuestion = _barNotQuestion.Substring(0, _barNotQuestion.Length - 1);

                    ViewBag.PieIsQuestion = _charmodel.ToList().Where(a => a.IsProblem == true).Count();
                    ViewBag.PieNotQuestion = _charmodel.ToList().Where(a => a.IsProblem == false).Count();

                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        // GET: Receiver/QueueCalls/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QueueCall queueCall = await db.Queues.FindAsync(id);
            if (queueCall == null)
            {
                return HttpNotFound();
            }
            return View(queueCall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QueueCall queueCall)
        {
            if (ModelState.IsValid)
            {
                var queueEntity = db.Queues.Find(queueCall.QueueCallId);
                queueEntity.ProductQuantity = queueCall.ProductQuantity;
                queueEntity.QueueUser.CompanyName = queueCall.QueueUser.CompanyName;
                queueEntity.IsProblem = queueCall.IsProblem;
                queueEntity.ProblemDescription = queueCall.ProblemDescription;
                db.Entry(queueEntity).State = EntityState.Modified;
                db.Entry(queueEntity.QueueUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(queueCall);
        }

        /// <summary>
        /// 下载详细
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public ActionResult DownloadCSV(string startDate, string endDate, string reState)
        {
            DateTime sdt = new DateTime();
            DateTime edt = DateTime.Now;
            List<QueueCall> model = new List<QueueCall>();
            if (!string.IsNullOrEmpty(startDate)) sdt = DateTime.Parse(startDate);
            if (!string.IsNullOrEmpty(endDate)) edt = DateTime.Parse(endDate);
            if (string.IsNullOrEmpty(reState) || reState == "0")
            {
                model = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt).OrderBy(q => q.StartTime).ToList();
            }
            else
            {
                bool state = false;
                if (reState == "1") state = false;
                if (reState == "2") state = true;
                model = db.Queues.AsQueryable().Where(q => q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.StartTime >= sdt && q.StartTime <= edt && q.IsProblem == state).OrderBy(q => q.StartTime).ToList();
            }

            var result = model;

            StringBuilder builder = new StringBuilder();

            //string properties = "Id,CreatedDate,CreatedBy,ModifiedDate,Deleted,ReservationNum,StartTime,EndTime,IsNotification,DeliveryType,ExpressCompanyName,ExpressNum,SupplierId,BuyerContactorId,IsUrgent,UrgentReason,QueueNumber,TransDate,ReservationStatus,TransStatus,ProcessStatus,Orders.Id,Orders.CreatedDate,Orders.CreatedBy,Orders.ModifiedDate,Orders.Deleted,Orders.ReservationId,Orders.PurchaseNumber,Orders.DeliveryNumber,Orders.BuyerContactorId,Orders.OrderStatus,Orders.ReceiverId,Orders.EvaluateStatus,Orders.EvaluateDesc";
            string propertyNames = "取号ID,取号时间,开始收料时间,结束收料时间,收料码头分类,收料窗口,收料员,供应商名称,送货人,送货人手机号,送货人身份证,数量,是否有问题,问题描述";

            builder.AppendLine(propertyNames);

            foreach (var r in result)
            {
                string line = "";
                line += string.Format("{0},", r.QueueCallId);
                line += string.Format("{0:yyyy-MM-dd HH:mm:ss},", r.CreateTime);
                line += string.Format("{0:yyyy-MM-dd HH:mm:ss},", r.StartTime);
                line += string.Format("{0:yyyy-MM-dd HH:mm:ss},", r.EndTime);
                line += string.Format("{0},", EnumCOM.GetEnumDispalyName(r.ReceiveArea.Wharfs));
                line += string.Format("{0},", r.ReceiveArea.AreaName);
                line += string.Format("{0:HH:mm},", r.Operation.TrueName);
                line += string.Format("{0:HH:mm},", r.QueueUser.CompanyName);
                line += string.Format("{0},", r.QueueUser.TrueName);
                line += string.Format("{0},", r.QueueUser.PhoneNumber);
                line += string.Format("{0},", r.QueueUser.IDCard);
                line += string.Format("{0},", r.ProductQuantity);
                line += string.Format("{0},", r.IsProblem);
                line += string.Format("{0},", r.ProblemDescription);

                builder.AppendLine(line);

            }

            return File(System.Text.Encoding.Default.GetBytes(builder.ToString()), "application/octet-stream", string.Format("HistoryData{0}.csv", DateTime.Now));
        }

    }
}
