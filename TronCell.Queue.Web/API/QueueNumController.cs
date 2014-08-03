using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web.API
{
    public class QueueNumController : BaseAPIController
    {

        //http://localhost:13352/api/QueueNum?numberStr=320324199006140674
        /// <summary>
        /// 获取排队号码
        /// </summary>
        /// <param name="queueUserInfo"></param>
        /// <returns>返回排队号和信息
        /// eg：0002,前面还有1个用户等待排队,140673,张三
        /// 返回空,则有异常或有错误信息返回"error:XXX"
        /// </returns>
        public string GetQueueuNum(string NumberStr)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime tomorrow = today.AddDays(1);
                List<ApplicationUser> reservations = db.Users.Where(p => (p.IDCard == NumberStr || p.PhoneNumber == NumberStr) && p.Deleted == false).ToList();
                var userManager= new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                
                #region 判断当前最小号码
                if (reservations.Count == 0)
                {
                    return "error:请刷身份证登记,再取号";
                }
                if (!userManager.GetRoles(reservations[0].Id).Contains("Supplier"))
                {
                    return "error:用户不是送货人员,不能取号";
                }
                var minQueueNum = (from s in db.Queues
                                   where s.CreateTime >= today && s.CreateTime < tomorrow && s.Deleted == false && s.State == ProcessStatus.GotQueueNumber && s.State != ProcessStatus.Processed && s.State != ProcessStatus.LazyProcess && s.State != ProcessStatus.NoQueueNumber && s.State != ProcessStatus.Processing
                                   select s.QueueNum).Min();

                int min;
                if (string.IsNullOrEmpty(minQueueNum))
                {
                    min = 1;
                }
                else
                {
                    min = int.Parse(minQueueNum);
                }
                #endregion
                string reser = reservations[0].Id;
                List<QueueCall> callList = db.Queues.Where(t => t.CreateTime >= today && t.CreateTime < tomorrow && t.Deleted == false && t.QueueUser.Id == reser && (t.State == ProcessStatus.GotQueueNumber||t.State==ProcessStatus.Processing)).ToList();
                //&&(t.State == ProcessStatus.NoQueueNumber || t.State == ProcessStatus.Processing)&& (t.State == ProcessStatus.NoQueueNumber||t.State==ProcessStatus.Processing)
                QueueCall call;
                var result = "";
                int queue;
                if (callList.Count > 0)
                {
                    call = callList[0];
                    queue = int.Parse(callList[0].QueueNum);
                    result = queue.ToString().PadLeft(4, '0');
                }
                else
                {
                    var maxQueueNum = (from r in db.Queues
                                       where r.CreateTime >= today && r.CreateTime < tomorrow && r.Deleted == false
                                       select r.QueueNum).Max();
                    if (string.IsNullOrEmpty(maxQueueNum))
                    {
                        queue = 1;
                    }
                    else
                    {
                        queue = int.Parse(maxQueueNum);
                        queue++;
                    }

                    result = queue.ToString().PadLeft(4, '0');

                    QueueCall queueEntiy = new QueueCall();
                    queueEntiy.CreateTime = DateTime.Now;
                    queueEntiy.Deleted = false;
                    queueEntiy.QueueUserId = reservations[0].Id;
                    queueEntiy.Priority = PriorityStatus.General;
                    queueEntiy.State = ProcessStatus.GotQueueNumber;
                    queueEntiy.QueueNum = result;
                    db.Entry(queueEntiy).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }
                string numCount = (queue - min).ToString();
                string trueName = reservations[0].TrueName;
                var passCode = reservations[0].IDCard.Substring(reservations[0].IDCard.Length - 8,7).ToString() + result;
                result = result + "," + "前面还有" + numCount + "个用户排队等待," + passCode + "," + trueName;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}