using System.Collections.Generic;

namespace Queue.Entities.Models
{
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public virtual Address Address { get; set; }
        public int CGroupID { get; set; }
        public CGroup CGroup { get; set; }

    }
    public class CGroup
    {
        public int Id { get; set; }
        public string GName { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }

    public class Address
    {
        public int ID { get; set; }
        public string Contury { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Code { get; set; }
        public Contact Contact { get; set; }
    }


}
