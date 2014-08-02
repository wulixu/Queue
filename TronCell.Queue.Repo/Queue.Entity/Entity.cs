using System;
namespace Queue.Entities
{
    public abstract class Entity
    {
        public virtual int Id
        {
            get;
            set;
        }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Deleted { get; set; }
        public virtual bool IsTransient()
        {
            return this.Id == default(int);
        }
    }
}
