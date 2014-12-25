using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Helper;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace TronCell.Queue.Web.Areas.Receiver.Controllers
{
    [Authorize(Roles = "Receiver")]
    public class ChoiceAreaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Receiver/ChoiceArea/
        //public ActionResult Index(int pageIndex = 1)
        //{
        //    var model = db.ReceiveArea.AsQueryable().OrderBy(a => a.AreaName).ToPagedList(pageIndex, 10);
        //    return View(model);
        //}

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 五金类收料区
        /// </summary>
        /// <returns></returns>
        public ActionResult HardwareReceiveArea(string callNum)
        {
            var _area = Wharfs.五金类码头;
            if (callNum == "callNum")
            {
                ReceiveArea reciverArea = SelectFreeArea(_area);
                if (reciverArea != null)
                {
                    QueueCall _queuecall = SelectQueueNum(_area);
                    if (_queuecall != null)
                    {
                        CallNullToReceiver(reciverArea, _queuecall);
                    }
                }
            }
            var queueList = SelectAllQueueCall(Wharfs.五金类码头);
            return View(queueList);
        }
        /// <summary>
        /// 电子类收料区
        /// </summary>
        /// <returns></returns>
        public ActionResult ElectronicReceiveArea(string callNum)
        {
            var _area = Wharfs.电子类码头;
            if (callNum == "callNum")
            {
                ReceiveArea reciverArea = SelectFreeArea(_area);
                if (reciverArea != null)
                {
                    QueueCall _queuecall = SelectQueueNum(_area);
                    if (_queuecall != null)
                    {
                        CallNullToReceiver(reciverArea, _queuecall);
                    }
                }
            }
            var queueList = SelectAllQueueCall(Wharfs.电子类码头);
            return View(queueList);
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

        // POST: Receiver/QueueCalls/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QueueCall queueCall)
        {
            if (ModelState.IsValid)
            {
                var queueEntity = db.Queues.Find(queueCall.QueueCallId);
                queueEntity.EndTime = DateTime.Now;
                var WharfsResult = queueEntity.ReceiveArea.Wharfs;
                queueEntity.ProductQuantity = queueCall.ProductQuantity;
                queueEntity.QueueUser.CompanyName = queueCall.QueueUser.CompanyName;
                queueEntity.State = TronCell.Queue.Web.Models.ProcessStatus.Processed;
                queueEntity.ReceiveArea.AreaState = AreaState.IsFree;
                db.Entry(queueEntity).State = EntityState.Modified;
                db.Entry(queueEntity.ReceiveArea).State = EntityState.Modified;
                db.Entry(queueEntity.QueueUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (WharfsResult == Wharfs.五金类码头) return RedirectToAction("HardwareReceiveArea");
                if (WharfsResult == Wharfs.电子类码头) return RedirectToAction("ElectronicReceiveArea");
            }
            return View(queueCall);
        }

        // GET: Receiver/QueueCalls/IsProblemReceiver/5
        public async Task<ActionResult> IsProblemReceiver(int? id)
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
        // POST: Receiver/QueueCalls/IsProblemReceiver/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IsProblemReceiver(QueueCall queueCall)
        {
            string[] values = Request.Form.GetValues("ProblemText");
            string _values = "";
            if (values != null)
            {
                foreach (var item in values)
                {
                    _values = item + "," + _values;
                }
            }

            string problemStr =_values + Request.Form["problemTextArea1"] ;
            if (ModelState.IsValid)
            {
                var queueEntity = db.Queues.Find(queueCall.QueueCallId);
                queueEntity.EndTime = DateTime.Now;
                var WharfsResult = queueEntity.ReceiveArea.Wharfs;
                queueEntity.ProductQuantity = queueCall.ProductQuantity;
                queueEntity.QueueUser.CompanyName = queueCall.QueueUser.CompanyName;
                queueEntity.State = TronCell.Queue.Web.Models.ProcessStatus.Processed;
                queueEntity.ReceiveArea.AreaState = AreaState.IsFree;
                queueEntity.IsProblem = true;
                queueEntity.ProblemDescription = problemStr;

                db.Entry(queueEntity).State = EntityState.Modified;
                db.Entry(queueEntity.ReceiveArea).State = EntityState.Modified;
                db.Entry(queueEntity.QueueUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (WharfsResult == Wharfs.五金类码头) return RedirectToAction("HardwareReceiveArea");
                if (WharfsResult == Wharfs.电子类码头) return RedirectToAction("ElectronicReceiveArea");
            }
            return View(queueCall);
        }

        public async Task<ActionResult> PassNum(int id)
        {
            var queueEntity = db.Queues.Find(id);
            queueEntity.EndTime = DateTime.Now;
            var WharfsResult = queueEntity.ReceiveArea.Wharfs;
            queueEntity.State = TronCell.Queue.Web.Models.ProcessStatus.Processed;
            queueEntity.ReceiveArea.AreaState = AreaState.IsFree;
            queueEntity.IsProblem = true;
            queueEntity.ProblemDescription = "过号";

            db.Entry(queueEntity).State = EntityState.Modified;
            db.Entry(queueEntity.ReceiveArea).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (WharfsResult == Wharfs.五金类码头) return RedirectToAction("HardwareReceiveArea");
            if (WharfsResult == Wharfs.电子类码头) return RedirectToAction("ElectronicReceiveArea");

            return HttpNotFound();
        }



        #region 叫号信息处理
        //获取当前空闲码头
        private ReceiveArea SelectFreeArea(Wharfs areaChoice)
        {
            ReceiveArea _area = db.ReceiveArea.AsQueryable().Where(a => a.AreaState == AreaState.IsFree && a.Wharfs == areaChoice).FirstOrDefault();
            return _area;
        }

        //获取当前收料员的所有排队号码
        private List<QueueCall> SelectAllQueueCall(Wharfs areaChoice)
        {
            DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime tomorrow = today.AddDays(1);
            var receiver = getReceiverUser();
            var queueList= (from r in db.Queues
             where r.CreateTime >= today && r.CreateTime < tomorrow && r.State == TronCell.Queue.Web.Models.ProcessStatus.Processing && r.Deleted == false && r.OperationId == receiver.Id&&r.ReceiveArea.Wharfs==areaChoice
             select r).OrderBy(r => r.Priority).OrderBy(r => r.QueueNum).ToList();
            return queueList;
        }

        /// <summary>
        /// 获取排队号码
        /// </summary>
        /// <param name="areaChoice">所在码头区域</param>
        /// <returns></returns>
        private QueueCall SelectQueueNum(Wharfs areaChoice)
        {
            DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime tomorrow = today.AddDays(1);
            QueueCall queue = null;
            var _queueList = (from r in db.Queues
                              where r.CreateTime >= today && r.CreateTime < tomorrow && r.State == TronCell.Queue.Web.Models.ProcessStatus.GotQueueNumber && r.Deleted == false && r.Wharfs == areaChoice
                              select r).OrderBy(r => r.Priority).ThenBy(r => r.QueueNum).ToList();
            if (_queueList.Count > 0)
            {
                queue = _queueList[0];
            }
            //QueueCall _queue = db.Queues.ToList().Where(a => a.CreateTime >= today && a.CreateTime < tomorrow && a.State == TronCell.Queue.Web.Models.ProcessStatus.GotQueueNumber && a.Wharfs == areaChoice && a.Deleted == false).OrderBy(a => a.QueueNum).OrderBy(b => b.Priority).First();
            return queue;
        }

        /// <summary>
        /// 叫号，将排队人员添加到对应窗口
        /// </summary>
        /// <param name="isFreeArea">空闲窗口</param>
        /// <param name="queueCall">排队实体</param>
        private void CallNullToReceiver(ReceiveArea isFreeArea,QueueCall queueCall)
        {
            
            ApplicationUser receiver = getReceiverUser();
            //排队号实体
            var queEntity = db.Queues.Find(queueCall.QueueCallId);
            isFreeArea.AreaState = AreaState.IsBusy;
            queEntity.ReceiveArea = isFreeArea;
            queEntity.StartTime = DateTime.Now;
            if (queEntity.State != TronCell.Queue.Web.Models.ProcessStatus.Processing) queEntity.State = TronCell.Queue.Web.Models.ProcessStatus.Processing;
            if (receiver != null) queEntity.Operation = receiver;
            db.Entry(queEntity).State = System.Data.Entity.EntityState.Modified;
            db.Entry(queEntity.ReceiveArea).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        //获取当前收料员
        private ApplicationUser getReceiverUser()
        {
            var userName = User.Identity.Name;
            var query = (from p in db.Users
                         where p.UserName == userName
                         select p).ToList();
            ApplicationUser receiver = null;
            if (query.Count > 0)
                receiver = query[0];
            return receiver;
        }

        #endregion
        public ActionResult Details(int id, string methodName, int queueCallId)
        {
            ReceiveArea receivearea = db.ReceiveArea.Find(id);
            if (receivearea == null)
            {
                return HttpNotFound();
            }

            QueueCall queEntity = new QueueCall();
            DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime tomorrow = today.AddDays(1);
            string resutl = "";
            if (methodName != "firstQueue")
            {
                resutl = PassQueue(today, tomorrow, methodName, queueCallId);
            }
            if (resutl == "")
            {
                var userName = User.Identity.Name;
                var query = (from p in db.Users
                             where p.UserName == userName
                             select new { p.Id, p.IDCard, p.UserName, p.Roles }).ToList();
                var _userID = "";
                if (query.Count > 0)
                {
                    _userID = query[0].Id;
                }
                var _queue = (from r in db.Queues
                              where r.CreateTime >= today && r.CreateTime < tomorrow && r.State == TronCell.Queue.Web.Models.ProcessStatus.Processing && r.Operation.Id == _userID && r.Deleted == false
                              select r.QueueCallId).ToList();

                if (_queue.Count > 0)
                {
                    queEntity.QueueCallId = _queue[0];
                    queEntity.ReceiveArea = receivearea;
                }
                else
                {

                    var querylist = (from p in db.Queues
                                     where p.QueueNum != null && p.CreateTime >= today && p.CreateTime < tomorrow && p.State == TronCell.Queue.Web.Models.ProcessStatus.GotQueueNumber && p.State != TronCell.Queue.Web.Models.ProcessStatus.Processed && p.State != TronCell.Queue.Web.Models.ProcessStatus.NoQueueNumber && p.State != TronCell.Queue.Web.Models.ProcessStatus.LazyProcess && p.Deleted == false
                                     select new { p.QueueCallId, p.QueueNum, p.Priority }).OrderBy(w => w.Priority).ThenBy(v => v.QueueNum).ToList();

                    if (querylist.Count > 0)
                    {
                        var item = querylist.ToList()[0];
                        if (item.QueueNum != "")
                        {
                            queEntity = db.Queues.Find(item.QueueCallId);
                            if (queEntity.State != TronCell.Queue.Web.Models.ProcessStatus.Processing)
                            {
                                queEntity.State = TronCell.Queue.Web.Models.ProcessStatus.Processing;
                            }
                            queEntity.ReceiveArea = receivearea;
                            var strContent = string.Format("顾客您好，请到{0}进行收料！", receivearea.AreaName.ToString());
                            //sendSMS(queEntity.QueueUser.PhoneNumber, strContent);
                            if (query.Count > 0)
                            {
                                queEntity.OperationId = query[0].Id;
                            }

                            db.Entry(queEntity).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            else if (resutl == "JumpRecive")
            {
                queEntity.QueueCallId = queueCallId;
            }
            QueueCall queueDetails = db.Queues.Find(queEntity.QueueCallId);
            if (queueDetails != null && !Request.IsAjaxRequest())
                return View(queueDetails);
            else if (queueDetails != null && Request.IsAjaxRequest())
            {
                return this.Json(new { result = false, queuecallid = queueDetails.QueueCallId, areaid = id });
            }
            else
            {
                //return RedirectToAction("TimeRefreshPage", "ChoiceArea", new { area = "Receiver", AreaID = id });
                //return this.Json(new { result = true },JsonRequestBehavior.AllowGet);
                if (Request.IsAjaxRequest())
                {
                    return this.Json(new { result = true });
                }
                else
                    return RedirectToAction("TimeRefreshPage", new { AreaID = id });
            }
        }
        public ActionResult TimeRefreshPage(int AreaID)
        {
            ViewBag.valueAreaid = AreaID;
            return View();
        }
        //处理是否收料
        private string PassQueue(DateTime today, DateTime tomorrow, string methodName, int queueCallId)
        {
            string result = "";
            try
            {
                QueueCall queue = db.Queues.Find(queueCallId);

                if (queue != null)
                {
                    switch (methodName)
                    {
                        case "PassQueue":
                            queue.Deleted = true;
                            queue.EndTime = DateTime.Now;
                            break;
                        case "Problematic":
                            queue.State = TronCell.Queue.Web.Models.ProcessStatus.LazyProcess;
                            queue.EndTime = DateTime.Now;
                            break;
                        case "NextQueue":
                            if (queue.StartTime != null)
                            {
                                queue.State = TronCell.Queue.Web.Models.ProcessStatus.Processed;
                                queue.EndTime = DateTime.Now;
                            }
                            break;
                        case "startRecive":
                            queue.StartTime = DateTime.Now;
                            result = "strartReciver";
                            break;
                        case "JumpRecive":
                            result = "JumpRecive";
                            break;
                    }

                    db.Entry(queue).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string errStr = ex.Message;
            }
            return result;
        }

        private string sendSMS(string mobile, string strContent)
        {
            //消息签名
            string signature = System.Configuration.ConfigurationManager.AppSettings["SEND_MESSAGESIGNATURE"].ToString();
            strContent = strContent + "【" + signature + "】";
            SendMSS send = new SendMSS();
            var resultString = send.sendmssForUser(mobile, strContent);
            if (resultString.Equals("sms&stat=100&message=发送成功"))
            {
                return "Success";
            }
            else
                return "Error";
        }

    }
}
