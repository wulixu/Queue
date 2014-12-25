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
        /// <summary>
        /// 收料区域所属码头
        /// </summary>
        [Display(Name="所属码头")]
        [Required]
        public Wharfs Wharfs { get; set; }
        /// <summary>
        /// 收料区码头状态
        /// </summary>
        [Display(Name = "码头当前状态")]
        public AreaState AreaState { get; set; }

    }
    /// <summary>
    /// 码头选择
    /// </summary>
    public enum Wharfs
    {
        /// <summary>
        /// 五金类码头
        /// </summary>
        [Display(Name = "五金类码头")]
        五金类码头 = 1,
        /// <summary>
        /// 电子类码头
        /// </summary>
        [Display(Name = "电子类码头")]
        电子类码头
    }

    /// <summary>
    /// 码头状态
    /// </summary>
    public enum AreaState
    {
        [Display(Name = "忙碌的 ")]
        IsBusy = 1,
        [Display(Name = "空闲的")]
        IsFree
    }
}