using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TronCell.Queue.Web.Models;

namespace TronCell.Queue.Web.API
{
    public class RegistUserController : BaseAPIController
    {
        [HttpGet]

        /// <summary>
        /// 添加供应商用户信息
        /// </summary>
        /// http://localhost:13352/api/RegistUser?userInfo=15161696372,zhou,320324199006140673
        /// success:用户信息登记成功
        /// error:用户登记失败
        /// error:用户已存在
        /// </returns>
        public string registSupplier(string userInfo)
        {
            string[] ss = userInfo.Split(',');
            
            string phoneNum=ss[0].ToString(); 
            string name=ss[1].ToString();
            string IDCardNum=ss[2].ToString();
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();

                List<ApplicationUser> reservations = db.Users.Where(p => (p.IDCard == IDCardNum) && p.Deleted == false).ToList();
                if (reservations.Count != 0)
                {
                    return "error:用户已存在";
                }
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                ApplicationUser user = new ApplicationUser();
                user.UserName = IDCardNum;
                user.TrueName = name;
                user.PhoneNumber = phoneNum;
                user.IDCard = IDCardNum;
                user.CreatedTime = DateTime.Now;
                user.Deleted = false;
                IdentityResult result = UserManager.Create(user, "123456");

                if (result.Succeeded)
                {
                    IdentityManager im = new IdentityManager();
                    im.AddUserToRole(user.Id, "supplier");
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return "success:用户信息登记成功";
                }
                else {
                    return "error:用户登记失败";
                }
            }
            catch (Exception ex) {
                return "error:用户登记失败" + ex.Message;
            }
            
        }
    }
}