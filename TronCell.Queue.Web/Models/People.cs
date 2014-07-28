using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TronCell.Queue.Web.Models
{
    [Table("People")]
    public class People
    {
        public People()
        {
            
        }

        [Key]
        public int PeopleId { get; set; }
        [MaxLength(10)]
        public string UserName { get; set; }
    }
}