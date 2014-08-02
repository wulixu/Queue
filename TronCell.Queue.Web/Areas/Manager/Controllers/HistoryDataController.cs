using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;
namespace TronCell.Queue.Web.Areas.Manager.Controllers
{
    [Authorize]
    public class HistoryDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Manager/HistoryData/
        public ActionResult Index(int indexPage=1)
        {
            try
            {
                var model = db.Queues.AsQueryable().Where(q => q.Deleted == false && q.State == TronCell.Queue.Web.Models.ProcessStatus.Processed && q.State != TronCell.Queue.Web.Models.ProcessStatus.GotQueueNumber
                    && q.State != TronCell.Queue.Web.Models.ProcessStatus.Processing && q.State != TronCell.Queue.Web.Models.ProcessStatus.LazyProcess).OrderBy(q => q.CreateTime).ToPagedList(indexPage, 10);
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
