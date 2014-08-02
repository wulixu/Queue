using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Entities.Models
{

    /// <summary>
    /// 收料区域
    /// </summary>
    [Table("ReceiveArea")]
    public class ReceiveAreaProfile : Entity
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 区域描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Category { get; set; }


    }
}
