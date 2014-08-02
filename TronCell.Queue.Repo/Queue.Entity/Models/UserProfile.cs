using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Entities.Models
{
    /// <summary>
    /// 用户表
    /// 管理员,供应商,收料员,经理
    /// </summary>
    [Table("UserProfile")]
    public class UserProfile : Entity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public virtual string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        //[Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码")]
        public virtual string Password { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required(ErrorMessage = "手机号码不能为空")]
        [RegularExpression(@"1[0-9]{10}", ErrorMessage = "手机号码无效")]
        [Display(Name = "电话号码")]
        public string Telephone { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        [Display(Name = "角色")]
        public RoleEnum Role { get; set; }

        [Required(ErrorMessage = "身份证号码不能为空")]
        [Display(Name = "身份证号码")]
        public string IDCard { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        //[Required(ErrorMessage = "公司名称不能为空")]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }

        [Display(Name = "车牌号")]
        public string CarNum { get; set; }
    }

    public enum RoleEnum
    {
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Display(Name = "管理员")]
        Admin,
        /// <summary>
        /// 经理
        /// </summary>
        [Display(Name = "经理")]
        Manager,
        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        Supplier,
        /// <summary>
        /// 采购员
        /// </summary>
        //[Display(Name = "采购员")]
        //Buyer,
        /// <summary>
        /// 收料员
        /// </summary>
        [Display(Name = "收料员")]
        Receiver,
        /// <summary>
        /// 收料组长
        /// </summary>
        //[Display(Name = "收料组长")]
        //ReceiverLeader,
        /// <summary>
        /// 匿名
        /// </summary>
        //[Display(Name = "匿名")]
        //Anonymous
    }
}
