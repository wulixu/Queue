using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TronCell.Queue.Web.Helper;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web.API
{
    public class InQueueReservationsController : BaseAPIController
    {
        //http://localhost:13352/api/InQueueReservations
        /// <summary>
        /// 按照叫号排序
        /// </summary>
        /// <returns>返回排队信息和状态
        /// eg:id,窗口，正在收料，排队号|id,窗口，等待收料，排队号
        /// 返回空 则有异常或没有排队信息
        /// </returns>
        public string GetInQueueReservations()
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime tomorrow = today.AddDays(1);
                var querylist = (from p in db.Queues
                                 where p.QueueNum != null && p.CreateTime >= today && p.CreateTime < tomorrow && (p.State == ProcessStatus.GotQueueNumber || p.State == ProcessStatus.Processing) && p.State != ProcessStatus.Processed && p.State != ProcessStatus.NoQueueNumber && p.State != ProcessStatus.LazyProcess && p.Deleted == false
                                 select new { p.QueueCallId, p.QueueNum,p.Wharfs ,p.ReceiveArea.AreaName, p.State, p.Priority }).OrderBy(w => w.Priority).ThenBy(v => v.QueueNum).ToList();
                string response = "";
                foreach (var item in querylist)
                {
                    response += item.QueueCallId.ToString() + "," + EnumCOM.GetEnumDispalyName(item.Wharfs) + item.AreaName + "," + EnumCOM.GetEnumDispalyName(item.State) + "," + item.QueueNum + "|";
                }
                if (!string.IsNullOrEmpty(response))
                {
                    response.Substring(0, response.Length - 1);
                }
                return response;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}