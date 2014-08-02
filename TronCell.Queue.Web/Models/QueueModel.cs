using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TronCell.Queue.Web.Models
{
    /// <summary>
    /// 取号表
    /// </summary>
    [Table("QueueCall")]
    public class QueueCall
    {
        [Key]
        public int QueueCallId { get; set; }
        /// <summary>
        /// 排队号
        /// </summary>
        public string QueueNum { get; set; }
        /// <summary>
        /// 排队状态
        /// </summary>
        public ProcessStatus State { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityStatus Priority { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 排队者ID
        /// </summary>
        public string QueueUserId { get; set; }
        /// <summary>
        /// 关联 排队者实体
        /// </summary>
        [ForeignKey("QueueUserId")]
        public virtual ApplicationUser QueueUser { get; set; }

        /// <summary>
        /// 收料员ID
        /// </summary>
        public string OperationId { get; set; }
        /// <summary>
        /// 关联 收料员实体
        /// </summary>
        [ForeignKey("OperationId")]
        public virtual ApplicationUser Operation { get; set; }
        /// <summary>
        /// 收料区域ID
        /// </summary>
        public int? ReceiveAreaId { get; set; }
        /// <summary>
        /// 关联 收料区域
        /// </summary>
        [ForeignKey("ReceiveAreaId")]
        public virtual ReceiveArea ReceiveArea { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
    }

    /// <summary>
    /// 处理流程状态
    /// </summary>
    public enum ProcessStatus
    {
        /// <summary>
        /// 未取号
        /// </summary>
        [Display(Name = "未取号")]
        NoQueueNumber = 1,
        /// <summary>
        /// 获取队列号
        /// </summary>
        [Display(Name = "等待收料")]
        GotQueueNumber,
        /// <summary>
        /// 货品处理中
        /// </summary>
        [Display(Name = "正在收料")]
        Processing,
        /// <summary>
        /// 处理完毕
        /// </summary>
        [Display(Name = "处理完")]
        Processed,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "延迟处理")]
        LazyProcess
    }

    public enum PriorityStatus {
        /// <summary>
        /// 优先送货
        /// </summary>
        [Display(Name="紧急送货")]
        Urgent=1,
        /// <summary>
        /// 普通送货
        /// </summary>
        [Display(Name="一般送货")]
        General
    }
}