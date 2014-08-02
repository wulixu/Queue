using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web.API
{
    public class QueueNumController : BaseAPIController
    {

        //http://localhost:13352/api/QueueNum?numberStr=33333333333
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

                if (reservations.Count == 0)
                {
                    return "error:用户不存在,请登记后取号";
                }
                int queue;
                var maxQueueNum = (from r in db.Queues
                                   where r.CreateTime >= today && r.CreateTime < tomorrow && r.Deleted == false
                                   select r.QueueNum).Max();

                var minQueueNum = (from s in db.Queues
                                   where s.CreateTime >= today && s.CreateTime < tomorrow && s.Deleted == false&&s.State==ProcessStatus.GotQueueNumber&&s.State!=ProcessStatus.Processed&&s.State!=ProcessStatus.LazyProcess&&s.State!=ProcessStatus.NoQueueNumber&&s.State!=ProcessStatus.Processing
                                   select s.QueueNum).Min();
                if (string.IsNullOrEmpty(maxQueueNum))
                {
                    queue = 1;
                }
                else
                {
                    queue = int.Parse(maxQueueNum);
                    queue++;
                }
                int min;
                if (string.IsNullOrEmpty(minQueueNum)) {
                    min = 1;
                }
                    else{
                    min=int.Parse(minQueueNum);
                }

                
                string numCount = (queue - min).ToString();
                string trueName = reservations[0].TrueName;
                var result = queue.ToString().PadLeft(4, '0');
                QueueCall queueEntiy = new QueueCall();
                queueEntiy.CreateTime = DateTime.Now;
                queueEntiy.Deleted = false;
                queueEntiy.QueueUserId = reservations[0].Id;
                queueEntiy.Priority = PriorityStatus.General;
                queueEntiy.State = ProcessStatus.GotQueueNumber;
                queueEntiy.QueueNum = result;
                db.Entry(queueEntiy).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                var passCode = reservations[0].IDCard.Substring(reservations[0].IDCard.Length - 7).ToString()+result;
                result = result + "," + "前面还有" + numCount + "个用户排队等待," + passCode+","+trueName;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}