using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;
namespace TronCell.Queue.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Admin/User/
        public ActionResult Index(int pageIndex = 1)
        {
            try
            {
                var model = db.Users.AsQueryable().Where(q => q.Deleted == false).OrderBy(a => a.UserName).ToPagedList(pageIndex, 10);
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //
        // GET: /Admin/User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/User/Create
        public ActionResult Create()
        {
            string result = "";
            foreach (var item in (new IdentityManager()).GetRoles())
            {
                result += item.Name.ToString() + ",";
            }
            ViewBag.role = result.Substring(0, result.Length - 1);
            return View();
        }
        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="roleStr"></param>
        /// <returns></returns>
        private bool isRole(string roleStr)
        {
            bool result = false;
            foreach (var item in (new IdentityManager()).GetRoles())
            {
                if (roleStr == item.Name)
                {
                    result = true;
                }
            }
            return result;
        }
        //
        // POST: /Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationUser userprofile)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            try
            {
                if (ModelState.IsValid)
                {
                    var userRole = "";
                    var roleStr = this.Request["Role"];
                    if (roleStr != null)
                    {
                        if (isRole(roleStr.ToString()))
                            userRole = this.Request["Role"].ToString();
                    }

                    userprofile.CreatedTime = DateTime.Now;
                    IdentityResult result = UserManager.Create(userprofile, userprofile.PasswordHash);
                    if (result.Succeeded)
                    {
                        IdentityManager im = new IdentityManager();
                        im.AddUserToRole(userprofile.Id, userRole);
                        db.Entry(userprofile).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                return View(userprofile);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PasswordHasher ph = new PasswordHasher();
                    var userpassword = ph.HashPassword(user.PasswordHash);
                    user.PasswordHash = userpassword;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(user);
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /Admin/User/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                ApplicationUser user = db.Users.Find(id);
                user.Deleted = true;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
