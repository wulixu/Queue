using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TronCell.Queue.Web.Models
{
    [Table("ReceiveArea")]
    public class ReceiveArea
    {
        [Required]
        public int ReceiveAreaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Required(ErrorMessage="区域名称不能为空")]
        [Display(Name="区域名称")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域描述
        /// </summary>
        [Display(Name="描述")]
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name="类型")]
        public string Category { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name="创建时间")]
        public DateTime CreateTime { get; set; }

    }
}