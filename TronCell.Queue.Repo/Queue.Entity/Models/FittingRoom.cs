using System.Collections.Generic;

namespace Queue.Entities.Models
{
    public class FittingRoom:Repository.Pattern.Ef6.Entity
    {
        public int FittingRoomId { get; set; }

        public string RoomName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Fitting> Fittings { get; set; }
    }
}
