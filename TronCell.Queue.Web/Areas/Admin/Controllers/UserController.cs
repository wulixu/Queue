using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TronCell.Queue.Web.Models;
using Webdiyer.WebControls.Mvc;
using Microsoft.Practices.ServiceLocation;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace TronCell.Queue.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
            SelectRolesForList();
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
        public async Task<ActionResult> Create(RegisterViewModel userprofile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userRole = "";
                    var roleStr = Request.Params["ImageSize"];
                    //var roleStr = this.Request["Role"];
                    if (!string.IsNullOrEmpty(roleStr)&&isRole(roleStr.ToString()))
                    {
                        userRole = roleStr.ToString();
                        if (userRole == "Supplier")
                            userprofile.UserName = userprofile.IDCard;
                    }
                    else
                    {
                        SelectRolesForList();
                        ModelState.AddModelError("", "请选择用户角色");
                        return View(userprofile);
                    }

                    var userResult = db.Users.AsQueryable().Where(a => a.IDCard == userprofile.IDCard).ToList().Count;
                    if (userResult > 0)
                    {
                        SelectRolesForList();
                        ModelState.AddModelError("", "该身份证已存在，不能多次使用！");
                        return View(userprofile);
                    }
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                    var user = new ApplicationUser() { UserName = userprofile.UserName, TrueName = userprofile.TrueName, IDCard = userprofile.IDCard, PhoneNumber = userprofile.PhoneNumber, CompanyName = userprofile.CompanyName, CreatedTime = DateTime.Now, Deleted = false };
                    IdentityResult result = await UserManager.CreateAsync(user, userprofile.Password);

                    //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    //userprofile.CreatedTime = DateTime.Now;
                    //IdentityResult result = await UserManager.CreateAsync(userprofile, userprofile.PasswordHash);
                    if (result.Succeeded)
                    {
                        IdentityManager im = new IdentityManager();
                        im.AddUserToRole(user.Id, userRole);
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "用户名" + user.UserName + "已存在或者是无效的,只能包含字母或数字.");
                    }
                }
                SelectRolesForList();
                return View(userprofile);
            }
            catch
            {
                return View();
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private void SelectRolesForList() {
            //string result = "";
            List<SelectListItem> sss = new List<SelectListItem>();
            foreach (var item in (new IdentityManager()).GetRoles())
            {
                //result += item.Name.ToString() + ",";
                sss.Add(new SelectListItem() { Text = item.Name, Value = item.Name });
            }

            ViewData["ImageSize"] = sss;
            //ViewBag.role = result.Substring(0, result.Length - 1);
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
                    //var userResult = db.Users.AsQueryable().Where(a => a.IDCard == user.IDCard).ToList().Count;
                    //if (userResult > 0)
                    //{
                    //    ModelState.AddModelError("", "该身份证已存在，不能多次使用！");
                    //    return View(user);
                    //}

                    var pasValue = Request.Params["passwordValue"].ToString();
                    if (pasValue != user.PasswordHash)
                    {
                        PasswordHasher ph = new PasswordHasher();
                        var userpassword = ph.HashPassword(user.PasswordHash);
                        user.PasswordHash = userpassword;
                    }
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
                db.Users.Remove(user);
                db.SaveChanges();
                //user.Deleted = true;
                //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
