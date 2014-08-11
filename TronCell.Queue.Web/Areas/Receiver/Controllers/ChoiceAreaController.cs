using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Helper;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace TronCell.Queue.Web.Areas.Receiver.Controllers
{
    [Authorize]
    public class ChoiceAreaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Receiver/ChoiceArea/
        public ActionResult Index(int pageIndex = 1)
        {
            var model = db.ReceiveArea.AsQueryable().OrderBy(a => a.AreaName).ToPagedList(pageIndex, 10);
            return View(model);
        }

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
                              where r.CreateTime >= today && r.CreateTime < tomorrow && r.State == TronCell.Queue.Web.Models.ProcessStatus.Processing && r.Operation.Id == _userID&&r.Deleted==false
                              select r.QueueCallId).ToList();

                if (_queue.Count > 0)
                {
                    queEntity.QueueCallId = _queue[0];
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
