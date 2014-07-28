
namespace Queue.Entities.Models
{
    public class Fitting: Repository.Pattern.Ef6.Entity
    {
        public int FittingId { get; set; }
        public int FittingRoomId { get; set; }

        public virtual FittingRoom FittingRoom { get; set; }
    }
}
